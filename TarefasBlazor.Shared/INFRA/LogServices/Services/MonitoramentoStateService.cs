namespace TarefasBlazor.Shared.INFRA.LogServices.Services
{
    public class MonitoramentoStateService
    {
        private static string? _correlationIdAtivo;

        public string? CorrelationIdAtivo
        {
            get => _correlationIdAtivo;
            private set => _correlationIdAtivo = value;
        }

        public bool IsAtivo => !string.IsNullOrWhiteSpace(CorrelationIdAtivo);

        public event Action? OnChange;
        public void Ativar()
        {
            // Gera um ID único automaticamente para esta "sessão" de monitoramento
            CorrelationIdAtivo = Guid.NewGuid().ToString("N");
            NotifyStateChanged();
        }

        public void Desativar()
        {
            CorrelationIdAtivo = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
