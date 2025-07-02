using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Events.Produto
{
    public class ProdutoAtualizadoEvent
    {
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
