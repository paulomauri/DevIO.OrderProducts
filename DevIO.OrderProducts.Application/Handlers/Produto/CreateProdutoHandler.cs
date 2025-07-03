using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class CreateProdutoHandler : IRequestHandler<CreateProdutoCommand, Guid>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private const string CacheKey = "produto";
    private readonly ILogger<CreateProdutoHandler> _logger;

    public CreateProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache, ILogger<CreateProdutoHandler> logger)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = new Domain.Entities.Produto(request.Nome, request.Descricao ?? "", request.Preco, request.Estoque);

        // Grava no repositório
        await _produtoRepository.AdicionarAsync(produto);
        // az o commit na unidade de trabalho
        await _unitOfWork.CommitAsync(cancellationToken);
        // Loga a informação
        _logger.LogInformation("Produto {ProdutoId} criado com sucesso.", produto.Id);
        // Limpa o cache para garantir que os dados estejam atualizados
        await _cache.RemoveAsync(CacheKey);
        
        return produto.Id;
    }
}
