namespace DevIO.OrderProducts.Domain.Entities;
public class Produto
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public int Estoque { get; private set; }

    public Produto(string nome, string descricao, decimal preco, int estoque)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Estoque = estoque;
    }

    public void Atualizar(string nome, string descricao, decimal preco)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
    }

    public void AjustarEstoque(int quantidade)
    {
        Estoque += quantidade;
    }
}

