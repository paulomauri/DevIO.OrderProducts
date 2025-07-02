using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Events.Pedido
{
    public class PedidoCriadoEvent
    {
        public Guid PedidoId { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
    }
}
