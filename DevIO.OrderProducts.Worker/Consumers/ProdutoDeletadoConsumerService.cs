using Confluent.Kafka;
using DevIO.OrderProducts.Application.Events.Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Worker.Consumers;

public class ProdutoDeletadoConsumerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<ProdutoDeletadoConsumerService> _logger;

    public ProdutoDeletadoConsumerService(IConfiguration config, ILogger<ProdutoDeletadoConsumerService> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var topic = _config["Kafka:ProdutoDeletado:Topic"];
            var groupId = _config["Kafka:ProdutoDeletado:GroupId"];
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
                    var produto = JsonSerializer.Deserialize<ProdutoDeletadoEvent>(result.Message.Value);
                    _logger.LogInformation("Produto Deletado: {Id} - {DataExclusao}", produto?.ProdutoId, produto?.DataExclusao);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem do tópico produto-deletado");
                }
            }
        }, stoppingToken);

    }
}
