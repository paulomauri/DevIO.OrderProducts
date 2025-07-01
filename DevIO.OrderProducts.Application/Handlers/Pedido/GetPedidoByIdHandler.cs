
using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Queries.Pedido;
using DevIO.OrderProducts.Domain.Interfaces;

namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class GetPedidoByIdHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IRedisCacheService _cache;
    private readonly ICachePolicyService _policy;   
    private const string CacheKey = "pedido";

    public GetPedidoByIdHandler(IPedidoRepository pedidoRepository, IRedisCacheService cache, ICachePolicyService policy)
    {
        _pedidoRepository = pedidoRepository;
        _cache = cache;
        _policy = policy;
    }

    public async Task<PedidoDto?> Handle(GetPedidoByIdQuery request, CancellationToken cancellationToken)
    {
        var cached = await _cache.GetAsync<PedidoDto>($"{CacheKey}_{request.Id}");
        if (cached != null)
            return cached;

        var pedido = await _pedidoRepository.ObterPorIdAsync(request.Id);

        if (pedido == null) return null;

        var ttl = _policy.GetExpirationFor(CacheKey);

        await _cache.SetAsync($"{CacheKey}_{request.Id}", 
            new PedidoDto
            {
                Id = pedido.Id,
                DataCriacao = pedido.DataCriacao,
                Status = (int)pedido.Status,
                Itens = pedido.Itens.Select(i => new ItemPedidoDto
                {
                    ProdutoId = i.ProdutoId,
                    Observacao = i.Observacao,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            },
            ttl);

        return new PedidoDto
        {
            Id = pedido.Id,
            DataCriacao = pedido.DataCriacao,
            Status = (int)pedido.Status,
            Itens = pedido.Itens.Select(i => new ItemPedidoDto
            {
                ProdutoId = i.ProdutoId,
                Observacao = i.Observacao,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };
    }
}
