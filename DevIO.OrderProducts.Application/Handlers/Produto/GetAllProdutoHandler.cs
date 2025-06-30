using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Queries.Produto;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

public class GetAllProdutoHandler : IRequestHandler<GetAllProdutoQuery, IEnumerable<ProdutoDto>>
{
    private readonly IProdutoRepository _produtoRepository;

    public GetAllProdutoHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task <IEnumerable<ProdutoDto>> Handle(GetAllProdutoQuery request, CancellationToken cancellationToken)
    {
        var produtos = await _produtoRepository.ObterTodosAsync();

        return produtos.Select(p => new ProdutoDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            Estoque = p.Estoque
        });
    }
}
