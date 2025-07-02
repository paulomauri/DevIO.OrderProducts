using Confluent.Kafka;
using DevIO.OrderProducts.Application.Events.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Worker.Consumers;

public class PedidoAtualizadoConsumerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<PedidoAtualizadoConsumerService> _logger;

    public PedidoAtualizadoConsumerService(IConfiguration config, ILogger<PedidoAtualizadoConsumerService> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var topic = _config["Kafka:PedidoAtualizado:Topic"];
            var groupId = _config["Kafka:PedidoAtualizado:GroupId"];
            var bootstrap = _config["Kafka:BootstrapServers"];

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrap,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    var pedido = JsonSerializer.Deserialize<PedidoAtualizadoEvent>(result.Message.Value);
                    _logger.LogInformation("Pedido Atualizado: {Id} - {Total}", pedido?.PedidoId, pedido?.Total);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem do tópico pedido-atualizado");
                }
            }
        }, stoppingToken);
    }
}
