using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.MarcaServices
{
    public class ExcluirMarcaService:RetornoPadraoService, IServicoDeExclusaoPadrao
    {
        private readonly MarcaRepository<DbContextTarefas> _marcaRepository;

        public ExcluirMarcaService(MarcaRepository<DbContextTarefas> marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task ExcluirMarca(Guid marcaId)
        {
            var marca = await _marcaRepository.SelecionarObjetoAsync(m=> m.Id == marcaId);
            if (marca == null)
                return;

            _marcaRepository.DbSet.Remove(marca);
            await _marcaRepository.DbContext.SaveChangesAsync();
        }
    }
}
