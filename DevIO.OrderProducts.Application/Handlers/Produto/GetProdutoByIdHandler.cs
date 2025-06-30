using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Queries.Produto;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class GetProdutoByIdHandler : IRequestHandler<GetProdutoByIdQuery, ProdutoDto?>
{
    private readonly IProdutoRepository _produtoRepository;
    public GetProdutoByIdHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }
    public async Task<ProdutoDto?> Handle(GetProdutoByIdQuery request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto == null) return null;
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
