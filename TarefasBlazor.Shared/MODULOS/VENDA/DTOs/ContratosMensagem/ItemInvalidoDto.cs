namespace TarefasBlazor.Shared.MODULOS.VENDA.DTOs.ContratosMensagem
{
    public record ItemInvalidoDto(
    Guid IdProduto,
    int QuantidadeSolicitada,
    int QuantidadeEmEstoque,
    string Motivo = "Estoque Insuficiente"
);
}
