using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using TarefasBlazor.Shared.INFRA.LogServices.Interfaces;
using TarefasBlazor.Shared.MODULOS.LOG.Entidades;

namespace TarefasBlazor.Shared.INFRA.LogServices.Services
{
    public class MonitoramentoHttpMiddleware
    {
        private readonly RequestDelegate _next;

        public MonitoramentoHttpMiddleware(RequestDelegate next) => _next = next;

        // Note que aqui já estou usando a sua interface correta: IMonitoramentoService
        public async Task InvokeAsync(HttpContext context, IMonitoramentoApiService monitor)
        {
            // Tenta ler o ID enviado pelo Blazor através do MonitoramentoHandler
            var correlationId = context.Request.Headers[MonitoramentoConstantes.HeaderMonitoramentoId].ToString();

            // Se não tiver o header (monitoramento desligado na tela), passa reto
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                await _next(context);
                return;
            }

            // GUARDA O ID NO CONTEXTO: 
            // Assim as outras classes podem acessá-lo sem precisar dele no parâmetro!
            context.Items["CorrelationId"] = correlationId;

            var sw = Stopwatch.StartNew();
            var rota = $"{context.Request.Method} {context.Request.Path}";

            monitor.Registrar(correlationId, "HTTP IN", $"Iniciando: {rota}");

            try
            {
                await _next(context);

                sw.Stop();
                var status = context.Response.StatusCode;
                monitor.Registrar(correlationId, status >= 400 ? "HTTP AVISO" : "HTTP OK", $"Finalizado: {rota} | Status: {status} | Tempo: {sw.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                sw.Stop();
                monitor.Registrar(correlationId, "HTTP ERRO", $"Falha em {rota}: {ex.Message}");
                throw;
            }
        }
    }
}
