using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.CategoriaServices
{
    public class ObterCategoriaService : RetornoPadraoService, IServicoComBuscaPadrao
    {

        private readonly CategoriaRepository<DbContextTarefas> _categoriaRepository;

        public ObterCategoriaService(CategoriaRepository<DbContextTarefas> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public bool Encontrado { get; set; }

        public async Task ObterCategoriaPorNome(string nome)
        {
            var categoria = await _categoriaRepository.SelecionarObjetoAsync(c => c.Nome.Contains(nome));
            if (categoria == null)
                return;
            Encontrado = true;
            Data = CriarCategoriaDto(categoria);
        }

        public async Task ObterCategoriasSubCategirias()
        {
            var categorias = await _categoriaRepository.ObterCategoriasESubCategorias();
            var listaCategoria = categorias.Select(c => 
                            new CategoriaResponseDto(c.Id, c.Nome, c.Descricao,
                                c.Subcategorias.Select( sc =>
                                      new CategoriaResponseDto(sc.Id, sc.Nome, sc.Descricao)).ToList())).ToList();
            if (!listaCategoria.Any())
                return;
            Encontrado = true;
            Data = listaCategoria;
        }

        public async Task ObterTodasCategorias()
        {
            var categorias = await _categoriaRepository.SelecionarTodosAsync();
            var listaCategoria = categorias.Select(c =>
                                               new CategoriaResponseDto(c.Id, c.Nome, c.Descricao)
                                            ).ToList();

            if (!listaCategoria.Any())
                return;

            Encontrado = true;
            Data = listaCategoria;
        }

        private CategoriaResponseDto CriarCategoriaDto(Categoria categoria)
        {
            return new CategoriaResponseDto(categoria.Id, categoria.Nome, categoria.Descricao);
        }


    }
}
