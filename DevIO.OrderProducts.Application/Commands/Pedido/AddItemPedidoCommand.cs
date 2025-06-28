using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Commands.Pedido;

public class AddItemPedidoCommand : IRequest
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public string Observacao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
