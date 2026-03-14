namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request
{
    public record AvaliacaoRequestDto(
         Guid ProdutoId,
         string Autor,
         string Titulo,
         string Texto,
         int Nota 
     );
}
