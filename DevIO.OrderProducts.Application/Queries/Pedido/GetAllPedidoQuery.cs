using DevIO.OrderProducts.Application.DTO;
using MediatR;

namespace DevIO.OrderProducts.Application.Queries.Pedido;

public record GetAllPedidoQuery() : IRequest<IEnumerable<PedidoDto>>;
