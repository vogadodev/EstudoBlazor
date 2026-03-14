using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Repositories;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories
{
    public class MarcaRepository<T> : BaseRepository<Marca>  where T: BasisDbContextComum<T>
    {
        public MarcaRepository(T context) : base(context) { }
        public async Task<bool> NomeJaExiste(string nome, Guid? id = null) => 
            id.HasValue ? await DbSet.AnyAsync(m => m.Nome == nome && m.Id != id.Value)
                        : await DbSet.AnyAsync(m => m.Nome == nome);
        public async Task<List<Marca>> ObterTodasMarcasAsync(int pagina, int qtdItensPagina) => await DbSet.Skip((pagina - 1) * qtdItensPagina).Take(qtdItensPagina).ToListAsync();
    }
}
