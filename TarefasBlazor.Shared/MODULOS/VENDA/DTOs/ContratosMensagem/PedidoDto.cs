namespace TarefasBlazor.Shared.MODULOS.VENDA.DTOs.ContratosMensagem
{
    public record PedidoDto(
    Guid IdPedido,
    string IdCliente,
    List<ItemPedidoDto> Itens
);
}
