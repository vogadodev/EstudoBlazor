using TarefasBlazor.Shared.MODULOS.VENDA.Entidades;

namespace TarefasBlazor.Shared.MODULOS.VENDA.DTOs.Response
{
    public record VendaResponseDto(
      Guid Id,
      Guid? ClienteId,
      decimal ValorTotal,
      string StatusVenda,
      string StatusPagamento,
      DateTime DataCriacao,
      DateTime? DataAtualizacao,
      bool EstaAtivo,
      ICollection<ItemVenda> ItensVenda
    );
}
