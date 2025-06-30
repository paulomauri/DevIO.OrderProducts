
using DevIO.OrderProducts.Application.DTO;
using DevIO.OrderProducts.Application.Queries.Pedido;
using DevIO.OrderProducts.Domain.Interfaces;

namespace DevIO.OrderProducts.Application.Handlers.Pedido;

public class GetPedidoByIdHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    public GetPedidoByIdHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<PedidoDto?> Handle(GetPedidoByIdQuery request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(request.Id);
        if (pedido == null) return null;
        return new PedidoDto
        {
            Id = pedido.Id,
            DataCriacao = pedido.DataCriacao,
            Status = (int)pedido.Status,
            Itens = pedido.Itens.Select(i => new ItemPedidoDto
            {
                ProdutoId = i.ProdutoId,
                Observacao = i.Observacao,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };
    }
}
