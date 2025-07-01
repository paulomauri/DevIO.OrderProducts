
using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class AddItemPedidoHandler : IRequestHandler<AddItemPedidoCommand>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private const string CacheKey = "pedido";

    public AddItemPedidoHandler(IPedidoRepository pedidoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Handle(AddItemPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido =  await _pedidoRepository.ObterPorIdAsync(request.PedidoId) 
            ?? throw new Exception("Pedido não encontrado.");

        var itemPedido = new Domain.Entities.ItemPedido(request.ProdutoId, request.Observacao ?? "", request.Quantidade, request.PrecoUnitario);
        pedido.AdicionarItem(itemPedido);

        await _pedidoRepository.AtualizarAsync(pedido);
        await _unitOfWork.CommitAsync(cancellationToken);
        await _cache.RemoveAsync($"{CacheKey}_{pedido.Id}"); // Remove o cache para garantir que os dados estejam atualizados
    }
}
