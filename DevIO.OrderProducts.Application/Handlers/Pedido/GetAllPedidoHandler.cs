using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Queries.Pedido;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;


namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class GetAllPedidoHandler : IRequestHandler<GetAllPedidoQuery, IEnumerable<PedidoDto>>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IRedisCacheService _cache;
    private readonly ICachePolicyService _policy;
    private const string CacheKey = "pedido";

    public GetAllPedidoHandler(IPedidoRepository pedidoRepository, IRedisCacheService cache, ICachePolicyService policy)
    {
        _pedidoRepository = pedidoRepository;
        _cache = cache;
        _policy = policy;
    }

    public async Task<IEnumerable<PedidoDto>> Handle(GetAllPedidoQuery request, CancellationToken cancellationToken)
    {
        var cached = await _cache.GetAsync<IEnumerable<PedidoDto>>(CacheKey);
        if (cached != null)
            return cached;

        var pedidos = await _pedidoRepository.ObterTodosAsync();

        var dtos = pedidos.Select(p => new PedidoDto
        {
            Id = p.Id,
            DataCriacao = p.DataCriacao,
            Status = (int)p.Status,
            Itens = p.Itens.Select(i => new ItemPedidoDto
            {
                ProdutoId = i.ProdutoId,
                Observacao = i.Observacao,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        });

        var ttl = _policy.GetExpirationFor(CacheKey);

        await _cache.SetAsync(CacheKey, dtos, ttl);

        return dtos;
    }
}
