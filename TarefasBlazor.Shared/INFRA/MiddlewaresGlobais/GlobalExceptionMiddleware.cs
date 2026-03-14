using Microsoft.AspNetCore.Http;
using TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;
using TarefasBlazor.Shared.MODULOS.COMUM.Enums;

namespace TarefasBlazor.Shared.INFRA.MiddlewaresGlobais
{
    public class GlobalExceptionMiddleware : RetornoPadraoService
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //TODO- EM PRODUÇÃO RETORNA MENSAGEM GENÉRICA SEM EXPOR DETALHES DA EXCEÇÃO
                Mensagens.Add(new Mensagem(ex.Message, EnumTipoMensagem.Erro));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(this);
                Mensagens.Clear();
            }
        }
    }
}