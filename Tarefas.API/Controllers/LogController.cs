using Microsoft.AspNetCore.Mvc;
using System.Text;
using TarefasBlazor.Shared.INFRA.LogServices.Interfaces;

namespace Tarefas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IMonitoramentoApiService _monitoramentoService;

        public LogController(IMonitoramentoApiService monitoramentoService)
        {
            _monitoramentoService = monitoramentoService;
        }

        // Busca os logs de um processo específico na tela
        [HttpGet("{correlationId}")]
        public IActionResult ObterLogs(string correlationId)
        {
            var logs = _monitoramentoService.ObterLogs(correlationId);

            if (logs == null || !logs.Any())
                return NotFound(new { Mensagem = "Nenhum log encontrado para este Correlation ID." });

            return Ok(logs);
        }

        // Gera e baixa o arquivo .txt com o histórico do Correlation ID
        [HttpGet("{correlationId}/download")]
        public IActionResult BaixarLog(string correlationId)
        {
            var logs = _monitoramentoService.ObterLogs(correlationId);

            if (logs == null || !logs.Any())
                return NotFound("Nenhum log para exportar.");

            var sb = new StringBuilder();
            sb.AppendLine($"--- LOGS DO CORRELATION ID: {correlationId} ---");
            sb.AppendLine($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine(new string('-', 50));

            foreach (var log in logs)
            {
                // Ajuste as propriedades conforme sua classe LogEventoDto
                sb.AppendLine($"[{log.DataHora:HH:mm:ss}] [{log.Categoria}] {log.Mensagem}");
            }

            var fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"log_{correlationId}_{DateTime.Now:yyyyMMdd_HHmm}.txt";

            return File(fileBytes, "text/plain", fileName);
        }

        [HttpDelete("{correlationId}")]
        public IActionResult LimparLogs(string correlationId)
        {
            _monitoramentoService.Limpar(correlationId);
            return NoContent();
        }
    }
}
