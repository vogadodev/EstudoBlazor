namespace TarefasBlazor.Shared.MODULOS.VENDA.DTOs.ContratosMensagem
{
    public record PedidoNaoConcluidoDto(
    PedidoDto PedidoOriginal,
    List<ItemInvalidoDto> ItensIndisponiveis
);
}
