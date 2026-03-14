namespace TarefasBlazor.Shared.MODULOS.VENDA.DTOs.Response
{
    public record PedidoResponseDto(Guid IdPedido, decimal ValorTotal, DateTime DataDoPedido 
    );
    
}
