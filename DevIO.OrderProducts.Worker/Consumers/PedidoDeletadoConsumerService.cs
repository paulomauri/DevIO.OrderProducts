using Confluent.Kafka;
using DevIO.OrderProducts.Application.Events.Pedido;
using DevIO.OrderProducts.Application.Events.Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Worker.Consumers;

public class PedidoDeletadoConsumerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<PedidoDeletadoConsumerService> _logger;

    public PedidoDeletadoConsumerService(IConfiguration config, ILogger<PedidoDeletadoConsumerService> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var topic = _config["Kafka:PedidoDeletado:Topic"];
            var groupId = _config["Kafka:PedidoDeletado:GroupId"];
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
                    var pedido = JsonSerializer.Deserialize<PedidoDeletadoEvent>(result.Message.Value);
                    _logger.LogInformation("Produto Deletado: {Id} - {DataExclusao}", pedido?.PedidoId , pedido?.DataExclusao );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem do tópico pedido-deletado");
                }
            }
        }, stoppingToken);
    }
}
