using DevIO.OrderProducts.Application.Events.Pedido;
using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Application.Events;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Interfaces.Messaging;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;


namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class CreatePedidoHandler : IRequestHandler<CreatePedidoCommand, Guid>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private readonly IKafkaProducerService _kafkaProducerService;
    private const string CacheKey = "pedido";

    public CreatePedidoHandler(IPedidoRepository pedidoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache, IKafkaProducerService kafkaProducerService)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task<Guid> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = new Domain.Entities.Pedido();
        request.Itens.ForEach(item => 
        {
            var itemPedido = new Domain.Entities.ItemPedido(item.ProdutoId, item.Observacao ?? "", item.Quantidade, item.PrecoUnitario);
            pedido.AdicionarItem(itemPedido);
        });

        await _pedidoRepository.AdicionarAsync(pedido);

        await _unitOfWork.CommitAsync(cancellationToken);

        await _cache.RemoveAsync(CacheKey);

        await _kafkaProducerService.ProduceAsync("pedido-criado", new PedidoCriadoEvent()
        {
            PedidoId = pedido.Id,
            Data = pedido.DataCriacao,
            Valor = pedido.CalcularTotal(),
        });

        return pedido.Id;
    }
}
