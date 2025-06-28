using DevIO.OrderProducts.Application.Commands.Produto;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;

namespace DevIO.OrderProducts.Application.Handlers.Produto;


    public class DeleteProdutoHandler 
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProdutoHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork)
        {
            _produtoRepository = produtoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
            if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");
            await _produtoRepository.RemoverAsync(produto);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Unit.Value;
        }
    }
