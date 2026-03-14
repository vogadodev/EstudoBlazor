using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using TarefasBlazor.Shared.INFRA.LogServices.Interfaces;
using TarefasBlazor.Shared.MODULOS.LOG.Entidades;

namespace TarefasBlazor.Shared.INFRA.LogServices.Services
{
    public class MonitoramentoSqlInterceptor : DbCommandInterceptor
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMonitoramentoApiService _monitor;

        public MonitoramentoSqlInterceptor(IHttpContextAccessor httpContext, IMonitoramentoApiService monitor)
        {
            _httpContext = httpContext;
            _monitor = monitor;
        }

        // Intercepta chamadas Síncronas (ex: .ToList())
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            LogarSql(command);
            return base.ReaderExecuting(command, eventData, result);
        }

        // Intercepta chamadas Assíncronas (ex: .ToListAsync()) - O MAIS IMPORTANTE
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            LogarSql(command);
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        private void LogarSql(DbCommand command)
        {
            // Tenta pegar o ID da Constante
            var correlationId = _httpContext.HttpContext?.Items["CorrelationId"]?.ToString();

            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                // Formata a query para não quebrar a linha no .txt e poluir o log
                var sqlFormatado = command.CommandText.Replace(Environment.NewLine, " ");
                _monitor.Registrar(correlationId, "SQL EXEC", sqlFormatado);
            }
        }
    }
}
