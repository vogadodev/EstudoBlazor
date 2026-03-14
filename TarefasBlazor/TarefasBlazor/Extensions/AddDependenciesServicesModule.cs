using TarefasBlazor.Services.EstoqueServices.Interfaces;
using TarefasBlazor.Services.EstoqueServices.Services;
using TarefasBlazor.Shared.INFRA.LogServices.Services;

namespace TarefasBlazor.Extensions
{
    public static class AddDependenciesServicesModule
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IObterCategorias, ObterCategoriaService>();
            services.AddScoped<MonitoramentoStateService>();
            services.AddTransient<MonitoramentoHandler>();
         
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            
            return services;
        }
    }
}
