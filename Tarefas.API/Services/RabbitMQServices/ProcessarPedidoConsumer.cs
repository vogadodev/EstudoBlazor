using Tarefas.API.Data;
using TarefasBlazor.Shared.INFRA.RabbitMQServices.Interfaces;
using TarefasBlazor.Shared.INFRA.RabbitMQServices.Queues;
using TarefasBlazor.Shared.MODULOS.ESTOQUE.Repositories;
using TarefasBlazor.Shared.MODULOS.VENDA.DTOs.ContratosMensagem;

namespace Tarefas.API.Services.RabbitMQServices
{
    public class ProcessarPedidoConsumer : BackgroundService
    {
        private readonly ILogger<ProcessarPedidoConsumer> _logger;
        private readonly IMessageBusService _messageBus;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessarPedidoConsumer(
            ILogger<ProcessarPedidoConsumer> logger,
            IMessageBusService messageBus,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _messageBus = messageBus;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consumidor de Novos Pedidos iniciando.");

            await _messageBus.ConsumirMensagensAsync<PedidoDto>(
                RabbitMqQueues.PedidosNovos,
                async (pedido) => await ProcessarPedidoAsync(pedido)
            );
        }

        private async Task ProcessarPedidoAsync(PedidoDto pedido)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var produtoRepository = scope.ServiceProvider.GetRequiredService<ProdutoRepository<DbContextTarefas>>();

                _logger.LogInformation("Processando Pedido {PedidoId}", pedido.IdPedido);

                var falhas = new List<ItemInvalidoDto>();

                foreach (var item in pedido.Itens)
                {
                    var produtoDb = await produtoRepository.SelecionarObjetoAsync(p => p.Id == item.IdProduto);
                    if (produtoDb == null)
                    {
                        falhas.Add(new ItemInvalidoDto(item.IdProduto, item.Quantidade, 0, "Produto Inexistente"));
                    }
                    else if (produtoDb.QuantidadeEstoque < item.Quantidade)
                    {
                        falhas.Add(new ItemInvalidoDto(item.IdProduto, item.Quantidade, produtoDb.QuantidadeEstoque, "Estoque Insuficiente"));
                    }
                }

                if (falhas.Any())
                {
                    _logger.LogWarning("Pedido {PedidoId} falhou. Itens indisponíveis: {Count}", pedido.IdPedido, falhas.Count);
                    var msgFalha = new PedidoNaoConcluidoDto(pedido, falhas);
                    await _messageBus.PublicarMensagemAsync(msgFalha, RabbitMqQueues.PedidosNaoConcluidos);
                }
                else
                {
                    try
                    {
                        await AtualizarEstoqueLocalmente(pedido.Itens, produtoRepository);
                        await produtoRepository.DbContext.SaveChangesAsync();

                        _logger.LogInformation("Pedido {PedidoId} concluído com sucesso. Publicando.", pedido.IdPedido);
                        await _messageBus.PublicarMensagemAsync(pedido, RabbitMqQueues.PedidosConcluidos);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao dar baixa no estoque para Pedido {PedidoId}. Revertendo.", pedido.IdPedido);
                        var msgErro = new PedidoNaoConcluidoDto(pedido, falhas.Any() ? falhas : new List<ItemInvalidoDto> { new(Guid.Empty, 0, 0, $"Erro interno: {ex.Message}") });
                        await _messageBus.PublicarMensagemAsync(msgErro, RabbitMqQueues.PedidosNaoConcluidos);
                    }
                }
            }
        }
        private async Task AtualizarEstoqueLocalmente(List<ItemPedidoDto> listaPedidosDto, ProdutoRepository<DbContextTarefas> produtoRepository)
        {
            var listaIdsProdutos = listaPedidosDto.Select(p => p.IdProduto).ToList();
            var listaProdutosBanco = await produtoRepository.SelecionarListaObjetoAsync(p => listaIdsProdutos.Contains(p.Id));

            foreach (var itemAtualizar in listaProdutosBanco)
            {
                itemAtualizar.QuantidadeEstoque -= listaPedidosDto.Where(p => p.IdProduto == itemAtualizar.Id).First().Quantidade;
                produtoRepository.DbSet.Update(itemAtualizar);
            }
        }
    }
}