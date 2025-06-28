using MediatR;

namespace DevIO.OrderProducts.Application.Commands.Produto;

public class CreateProdutoCommand : IRequest<Guid>
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}