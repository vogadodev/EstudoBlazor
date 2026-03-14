using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Repositories;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories
{
    public class FornecedorRepository<T> : BaseRepository<Fornecedor> where T : BasisDbContextComum<T>
    {
        public FornecedorRepository(T dbContext) : base(dbContext)
        {

        }

        public async Task<List<Fornecedor>> ObterTodosFornecedoresPaginado(int pagina, int qtdItemPagina)
        {
            return await DbSet
                .OrderBy(f => f.NomeFantasia)
                .Skip((pagina - 1) * qtdItemPagina)
                .Take(qtdItemPagina)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
