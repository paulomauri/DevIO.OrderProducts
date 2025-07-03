
using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class AddItemPedidoHandler : IRequestHandler<AddItemPedidoCommand>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private readonly ILogger<AddItemPedidoHandler> _logger;
    private const string CacheKey = "pedido";

    public AddItemPedidoHandler(IPedidoRepository pedidoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache, ILogger<AddItemPedidoHandler> logger)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task Handle(AddItemPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido =  await _pedidoRepository.ObterPorIdAsync(request.PedidoId) 
            ?? throw new Exception("Pedido não encontrado.");

        var itemPedido = new Domain.Entities.ItemPedido(request.ProdutoId, request.Observacao ?? "", request.Quantidade, request.PrecoUnitario);
        pedido.AdicionarItem(itemPedido);

        // Atualiza o pedido no repositório
        await _pedidoRepository.AtualizarAsync(pedido);
        // Commit na unidade de trabalho
        await _unitOfWork.CommitAsync(cancellationToken);
        // Loga a informação
        _logger.LogInformation("Item adicionado ao pedido {PedidoId} com sucesso.", pedido.Id);
        // Limpa o cache para garantir que os dados estejam atualizados
        await _cache.RemoveAsync($"{CacheKey}_{pedido.Id}"); 
    }
}
