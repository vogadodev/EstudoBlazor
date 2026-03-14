namespace TarefasBlazor.Shared.INFRA.RabbitMQServices.Queues
{
    public static class RabbitMqQueues
    {
        public const string PedidosNovos = "fila.pedidos.novos";
        public const string PedidosConcluidos = "fila.pedidos.concluidos";
        public const string PedidosNaoConcluidos = "fila.pedidos.nao_concluidos";
    }
}
