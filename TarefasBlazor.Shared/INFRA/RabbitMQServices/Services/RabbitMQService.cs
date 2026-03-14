using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TarefasBlazor.Shared.INFRA.RabbitMQServices.Interfaces;
using TarefasBlazor.Shared.MODULOS.COMUM.Entidades;


namespace TarefasBlazor.Shared.INFRA.RabbitMQServices.Services
{

    public class RabbitMQService : IMessageBusService, IAsyncDisposable
    {
        private const string EXCHANGE_NAME = "avanade.direct.exchange";

        private readonly ILogger<RabbitMQService> _logger;
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _consumerChannel;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _consumerChannelLock = new SemaphoreSlim(1, 1);

        public RabbitMQService(ILogger<RabbitMQService> logger, IOptions<RabbitMqSettings> settings)
        {
            _logger = logger;
            var rabbitMqSettings = settings.Value;

            // Configuração da Factory
            _factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.Host,
                Port = rabbitMqSettings.Port,
                UserName = rabbitMqSettings.Username,
                Password = rabbitMqSettings.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30),
                DispatchConsumersAsync = true
            };
        }

        // --- MÉTODO DE POSTAR (Publicar) ---
        public async Task PublicarMensagemAsync<T>(T message, string queueName) where T : class
        {          
            await EnsureConnectionAsync();
          
            using var channel = _connection.CreateModel();

            var body = JsonSerializer.SerializeToUtf8Bytes(message);          
            channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Direct, durable: true);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: EXCHANGE_NAME, routingKey: queueName);

            channel.BasicPublish(
                exchange: EXCHANGE_NAME,
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            _logger.LogInformation("Mensagem publicada na fila {QueueName}", queueName);
        }

        // --- MÉTODO DE CONSUMIR (Registrar Handler) ---
        public async Task ConsumirMensagensAsync<T>(string queueName, Func<T, Task> handler) where T : class        {
         
            await EnsureConsumerChannelAsync();

            _consumerChannel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _consumerChannel.QueueBind(queue: queueName, exchange: EXCHANGE_NAME, routingKey: queueName);
            _consumerChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
          
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                T message = null;

                try
                {
                    message = JsonSerializer.Deserialize<T>(json);
                    if (message != null){
                       
                        await handler(message);
                        
                        _consumerChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        _logger.LogError("Consumindo Fila {QueueName}. Mensagem: {Json}", queueName, json);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem da fila {QueueName}. Mensagem: {Json}", queueName, json);
                 
                    _consumerChannel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };
           
            _consumerChannel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }
      
        private async Task EnsureConnectionAsync()
        {
            if (_connection != null && _connection.IsOpen)
                return;

            await _connectionLock.WaitAsync();
            try
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _connection = _factory.CreateConnection();
                    _logger.LogInformation("Conexão RabbitMQ estabelecida.");
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private async Task EnsureConsumerChannelAsync()
        {
            await EnsureConnectionAsync();

            if (_consumerChannel != null && _consumerChannel.IsOpen)
                return;

            await _consumerChannelLock.WaitAsync();
            try
            {
                if (_consumerChannel == null || !_consumerChannel.IsOpen)
                {
                    _consumerChannel = _connection.CreateModel();
                    _consumerChannel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Direct, durable: true);
                    _logger.LogInformation("Canal de consumidor RabbitMQ estabelecido.");
                }
            }
            finally
            {
                _consumerChannelLock.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Close();
                _consumerChannel.Dispose();
            }
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
            _connectionLock.Dispose();
            _consumerChannelLock.Dispose();
            await ValueTask.CompletedTask;
        }
    }
}