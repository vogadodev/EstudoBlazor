using TarefasBlazor.Services.EstoqueServices.Interfaces;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response;

namespace TarefasBlazor.Services.EstoqueServices.Services
{
    public class ObterCategoriaService : IObterCategorias
    {
        private readonly HttpClient _http;

        public ObterCategoriaService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("TarefaApi");
        }

        public async Task<ApiResponse<CategoriaResponseDto>> ObterCategoriaAsync(string termo)
        {
            try
            {
                // O termo é passado na URL conforme esperado pelo endpoint
                return await _http.GetFromJsonAsync<ApiResponse<CategoriaResponseDto>>($"/api/v1/Categoria/{termo}");
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoriaResponseDto> { Mensagens = new List<string> { ex.Message } };
            }
        }
    }
}
