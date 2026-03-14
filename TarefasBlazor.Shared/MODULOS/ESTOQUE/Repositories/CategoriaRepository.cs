using Microsoft.EntityFrameworkCore;
using TarefasBlazor.Shared.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Repositories;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;

namespace TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories
{
    public class CategoriaRepository <T>: BaseRepository<Categoria> where T : BasisDbContextComum<T>
    {
        public CategoriaRepository(T context) : base(context) { }
        public async Task<bool> NomeJaExiste(string nome, Guid? id = null) => id.HasValue ? await DbSet.AnyAsync(c => c.Nome == nome && c.Id != id.Value) : await DbSet.AnyAsync(c => c.Nome == nome);
        
        public async Task<List<Categoria>> ObterCategoriasESubCategorias()
        {
            return await DbSet.Include(c => c.Subcategorias).Where(c=> c.CategoriaPaiId == null).ToListAsync();
        }
    
    }
}
