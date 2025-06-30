using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Queries.Pedido;
using DevIO.OrderProducts.Domain.Interfaces;
using MediatR;


namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class GetAllPedidoHandler : IRequestHandler<GetAllPedidoQuery, IEnumerable<PedidoDto>>
{
    private readonly IPedidoRepository _pedidoRepository;
    public GetAllPedidoHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<IEnumerable<PedidoDto>> Handle(GetAllPedidoQuery request, CancellationToken cancellationToken)
    {
        var pedidos = await _pedidoRepository.ObterTodosAsync();
        return pedidos.Select(p => new PedidoDto
        {
            Id = p.Id,
            DataCriacao = p.DataCriacao,
            Status = (int)p.Status,
            Itens = p.Itens.Select(i => new ItemPedidoDto
            {
                ProdutoId = i.ProdutoId,
                Observacao = i.Observacao,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        });
    }
}
