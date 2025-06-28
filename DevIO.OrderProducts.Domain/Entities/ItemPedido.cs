namespace DevIO.OrderProducts.Domain.Entities;

public class ItemPedido
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string Observacao { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    protected ItemPedido() { }

    public ItemPedido(Guid produtoId, string observacao, int quantidade, decimal precoUnitario)
    {
        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        Observacao = observacao;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public decimal CalcularTotal() => Quantidade * PrecoUnitario;
}
