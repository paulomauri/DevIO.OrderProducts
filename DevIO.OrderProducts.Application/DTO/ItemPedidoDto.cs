using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.DTO;

public record ItemPedidoDto
{
    public Guid Id { get; set; }
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public string? Observacao { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
