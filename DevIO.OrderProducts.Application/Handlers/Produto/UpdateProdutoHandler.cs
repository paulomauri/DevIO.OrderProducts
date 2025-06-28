using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;

public class UpdateProdutoHandler
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);

        if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");

        produto.Atualizar(request.Nome, request.Descricao ?? "", request.Preco);
        await _produtoRepository.AtualizarAsync(produto);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }

}
