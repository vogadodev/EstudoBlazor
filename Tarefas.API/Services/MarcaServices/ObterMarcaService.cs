using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.MarcaServices
{
    public class ObterMarcaService : RetornoPadraoService, IServicoComBuscaPadrao
    {
        private readonly MarcaRepository<DbContextTarefas> _MarcaRepository;

        public bool Encontrado { get; set; }
        public ObterMarcaService(MarcaRepository<DbContextTarefas> marcaRepository)
        {
            _MarcaRepository = marcaRepository;
        }

        public async Task ObterPorNome(string nome)
        {
            var marca = await _MarcaRepository.SelecionarListaObjetoAsync(m=> m.Nome.Contains(nome));
           
            if (!marca.Any())            
                   return;
            
            Encontrado = true;
            Data = marca;
        }

        public async Task ObterTodas(int pagina, int qtdItensPagina)
        {
            var marca = await _MarcaRepository.ObterTodasMarcasAsync(pagina, qtdItensPagina);
            if (!marca.Any())
            {
                return;
            }
            Encontrado = true;
            Data = marca;
        }
    }
}
