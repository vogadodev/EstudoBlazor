namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request
{
    public record CategoriaRequestDto(string Nome, string Descricao, Guid? id, Guid? paiId);
}
