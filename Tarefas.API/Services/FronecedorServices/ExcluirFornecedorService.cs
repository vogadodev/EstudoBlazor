using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.FronecedorServices
{
    public class ExcluirFornecedorService : RetornoPadraoService, IServicoDeExclusaoPadrao
    {
        private readonly FornecedorRepository<DbContextTarefas> _fornecedorRepository;

        public ExcluirFornecedorService(FornecedorRepository<DbContextTarefas> fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }
        public async Task ExcluirFornecedor(Guid idFornecedor)
        {
            var fornecedor = await _fornecedorRepository.SelecionarObjetoAsync(f => f.Id == idFornecedor);
            if (fornecedor == null)
                return;

            _fornecedorRepository.DbSet.Remove(fornecedor);
            await _fornecedorRepository.DbContext.SaveChangesAsync();
        }
    }
}
