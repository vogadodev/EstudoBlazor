using System.ComponentModel;

namespace TarefasBlazor.Shared.MODULOS.COMUM.Enums
{
    public enum EnumEndpointPrincipalGateway
    {
        [Description("http://avanade-gateway-api:8080")]
        EndPointGatewayDockerCompose,
        [Description("https://localhost:7046/")]
        EndpointLocalHost
    }
}
