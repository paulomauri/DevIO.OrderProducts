using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Events.Produto;

public class ProdutoDeletadoEvent
{
    public Guid ProdutoId { get; set; }
    public DateTime DataExclusao { get; set; }
}
