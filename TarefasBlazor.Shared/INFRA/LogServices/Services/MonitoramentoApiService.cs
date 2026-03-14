using Microsoft.Extensions.Caching.Memory;
using TarefasBlazor.Shared.INFRA.LogServices.Interfaces;
using TarefasBlazor.Shared.MODULOS.LOG.Entidades;

namespace TarefasBlazor.Shared.INFRA.LogServices.Services
{
    public class MonitoramentoApiService : IMonitoramentoApiService
    {
        private readonly IMemoryCache _cache;

        public MonitoramentoApiService(IMemoryCache cache) => _cache = cache;

        public void Registrar(string correlationId, string categoria, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(correlationId)) return;

            // Tenta pegar a lista existente, se não, cria uma nova
            if (!_cache.TryGetValue(correlationId, out List<LogEventoDto>? logs) || logs == null)
            {
                logs = new List<LogEventoDto>();
            }

            lock (logs) // Lock simples para evitar erro de concorrência na lista
            {
                logs.Add(new LogEventoDto
                {
                    Categoria = categoria,
                    Mensagem = mensagem,
                    DataHora = DateTime.Now // Certifique-se que o DTO tem esse campo
                });
            }

            // Importante: Setar novamente para renovar o tempo de expiração
            _cache.Set(correlationId, logs, TimeSpan.FromMinutes(20));
        }
        public List<LogEventoDto> ObterLogs(string correlationId)
        {
            _cache.TryGetValue(correlationId, out List<LogEventoDto>? logs);
            return logs ?? new List<LogEventoDto>();
        }

        public void Limpar(string correlationId) => _cache.Remove(correlationId);
    }
}
