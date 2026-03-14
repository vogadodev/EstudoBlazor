

using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.RetornoPadraoAPIs
{
    public class RetornoPadraoService : MensagemService
    {
        public object? Data { get; set; } = null;
    }
}
