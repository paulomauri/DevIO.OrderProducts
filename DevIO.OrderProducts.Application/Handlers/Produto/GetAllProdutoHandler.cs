using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Queries.Produto;
using DevIO.OrderProducts.Domain.Interfaces;

using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;
public class GetAllProdutoHandler : IRequestHandler<GetAllProdutoQuery, IEnumerable<ProdutoDto>>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IRedisCacheService _cache;
    private readonly ICachePolicyService _policy;
    private const string CacheKey = "produto";

    public GetAllProdutoHandler(IProdutoRepository produtoRepository, IRedisCacheService cache, ICachePolicyService policy)
    {
        _produtoRepository = produtoRepository;
        _cache = cache;
        _policy = policy;
    }

    public async Task <IEnumerable<ProdutoDto>> Handle(GetAllProdutoQuery request, CancellationToken cancellationToken)
    {
        var cached = await _cache.GetAsync<IEnumerable<ProdutoDto>>(CacheKey);
        if (cached != null)
            return cached;

        var produtos = await _produtoRepository.ObterTodosAsync();

        var dtos = produtos.Select(p => new ProdutoDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            Estoque = p.Estoque
        });

        var ttl = _policy.GetExpirationFor(CacheKey);

        await _cache.SetAsync(CacheKey, dtos, ttl);

        return dtos;
    }
}
