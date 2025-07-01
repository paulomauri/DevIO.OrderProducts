using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class CreateProdutoHandler : IRequestHandler<CreateProdutoCommand, Guid>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private const string CacheKey = "produto";

    public CreateProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Guid> Handle(CreateProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = new Domain.Entities.Produto(request.Nome, request.Descricao ?? "", request.Preco, request.Estoque);

        await _produtoRepository.AdicionarAsync(produto);
        await _unitOfWork.CommitAsync(cancellationToken);
        await _cache.RemoveAsync(CacheKey);
        return produto.Id;
    }
}
