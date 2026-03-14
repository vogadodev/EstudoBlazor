using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.ProdutoServices
{
    public class ExcluirProdutoService:RetornoPadraoService , IServicoDeExclusaoPadrao
    {
        private readonly ProdutoRepository<DbContextTarefas> _produtoRepository;
        public ExcluirProdutoService(ProdutoRepository<DbContextTarefas> produtoReposiotory)
        {
            _produtoRepository = produtoReposiotory;
            
        }

        public async Task ExcluirProduto(Guid produtoId)
        {
            var produto = await _produtoRepository.SelecionarObjetoAsync(p=> p.Id == produtoId);
            if (produto == null)
                return;
            _produtoRepository.DbSet.Remove(produto);
            await _produtoRepository.DbContext.SaveChangesAsync();
        }
    }
}
