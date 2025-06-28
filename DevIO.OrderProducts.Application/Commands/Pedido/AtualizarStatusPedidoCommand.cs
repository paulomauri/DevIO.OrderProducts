
using MediatR;

namespace DevIO.OrderProducts.Application.Commands.Pedido;

public class AtualizarStatusPedidoCommand : IRequest 
{
    public Guid PedidoId { get; set; }
    public int Status { get; set; } // Representa o StatusPedido como um inteiro para simplificar a serialização/deserialização
}
