namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response
{
    public record AvaliacaoResponseDto(
        Guid Id,
        Guid ProdutoId,
        string Autor,
        string Titulo,
        string Texto,
        int Nota,
        DateTime DataCriacao
    );
}
