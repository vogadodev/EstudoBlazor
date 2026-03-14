using TarefasBlazor.Shared.MODULOS.LOG.Entidades;

namespace TarefasBlazor.Shared.INFRA.LogServices.Services
{
    public class MonitoramentoHandler : DelegatingHandler
    {
        private readonly MonitoramentoStateService _stateService;

        public MonitoramentoHandler(MonitoramentoStateService stateService)
        {
            _stateService = stateService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verifica se o botão "Ativar" foi clicado e se há um ID ativo
            if (_stateService.IsAtivo && !string.IsNullOrEmpty(_stateService.CorrelationIdAtivo))
            {
                // Carimba a requisição HTTP com o ID
                request.Headers.Add(MonitoramentoConstantes.HeaderMonitoramentoId, _stateService.CorrelationIdAtivo);
            }

            // Segue a vida normalmente para o destino
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
