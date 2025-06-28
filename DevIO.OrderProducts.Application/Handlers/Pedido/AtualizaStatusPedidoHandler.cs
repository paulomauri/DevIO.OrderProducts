using MediatR;
using DevIO.OrderProducts.Application.Commands.Pedido;
using DevIO.OrderProducts.Domain.Interfaces;

namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class AtualizaStatusPedidoHandler : IRequestHandler<AtualizarStatusPedidoCommand>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AtualizaStatusPedidoHandler(IPedidoRepository pedidoRepository, IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AtualizarStatusPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(request.PedidoId) 
            ?? throw new Exception("Pedido não encontrado."); 

        pedido.AlterarStatus((Domain.Enums.StatusPedido) request.Status );
        await _pedidoRepository.AtualizarAsync(pedido);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
