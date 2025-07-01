using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class DeleteProdutoHandler 
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private const string CacheKey = "produto";

    public DeleteProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Unit> Handle(DeleteProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");
        await _produtoRepository.RemoverAsync(produto);
        await _unitOfWork.CommitAsync(cancellationToken); 
        await _cache.RemoveAsync(CacheKey);

        return Unit.Value;
    }
}
