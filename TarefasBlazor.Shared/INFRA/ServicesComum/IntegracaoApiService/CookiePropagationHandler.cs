using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.IntegracaoApiService
{
    /// <summary>
    /// Este handler intercepta requisições do HttpClient e propaga
    /// os cookies de autenticação (AccessToken e RefreshToken) da 
    /// requisição HTTP atual (recebida) para a requisição HTTP de saída.
    /// </summary>
    public class CookiePropagationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;

        public CookiePropagationHandler(
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtSettings> jwtSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtSettings = jwtSettings.Value;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Verifica se estamos em um contexto de requisição HTTP
            if (httpContext != null)
            {
                // Tenta ler os cookies da requisição RECEBIDA
                var accessToken = httpContext.Request.Cookies[_jwtSettings.AccessTokenCookieName];
                var refreshToken = httpContext.Request.Cookies[_jwtSettings.RefreshTokenCookieName];

                var cookies = new List<string>();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    cookies.Add($"{_jwtSettings.AccessTokenCookieName}={accessToken}");
                }
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    cookies.Add($"{_jwtSettings.RefreshTokenCookieName}={refreshToken}");
                }

                // Se encontrou cookies, formata e adiciona no header "Cookie" da requisição de SAÍDA
                if (cookies.Any())
                {
                    request.Headers.Add("Cookie", string.Join("; ", cookies));
                }
            }

            // Continua com a requisição
            return base.SendAsync(request, cancellationToken);
        }
    }
}