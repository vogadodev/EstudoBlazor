namespace TarefasBlazor.Shared.INFRA.RabbitMQServices.Interfaces
{
    public interface IMessageBusService
    {
        // Método para publicar uma mensagem 
        Task PublicarMensagemAsync<T>(T message, string queueName) where T : class;

        // Método para registrar um consumidor 
        Task ConsumirMensagensAsync<T>(string queueName, Func<T, Task> handler) where T : class;
    }
}