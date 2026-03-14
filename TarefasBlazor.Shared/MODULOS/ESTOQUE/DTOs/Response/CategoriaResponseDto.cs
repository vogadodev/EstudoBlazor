namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response
{
    public record CategoriaResponseDto(Guid Id, string Nome, string Descricao, List<CategoriaResponseDto>? listaSubCategoria = null);
}
