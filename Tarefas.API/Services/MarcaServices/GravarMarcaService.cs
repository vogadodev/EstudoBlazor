using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.ServicesComum.GeradorDeIDsService;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.DTOs.Request;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Entidades;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;

namespace Tarefas.API.Services.MarcaServices
{
    public class GravarMarcaService : RetornoPadraoService
    {
        private readonly MarcaRepository<DbContextTarefas> _marcaRepository;
        private readonly ValidarMarcaService _validarMarcaService;
        public GravarMarcaService(MarcaRepository<DbContextTarefas> marcaRepository, ValidarMarcaService validarMarcaService)
        {
            _marcaRepository = marcaRepository;
            _validarMarcaService = validarMarcaService;
        }

        public async Task GravarMarca(MarcaRequestDto dto)
        {
            var marcarExistente = await _marcaRepository.SelecionarObjetoAsync(m => m.Id == dto.Id);
            var ehAtualizacao = marcarExistente != null;

            var dtoTemErros = await _validarMarcaService.Validar(dto, ehAtualizacao);
            if (dtoTemErros)
            {
                Mensagens.AddRange(_validarMarcaService.Mensagens);
                return;
            }

           
            if (ehAtualizacao)
            {
                PreencherMarca(marcarExistente!, dto);
                _marcaRepository.DbSet.Update(marcarExistente!);
            }
            else
            {
                var novaMarca = new Marca() { Id = CriarIDService.CriarNovoID() };
                PreencherMarca(novaMarca , dto);
            }
        }
        private void PreencherMarca(Marca marca, MarcaRequestDto dto)
        {
            marca.Nome = dto.Nome;
        }
    }
}
