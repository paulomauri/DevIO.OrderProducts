using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Domain.Enums
{
    public enum StatusPedido
    {
        Novo = 0,
        Reservado = 1,
        Finalizado = 2,
        Cancelado = 3,
        Entregue = 4
    }
}
