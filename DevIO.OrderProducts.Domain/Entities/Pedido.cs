using DevIO.OrderProducts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Domain.Entities;

public class Pedido
{
    public Guid Id { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public StatusPedido Status { get; private set; }
    public List<ItemPedido> Itens { get; private set; } = new();

    public Pedido()
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.UtcNow;
        Status = StatusPedido.Novo;
    }

    public void AdicionarItem(ItemPedido item)
    {
        Itens.Add(item);
    }

    public decimal CalcularTotal() => Itens.Sum(i => i.CalcularTotal());

    public void AlterarStatus(StatusPedido status)
    {
        Status = status;
    }
}
