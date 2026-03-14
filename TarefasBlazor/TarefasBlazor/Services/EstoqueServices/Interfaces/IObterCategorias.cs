using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response;

namespace TarefasBlazor.Services.EstoqueServices.Interfaces
{
    public interface IObterCategorias
    {
        Task<ApiResponse<CategoriaResponseDto>> ObterCategoriaAsync(string categoria);
    }
}
