using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Events.Produto;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Interfaces.Messaging;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class DeleteProdutoHandler 
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private readonly IKafkaProducerService _kafka;
    private readonly ILogger<DeleteProdutoHandler> _logger;
    private const string CacheKey = "produto";

    public DeleteProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache, IKafkaProducerService kafka, ILogger<DeleteProdutoHandler> logger)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _kafka = kafka;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");

        // Remover o produto do repositório
        await _produtoRepository.RemoverAsync(produto);

        // Commit na unidade de trabalho
        await _unitOfWork.CommitAsync(cancellationToken);

        // Loga a informação
        _logger.LogInformation("Produto {ProdutoId} deletado com sucesso.", produto.Id);

        // Limpa o cache para garantir que os dados estejam atualizados
        await _cache.RemoveAsync(CacheKey);

        // Publicar evento
        await _kafka.ProduceAsync("produto-deletado", new ProdutoDeletadoEvent
        {
            ProdutoId = produto.Id,
            DataExclusao = DateTime.UtcNow
        });

        return Unit.Value;
    }
}
