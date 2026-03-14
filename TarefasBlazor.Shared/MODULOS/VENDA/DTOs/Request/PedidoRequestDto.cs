using TarefasBlazor.Shared.MODULOS.VENDA.DTOs.ContratosMensagem;

namespace TarefasBlazor.Shared.MODULOS.VENDA.DTOs.Request
{
    public record PedidoRequestDto(Guid IdPedido, List<ItemPedidoDto> listaDeProdutos);
}
