using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Repositories;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories
{
    public class AvaliacaoRepository<T> : BaseRepository<Avaliacao> where T : BasisDbContextComum<T>
    {
        public AvaliacaoRepository(T context) : base(context) { }
        public async Task<bool> ProdutoExiste(Guid produtoId) => await DbSet.AnyAsync(p => p.Id == produtoId);
    }
}
