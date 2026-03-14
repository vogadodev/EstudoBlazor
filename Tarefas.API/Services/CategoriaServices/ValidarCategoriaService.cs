using Tarefas.API.Data;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;
using TarefasBlazor.Shared.INFRA.ServicesComum.ServicoComMensagemService;
using AVANADE.MODULOS.Modulos.AVANADE_ESTOQUE.Resourcers;

namespace Tarefas.API.Services.CategoriaServices
{
    public class ValidarCategoriaService : MensagemService
    {
        private readonly CategoriaRepository<DbContextTarefas> _categoriaRepository;

        public ValidarCategoriaService(CategoriaRepository<DbContextTarefas> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<bool> Validar(CategoriaRequestDto dto, bool ehAtualizacao)
        {
            ValidarCampoNomeObrigatorio(dto);
            ValidarCampoDescricao(dto);

            if (!Mensagens.TemErros() && !ehAtualizacao)
            {
                await ValidarSeNomeJaExiste(dto);
            }

            return Mensagens.TemErros();
        }

        private void ValidarCampoNomeObrigatorio(CategoriaRequestDto dto)
        {
            Mensagens.AdicionarErroSe(string.IsNullOrWhiteSpace(dto.Nome), CategoriaResourcer.NomeObrigatorio);
        }
      
        private void ValidarCampoDescricao(CategoriaRequestDto dto)
        {
            Mensagens.AdicionarErroSe(string.IsNullOrWhiteSpace(dto.Descricao), CategoriaResourcer.DescricaoObrigatorio);
        }

        private async Task ValidarSeNomeJaExiste(CategoriaRequestDto dto)
        {
            if (await _categoriaRepository.ValidarExistenciaAsync(c => c.Nome == dto.Nome && c.Id != dto.id))
            {
                Mensagens.AdicionarErro(string.Format(CategoriaResourcer.NomeJaCadastrado, dto.Nome));
            }
        }
    }
}
