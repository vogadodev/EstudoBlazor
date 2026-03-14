using Microsoft.AspNetCore.Mvc;
using Tarefas.API.Services.CategoriaServices;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;

namespace Tarefas.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly GravarCategoriaService _gravarCategoriaService;
        private readonly ObterCategoriaService _obterCategoriaService;
        private readonly ExcluirCategoriaService _excluirCategoriaService;

        public CategoriaController(
              GravarCategoriaService gravarCategoriaService
            , ObterCategoriaService obterCategoriaService
            , ExcluirCategoriaService excluirCategoriaService)
        {
            _gravarCategoriaService = gravarCategoriaService;
            _obterCategoriaService = obterCategoriaService;
            _excluirCategoriaService = excluirCategoriaService;
        }
        
        [HttpPost]
        public async Task<IActionResult> GravarCategoria(CategoriaRequestDto dto)
        {
            await _gravarCategoriaService.GravarCategoria(dto);
            return _gravarCategoriaService.ResponderRequest(this);
        }        
        [HttpGet("{nome}")]
        public async Task<IActionResult>ObterCategoriaPorNome(string nome)
        {
            await _obterCategoriaService.ObterCategoriaPorNome(nome);
            return _obterCategoriaService.ResponderRequest(this);
        }        
        [HttpGet("obterTodasComSubCategorias")]
        public async Task<IActionResult> ObterTodasComSubCategorias()
        {
            await _obterCategoriaService.ObterCategoriasSubCategirias();
            return _obterCategoriaService.ResponderRequest(this);
        }       
        [HttpGet("obterTodasSemSubCategorias")]
        public async Task<IActionResult> ObterTodasSemSubCategorias()
        {
            await _obterCategoriaService.ObterTodasCategorias();
            return _obterCategoriaService.ResponderRequest(this);
        }
        
        [HttpDelete("{categoriaId}")]
        public async Task<IActionResult> ExcluirCategoria(Guid categoriaId)
        {
            await _excluirCategoriaService.ExcluirCategoria(categoriaId);
            return _excluirCategoriaService.ResponderRequest(this);
        }
    }
}
