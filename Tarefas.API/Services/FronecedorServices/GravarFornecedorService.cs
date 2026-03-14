using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.GeradorDeIDsService;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.FronecedorServices
{

    public class GravarFornecedorService : RetornoPadraoService
    {
        private readonly FornecedorRepository<DbContextTarefas> _fornecedorRepository;
        private readonly ValidarFornecedorService _validarFornecedorService;

        public GravarFornecedorService(
              FornecedorRepository<DbContextTarefas> fornecedorRepository
            , ValidarFornecedorService validarFornecedorService)
        {
            _fornecedorRepository = fornecedorRepository;
            _validarFornecedorService = validarFornecedorService;
        }
        public async Task GravarFornecedor(FornecedorRequestDto dto)
        {
            var fornecedorExistente = await _fornecedorRepository.SelecionarObjetoAsync(f => f.Id == dto.Id);
            var ehAtualizacao = fornecedorExistente != null;

            var dtoTemErro = await _validarFornecedorService.Validar(dto, ehAtualizacao);
            if (dtoTemErro)
            {
                Mensagens.AddRange(_validarFornecedorService.Mensagens);
                return;
            }
           
            if (ehAtualizacao)
            {
                PreencherFornecedor(fornecedorExistente!, dto);
                _fornecedorRepository.DbSet.Update(fornecedorExistente!);
            }
            else
            {
                var novoFornecedor = new Fornecedor() { Id = CriarIDService.CriarNovoID() };
                PreencherFornecedor(novoFornecedor, dto);
            }

            await _fornecedorRepository.DbContext.SaveChangesAsync();
        }
        private void PreencherFornecedor(Fornecedor fornecedor , FornecedorRequestDto dto)
        {
            fornecedor.RazaoSocial = dto.RazaoSocial;
            fornecedor.NomeContato = dto.NomeContato;
            fornecedor.Telefone = dto.Telefone;
            fornecedor.NomeFantasia = dto.NomeFantasia;
            fornecedor.CNPJ = dto.CNPJ;
        }
    }
}
