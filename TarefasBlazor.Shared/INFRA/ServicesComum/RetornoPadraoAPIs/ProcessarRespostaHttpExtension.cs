using Microsoft.AspNetCore.Mvc;
using TarefasBlazor.Shared.INFRA.ServicesComum.ServicoComMensagemService;
using TarefasBlazor.Shared.MODULOS.COMUM.Interfaces;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs
{
    public static class ProcessarRespostaHttpExtension
    {
        public static IActionResult ResponderRequest(this RetornoPadraoService servico, ControllerBase controller, string? linkDoResource = null)
        {
            var retorno = new { servico.Data, servico.Mensagens };
            if (servico.Mensagens.TemErros())
            {
                return controller.BadRequest(retorno);
            }

            var servicoComBuscaPadrao = servico as IServicoComBuscaPadrao;
            if (servicoComBuscaPadrao != null && !servicoComBuscaPadrao.Encontrado)
            {
                return controller.NoContent();
            }           
                       
            var servicoDeGravacaoPadrao = servico as IServicoDeGravacaoPadrao;
            if (servicoDeGravacaoPadrao != null)
            {
                //Se foi criado um novo registro (padrão do post, mas possível no put) e se foi passado o link do resource
                if (servicoDeGravacaoPadrao.IdentificadorDoRegistroCriado != null && !string.IsNullOrWhiteSpace(linkDoResource))
                {
                    //retorna 201 com o link - inserido com sucesso + link para recurso criado
                    return controller.Created(linkDoResource + "/" + servicoDeGravacaoPadrao.IdentificadorDoRegistroCriado, retorno);
                }              
                if (retorno == null && (retorno?.Mensagens == null || !retorno.Mensagens.Any()))
                {
                    //retorna 201 - Criado com sucesso
                    return controller.Created();
                }
            }
            var servicoDeAtualizacaoPadrao = servico as IServicoDeAtualizacaoPadrao;
            if (servicoDeAtualizacaoPadrao != null && servicoDeAtualizacaoPadrao.Atualizado)
            {
                return controller.NoContent();
            }

            var servicoDeExclusaoPadrao = servico as IServicoDeExclusaoPadrao;
            if (servicoDeExclusaoPadrao != null)
            {
                // padrão do delete - sem conteúdo
                if (retorno == null && (retorno?.Mensagens == null || !retorno.Mensagens.Any()))
                {
                    //retorna 204 - excluído com sucesso
                    return controller.NoContent();
                }
            }

            //genérico - retorna ok com o conteúdo
            return controller.Ok(retorno);
        }
    }
}
