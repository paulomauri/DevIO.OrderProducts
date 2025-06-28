using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Application.Commands.Produto;

public class DeleteProdutoCommand : IRequest
{
    public Guid Id { get; set; }
}
