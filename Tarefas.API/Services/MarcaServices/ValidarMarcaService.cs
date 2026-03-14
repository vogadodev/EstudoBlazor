using AVANADE.MODULOS.Modulos.AVANADE_ESTOQUE.Resourcers;
using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.ServicoComMensagemService;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.MarcaServices
{
    public class ValidarMarcaService : MensagemService
    {
        private readonly MarcaRepository<DbContextTarefas> _marcaRepository;

        public ValidarMarcaService(MarcaRepository<DbContextTarefas> marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task<bool> Validar(MarcaRequestDto dto , bool ehAtualizacao)
        {
            ValidarCampoNomeObrigatorio(dto);
            
            if (!Mensagens.TemErros() && !ehAtualizacao)
            {
                await ValidarSeNomeJaExiste(dto);
            }

            return Mensagens.TemErros();
        }

        private void ValidarCampoNomeObrigatorio(MarcaRequestDto dto)
        {
            Mensagens.AdicionarErroSe(string.IsNullOrWhiteSpace(dto.Nome), MarcaResourcer.NomeObrigatorio);
        }

        private async Task ValidarSeNomeJaExiste(MarcaRequestDto dto)
        {
            if (await _marcaRepository.ValidarExistenciaAsync(m => m.Nome == dto.Nome && m.Id != dto.Id))
            {
                Mensagens.AdicionarErro(string.Format(MarcaResourcer.NomeJaCadastrado, dto.Nome));
            }
        }
    }
}
