using Confluent.Kafka;
using DevIO.OrderProducts.Application.Events.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Worker.Consumers;

public class PedidoCriadoConsumerService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PedidoCriadoConsumerService> _logger;

    public PedidoCriadoConsumerService(IConfiguration configuration, ILogger<PedidoCriadoConsumerService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = _configuration["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_configuration["Kafka:Topic"]);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                _logger.LogInformation("Evento recebido: {Message}", result.Message.Value);

                // Você pode desserializar o JSON aqui:
                var pedido = JsonSerializer.Deserialize<PedidoCriadoEvent>(result.Message.Value);
                _logger.LogInformation("Pedido ID: {Id} - Total: {Total}", pedido?.PedidoId, pedido?.Valor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consumir mensagem Kafka");
            }

            await Task.Delay(500, stoppingToken);
        }
    }
}
