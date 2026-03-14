using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.INFRA.ServicesComum.ServicoComMensagemService;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.IntegracaoApiService
{    
    public sealed class ConsumirApiExternaService : MensagemService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ConsumirApiExternaService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;        
        public ConsumirApiExternaService(HttpClient httpClient, ILogger<ConsumirApiExternaService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Configura o endereço base do Gateway uma única vez.
            _httpClient.BaseAddress = new Uri(AmbienteUrlPadraoService.UrlPadraoService());

            // Configura o timeout.
            _httpClient.Timeout = TimeSpan.FromMinutes(1);

            // Opções para desserialização case-insensitive
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<RetornoPadraoService?> Get(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                return await HandleResponse(response);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                var errorMessage = $"Erro de comunicação ao acessar o endpoint GET '{endpoint}'.";
                _logger.LogError(ex, errorMessage);
                Mensagens.AdicionarErro($"{errorMessage} Detalhes: {ex.Message}");
                return null;
            }
        }

        public async Task<RetornoPadraoService?> Post(string endpoint, object data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                return await HandleResponse(response);
            }
            catch (Exception ex) 
            {
                var errorMessage = $"Erro de comunicação ao acessar o endpoint POST '{endpoint}'.";
                _logger.LogError(ex, errorMessage);
                Mensagens.AdicionarErro($"{errorMessage} Detalhes: {ex.Message}");
                return null;
            }
        }

        public async Task<RetornoPadraoService?> PostComFiles(string endpoint, object data, Dictionary<string, Stream> files)
        {
            using var multipartContent = new MultipartFormDataContent();

            // Adiciona o payload JSON
            multipartContent.Add(new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"), "jsonData");

            // Adiciona os arquivos
            foreach (var (key, stream) in files)
            {
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                multipartContent.Add(streamContent, key, $"file_{Guid.NewGuid()}");
            }

            try
            {
                var response = await _httpClient.PostAsync(endpoint, multipartContent);
                return await HandleResponse(response);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erro de comunicação ao enviar arquivos para o endpoint '{endpoint}'.";
                _logger.LogError(ex, errorMessage);
                Mensagens.AdicionarErro($"{errorMessage} Detalhes: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Método centralizado para tratar a resposta HTTP, status code e corpo da mensagem.
        /// </summary>
        private async Task<RetornoPadraoService?> HandleResponse(HttpResponseMessage response)
        {
            // 1. Tratamento para respostas de SUCESSO (2xx)
            if (response.IsSuccessStatusCode)
            {
                // Status 204 No Content - Sucesso, mas sem corpo de resposta.
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return new RetornoPadraoService();
                }

                try
                {                  
                    var retornoSucesso = await response.Content.ReadFromJsonAsync<RetornoPadraoService>(_jsonOptions);

                    if (retornoSucesso?.Mensagens != null && retornoSucesso.Mensagens.Any())
                    {
                        Mensagens.AddRange(retornoSucesso.Mensagens);
                    }

                    return retornoSucesso;
                }
                catch (JsonException ex)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError(ex, "Falha ao desserializar uma resposta de sucesso. Corpo: {ResponseBody}", responseBody);
                    Mensagens.AdicionarErro("A resposta da API foi recebida com sucesso, mas seu formato é inválido.");
                    return null;
                }
            }

            // 2. Tratamento para respostas de ERRO (4xx e 5xx)
            var errorBody = await response.Content.ReadAsStringAsync();

            // Tenta desserializar o corpo do erro, pois ele pode conter um `RetornoPadraoService` com mensagens.
            try
            {
                var retornoErro = JsonSerializer.Deserialize<RetornoPadraoService>(errorBody, _jsonOptions);
                if (retornoErro?.Mensagens != null && retornoErro.Mensagens.Any())
                {
                    Mensagens.AddRange(retornoErro.Mensagens); 
                    return null;
                }
            }
            catch (JsonException ex)
            {
                Mensagens.AdicionarErro(ex.Message);
            }

            // 3. Fallback: Se não foi possível extrair mensagens do corpo, cria uma mensagem genérica baseada no StatusCode.
            var mensagem = $"A API retornou um erro inesperado. Código: {(int)response.StatusCode} ({response.ReasonPhrase}).";
            _logger.LogWarning("Resposta de erro da API sem corpo detalhado. Endpoint: {RequestUri}. Status: {StatusCode}. Corpo: {ErrorBody}",
                response.RequestMessage?.RequestUri, response.StatusCode, errorBody);
            Mensagens.AdicionarErro(mensagem);
            return null;
        }
    }
}