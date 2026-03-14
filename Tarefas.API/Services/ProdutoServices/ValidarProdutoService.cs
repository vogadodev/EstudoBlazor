using AVANADE.MODULOS.Modulos.AVANADE_ESTOQUE.Resourcers;
using Tarefas.API.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;
using TarefasBlazor.Shared.INFRA.ServicesComum.ServicoComMensagemService;

namespace Tarefas.API.Services.ProdutoServices
{
    public class ValidarProdutoService : MensagemService
    {
        private readonly ProdutoRepository<DbContextTarefas> _produtoRepository;
        private readonly FornecedorRepository<DbContextTarefas> _fornecedorRepository;
        private readonly MarcaRepository<DbContextTarefas> _marcaRepository;

        public ValidarProdutoService(
              ProdutoRepository<DbContextTarefas> produtoRepository
            , FornecedorRepository<DbContextTarefas> fornecedorRepository
            , MarcaRepository<DbContextTarefas> marcaRepository)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _marcaRepository = marcaRepository;
        }
        public async Task<bool> ValidarProduto(ProdutoRequestDto dto, bool ehAtualizacao)
        {
            ValidarCampoNomeObrigatorio(dto);
            ValidarCampoDescricaoObrigatorio(dto);
            ValidarCampoPrecoObrigatorio(dto);
            ValidarCampoCodigoUnicoObrigatorio(dto);
            ValidarCampoFornecedorObrigatorio(dto);
            ValidarCampoEstaEmPromocaoObrigatorio(dto);
            ValidarCampoTemFreteGratisObrigatorio(dto);
            ValidarCampoQuantidadeEmEstoqueObrigatorio(dto);
            ValidarCampoEstaAtivoObrigatorio(dto);
            ValidarCampoMarcaObrigatorio(dto);
            ValidarCampoCategoriaObrigatorio(dto);

            if (!Mensagens.TemErros())
            {               
                await ValidarSeCategoriaExiste(dto);
                await ValidarSeFornecedorExiste(dto);
            }

            if (!Mensagens.TemErros() && !ehAtualizacao)
            {
                await ValidarSeCodigoUnicoExiste(dto);
            }

            return Mensagens.TemErros();
        }

        private void ValidarCampoNomeObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(string.IsNullOrEmpty(dto.Nome), ProdutoResourcer.NomeObrigatorio);
        }

        private void ValidarCampoDescricaoObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(string.IsNullOrEmpty(dto.Descricao), ProdutoResourcer.DescricaoObrigatorio);
        }

        private void ValidarCampoPrecoObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.Preco <= 0, ProdutoResourcer.PrecoObrigatorio);
        }

        private void ValidarCampoCodigoUnicoObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(string.IsNullOrEmpty(dto.CodigoUnico), ProdutoResourcer.CodigoUnicoObrigatorio);
        }

        private void ValidarCampoFornecedorObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.FornecedorId == Guid.Empty || dto.FornecedorId == null, ProdutoResourcer.FornecedorObrigatorio);
        }

        private void ValidarCampoEstaEmPromocaoObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.EstaEmPromocao.GetType() != typeof(bool), ProdutoResourcer.EstaEmPromocaoObrigatorio);
        }

        private void ValidarCampoTemFreteGratisObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.TemFreteGratis.GetType() != typeof(bool), ProdutoResourcer.TemFreteGratisObrigatorio);
        }

        private void ValidarCampoQuantidadeEmEstoqueObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.QuantidadeEstoque <= 0, ProdutoResourcer.QuantidadeEstoqueObrigatorio);
        }

        private void ValidarCampoEstaAtivoObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.EstaAtivo.GetType() != typeof(bool), ProdutoResourcer.EstaAtivoObrigatorio);
        }

        private void ValidarCampoMarcaObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.MarcaId == Guid.Empty || dto.MarcaId == null, ProdutoResourcer.MarcaObrigatorio);
        }

        private void ValidarCampoCategoriaObrigatorio(ProdutoRequestDto dto)
        {
            Mensagens.AdicionarErroSe(dto.CategoriaId == Guid.Empty || dto.CategoriaId == null, ProdutoResourcer.MarcaObrigatorio);
        }

        private async Task ValidarSeCodigoUnicoExiste(ProdutoRequestDto dto)
        {

            if (await _produtoRepository.ValidarExistenciaAsync(p => p.CodigoUnico == dto.CodigoUnico))
            {
                Mensagens.AdicionarErro(string.Format(ProdutoResourcer.CodigoUnicoJaCadastrado, dto.CodigoUnico));
            }
        }

        private async Task ValidarSeCategoriaExiste(ProdutoRequestDto dto)
        {
            if (!await _marcaRepository.ValidarExistenciaAsync(m => m.Id == dto.MarcaId))
            {
                Mensagens.AdicionarErro(ProdutoResourcer.MarcaInvalida);
            }
        }

        private async Task ValidarSeFornecedorExiste(ProdutoRequestDto dto)
        {
            if (!await _fornecedorRepository.ValidarExistenciaAsync(f => f.Id == dto.FornecedorId))
            {
                Mensagens.AdicionarErro(ProdutoResourcer.FornecedorInvalido);
            }
        }
    }
}
