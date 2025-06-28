using DevIO.OrderProducts.Application.DTO;
using MediatR;

namespace DevIO.OrderProducts.Application.Commands.Pedido;

public class CreatePedidoCommand : IRequest<Guid>
{
    public List<ItemPedidoDto> Itens { get; set; } = new();
}
