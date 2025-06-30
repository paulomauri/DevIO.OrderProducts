using DevIO.OrderProducts.Application.DTO;
using MediatR;

namespace DevIO.OrderProducts.Application.Queries.Pedido;

public record GetPedidoByIdQuery(Guid Id) : IRequest<PedidoDto?>;