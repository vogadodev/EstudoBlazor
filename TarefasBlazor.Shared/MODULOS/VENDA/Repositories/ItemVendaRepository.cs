using TarefasBlazor.Shared.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Repositories;
using TarefasBlazor.Shared.MODULOS.VENDA.Entidades;

namespace TarefasBlazor.Shared.MODULOS.VENDA.Repositories
{
    public class ItemVendaRepository<T> : BaseRepository<ItemVenda> where T : BasisDbContextComum<T>
    {
        public ItemVendaRepository(T context) : base(context) { }
    }
}
