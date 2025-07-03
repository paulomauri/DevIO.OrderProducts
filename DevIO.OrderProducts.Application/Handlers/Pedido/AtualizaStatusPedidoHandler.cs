using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Application.Events.Pedido;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Interfaces.Messaging;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class AtualizaStatusPedidoHandler : IRequestHandler<AtualizarStatusPedidoCommand>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private readonly IKafkaProducerService _kafka;
    private readonly ILogger<AtualizaStatusPedidoHandler> _logger;
    private const string CacheKey = "pedido";
    public AtualizaStatusPedidoHandler(IPedidoRepository pedidoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache, IKafkaProducerService kafka, ILogger<AtualizaStatusPedidoHandler> logger)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _kafka = kafka;
        _logger = logger;
    }

    public async Task Handle(AtualizarStatusPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(request.PedidoId) 
            ?? throw new Exception("Pedido não encontrado."); 

        pedido.AlterarStatus((Domain.Enums.StatusPedido) request.Status);

        // Atualiza o status do pedido no repositório           
        await _pedidoRepository.AtualizarAsync(pedido);
        
        // Commit na unidade de trabalho
        await _unitOfWork.CommitAsync(cancellationToken);

        // Loga a informação
        _logger.LogInformation("Pedido {PedidoId} atualizado com sucesso para o status {Status}.", pedido.Id, pedido.Status);

        // Limpa o cache para garantir que os dados estejam atualizados
        await _cache.RemoveAsync(CacheKey); // Remove o cache para garantir que os dados estejam atualizados

        // Publicar evento de atualização
        await _kafka.ProduceAsync("pedido-atualizado", new PedidoAtualizadoEvent
        {
            PedidoId = pedido.Id,
            Total = pedido.CalcularTotal(),
            DataAtualizacao = DateTime.UtcNow
        });
    }
}
