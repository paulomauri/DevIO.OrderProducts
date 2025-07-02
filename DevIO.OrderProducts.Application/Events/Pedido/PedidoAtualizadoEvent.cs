using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Events.Pedido
{
    public class PedidoAtualizadoEvent
    {
        public Guid PedidoId { get; set; }
        public int Status { get; set; }
        public decimal Total { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
