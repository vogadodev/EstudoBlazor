using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Response;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.FronecedorServices
{
    public class ObterFornecedorService : RetornoPadraoService, IServicoComBuscaPadrao
    {
        private readonly FornecedorRepository<DbContextTarefas> _fornecedorRepository;

        public ObterFornecedorService(FornecedorRepository<DbContextTarefas> fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;           
        }

        public bool Encontrado { get ; set ; }

        public async Task ObterFornecedorPorNomeFantasia(string nomeFantasia)
        {
            var fornecedor = await _fornecedorRepository.SelecionarObjetoAsync(f=> f.NomeFantasia == nomeFantasia);
            
            if(fornecedor == null) 
               return;
            
            Encontrado = true;
            Data = CriarFornecedorDto(fornecedor);
        }

        public async Task ObterTodosFornecedorPaginado(int pagina, int qtdItemPagina)
        {
            var listaFornecedores = await _fornecedorRepository.ObterTodosFornecedoresPaginado(pagina, qtdItemPagina);
            if(!listaFornecedores.Any())
                return;

            Encontrado = true;
            Data = listaFornecedores.Select(f=> 
                    CriarFornecedorDto(f)
                    ).ToList();
        }

        private FornecedorResponseDto CriarFornecedorDto(Fornecedor fornecedor)
        {
            return new FornecedorResponseDto(
                fornecedor.Id,
                fornecedor.RazaoSocial,
                fornecedor.NomeContato,
                fornecedor.Telefone,
                fornecedor.NomeFantasia,
                fornecedor.CNPJ,
                fornecedor.Email
                );
        }
    }
}
