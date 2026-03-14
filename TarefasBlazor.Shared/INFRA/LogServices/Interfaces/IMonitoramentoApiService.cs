using TarefasBlazor.Shared.MODULOS.LOG.Entidades;

namespace TarefasBlazor.Shared.INFRA.LogServices.Interfaces
{
    public interface IMonitoramentoApiService
    {
        void Registrar(string correlationId, string categoria, string mensagem);
        List<LogEventoDto> ObterLogs(string correlationId);
        void Limpar(string correlationId);
    }
}
