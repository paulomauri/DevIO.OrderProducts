using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Queries.Produto;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class GetProdutoByIdHandler : IRequestHandler<GetProdutoByIdQuery, ProdutoDto?>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IRedisCacheService _cache;
    private readonly ICachePolicyService _policy;
    private const string CacheKey = "produto";
    public GetProdutoByIdHandler(IProdutoRepository produtoRepository, IRedisCacheService cache, ICachePolicyService policy )
    {
        _produtoRepository = produtoRepository;
        _cache = cache;
        _policy = policy;
    }
    public async Task<ProdutoDto?> Handle(GetProdutoByIdQuery request, CancellationToken cancellationToken)
    {
        var cached = await _cache.GetAsync<ProdutoDto>($"{CacheKey}_{request.Id}");
        if (cached != null)
            return cached;

        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        // If the product is not found, return null
        if (produto == null) return null;

        var ttl = _policy.GetExpirationFor(CacheKey);

        // Cache the product details
        await _cache.SetAsync($"{CacheKey}_{request.Id}", 
            new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Estoque = produto.Estoque
            },
            ttl);

        return new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque
        };
    }
}
