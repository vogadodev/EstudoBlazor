using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;


namespace Tarefas.API.Services.CategoriaServices
{
    public class ExcluirCategoriaService : RetornoPadraoService, IServicoDeExclusaoPadrao
    {
        private readonly CategoriaRepository<DbContextTarefas> _categoriaRepository;
        public ExcluirCategoriaService(CategoriaRepository<DbContextTarefas> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task ExcluirCategoria(Guid categoriaID)
        {
            var categoria = await _categoriaRepository.SelecionarObjetoAsync(c => c.Id == categoriaID);
            if (categoria == null)
                return;

                 _categoriaRepository.DbSet.Remove(categoria);
           await _categoriaRepository.DbContext.SaveChangesAsync();
        }

    }
}
