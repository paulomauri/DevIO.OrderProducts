using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Application.Events.Produto;
using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Application.Interfaces.Messaging;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class UpdateProdutoHandler
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cache;
    private readonly IKafkaProducerService _kafka;
    private readonly ILogger<UpdateProdutoHandler> _logger;
    private const string CacheKey = "produto";

    public UpdateProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IRedisCacheService cache, IKafkaProducerService kafka, ILogger<UpdateProdutoHandler> logger)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _kafka = kafka;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);

        if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");

        produto.Atualizar(request.Nome, request.Descricao ?? "", request.Preco);

        // Atualiza dados do produto no repositório
        await _produtoRepository.AtualizarAsync(produto);

        // Commit na unidade de trabalho
        await _unitOfWork.CommitAsync(cancellationToken);

        // Loga a informação
        _logger.LogInformation("Produto {ProdutoId} atualizado com sucesso.", produto.Id);

        // Limpa o cache para garantir que os dados estejam atualizados
        await _cache.RemoveAsync(CacheKey);

        // Publicar evento de atualização
        await _kafka.ProduceAsync("produto-atualizado", new ProdutoAtualizadoEvent
        {
            ProdutoId = produto.Id,
            Nome = produto.Nome,
            Preco = produto.Preco,
            DataAtualizacao = DateTime.UtcNow
        });

        return Unit.Value;
    }

}
