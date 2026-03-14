using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TarefasBlazor.Shared.INFRA.LogServices.Interfaces;
using TarefasBlazor.Shared.INFRA.LogServices.Services;
using TarefasBlazor.Shared.INFRA.MiddlewaresGlobais;
using TarefasBlazor.Shared.INFRA.RabbitMQServices.Interfaces;
using TarefasBlazor.Shared.INFRA.RabbitMQServices.Services;
using TarefasBlazor.Shared.INFRA.ServicesComum.AuthServices;
using TarefasBlazor.Shared.INFRA.ServicesComum.IntegracaoApiService;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;

namespace TarefasBlazor.Shared.INFRA
{
    public static class InfrastructureComum
    {
        public static IServiceCollection AddInfraServicosComum(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServicesComum(configuration);
            services.AddCorsComum(configuration);
            services.AddJsonConfigComum(configuration);
            services.AddHttpClient();
            services.AddJwtAuthentication(configuration);
            services.AddAuthorizationPolices();

            return services;
        }

        #region INJEÇÃO DE METODOS AUTOMÁTICOS

        private static IServiceCollection AddServicesComum(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddSingleton<IMonitoramentoApiService, MonitoramentoApiService>();
            services.AddScoped(typeof(ConsumirApiExternaService));
            services.AddTransient(typeof(CookiePropagationHandler));
            services.AddHttpClient<ConsumirApiExternaService>()
                        .AddHttpMessageHandler<CookiePropagationHandler>();
            services.AddSingleton<IMessageBusService, RabbitMQService>();

            return services;
        }

        private static IServiceCollection AddJsonConfigComum(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            return services;
        }

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtOptions);

            services.AddSingleton(jwtOptions);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[jwtOptions.AccessTokenCookieName];
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        private static IServiceCollection AddAuthorizationPolices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PoliciesTipoUsuario.Todos, policy =>
                    policy.RequireAuthenticatedUser());

                options.AddPolicy(PoliciesTipoUsuario.ApenasAdm, policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("UserType", "Adm"));
            });
            return services;
        }

        private static IServiceCollection AddCorsComum(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    string[] origins = { "http://avanade.ecommerce.com.br", "https://avanade.apigateway.com.br" };
                    builder
                     .WithOrigins(origins)
                     .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            return services;
        }
        #endregion

        #region INJEÇÃO MANUAL        
        public static void AddDbContextConfiguration<TDbContext>(
    this IServiceCollection services,
    IConfiguration configuration,
    string connectionStringName) where TDbContext : DbContext
        {
            // 1. Registra o interceptador no contêiner de dependências
            services.AddScoped<MonitoramentoSqlInterceptor>();

            // 2. Configura o DbContext e liga o interceptador
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                // Pega o interceptador pronto (com o HttpContext e Monitor injetados)
                var interceptor = serviceProvider.GetRequiredService<MonitoramentoSqlInterceptor>();

                options.UseSqlServer(configuration.GetConnectionString(connectionStringName))
                       .AddInterceptors(interceptor);
            });
        }

        public static IApplicationBuilder AddCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<MonitoramentoHttpMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
        #endregion
    }
}
