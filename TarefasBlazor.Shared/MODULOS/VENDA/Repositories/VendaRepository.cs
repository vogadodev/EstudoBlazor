using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Repositories;
using TarefasBlazor.Shared.MODULOS.VENDA.Entidades;

namespace TarefasBlazor.Shared.MODULOS.VENDA.Repositories
{
    public class VendaRepository<T> : BaseRepository<Venda> where T : BasisDbContextComum<T>
    {
        public VendaRepository(T context) : base(context) { }
        
        public async Task<Venda?> ObterVendaCompletaAsync(Guid id)
        {
            return await DbSet
                .Include(v => v.ItensVenda)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }
     
        public async Task<IEnumerable<Venda>> ObterVendasPorClienteAsync(Guid clienteId)
        {
            return await DbSet
                .Include(v => v.ItensVenda)
                .Where(v => v.ClienteId == clienteId)
                .OrderByDescending(v => v.DataCriacao)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
