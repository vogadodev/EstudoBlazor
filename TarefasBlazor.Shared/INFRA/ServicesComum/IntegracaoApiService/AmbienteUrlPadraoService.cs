

using TarefasBlazor.Shared.INFRA.ServicesComum.EnumService;
using TarefasBlazor.Shared.MODULOS.COMUM.Enums;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.IntegracaoApiService
{
    public static class AmbienteUrlPadraoService
    {
        public static string UrlPadraoService()
        {
            //Para testes com dockercompose utilize essa linha
            return EnumEndpointPrincipalGateway.EndPointGatewayDockerCompose.GetDescription();
            
            //Para testes locais sem dockercompose utilize essa linha            
            //return EnumEndpointPrincipalGateway.EndpointLocalHost.GetDescription();
        }
    }
}
