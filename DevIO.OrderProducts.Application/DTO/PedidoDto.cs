
namespace DevIO.OrderProducts.Application.DTO;

public record PedidoDto
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public int Status { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = new();
}
