using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarefas.API.Services.MarcaServices;
using TarefasBlazor.Shared.INFRA.ServicesComum.AuthServices;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;

namespace Tarefas.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MarcaController : ControllerBase
    {
        private readonly GravarMarcaService _gravarMarcaService;
        private readonly ObterMarcaService _obterMarcaService;
        private readonly ExcluirMarcaService _excluirMarcaService;

        public MarcaController(
              GravarMarcaService gravarMarcaService
            , ObterMarcaService obterMarcaService
            , ExcluirMarcaService excluirMarcaService)
        {
            _gravarMarcaService = gravarMarcaService;
            _obterMarcaService = obterMarcaService;
            _excluirMarcaService = excluirMarcaService;
        }

        [Authorize(Policy = PoliciesTipoUsuario.ApenasAdm)]
        [HttpPost]
        public async Task<IActionResult> GravarMarca(MarcaRequestDto dto)
        {
            await _gravarMarcaService.GravarMarca(dto);
            return _gravarMarcaService.ResponderRequest(this);

        }

        [Authorize(Policy = PoliciesTipoUsuario.Todos)]
        [HttpGet("{nome}")]
        public async Task<IActionResult> ObterMarcaPorNome(string nome)
        {
            await _obterMarcaService.ObterPorNome(nome);
            return _obterMarcaService.ResponderRequest(this);
        }
        [Authorize(Policy = PoliciesTipoUsuario.Todos)]
        [HttpGet("{pagina}/{qtdItensPagina}")]
        public async Task<IActionResult> ObterTodasMarcasPaginado(int pagina, int qtdItensPagina)
        {
            await _obterMarcaService.ObterTodas(pagina, qtdItensPagina);
            return _obterMarcaService.ResponderRequest(this);
        }

        [Authorize(Policy = PoliciesTipoUsuario.ApenasAdm)]
        [HttpDelete("{idMarca}")]
        public async Task<IActionResult> ExcluirMarca(Guid idMarca)
        {
            await _excluirMarcaService.ExcluirMarca(idMarca);
            return _excluirMarcaService.ResponderRequest(this);
        }

    }
}
