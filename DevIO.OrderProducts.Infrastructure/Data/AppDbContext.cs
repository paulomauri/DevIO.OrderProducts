using DevIO.OrderProducts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevIO.OrderProducts.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produto> Produto => Set<Produto>();
    public DbSet<Pedido> Pedido => Set<Pedido>();
    public DbSet<ItemPedido> ItemPedido => Set<ItemPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Mappings.Produto());
        modelBuilder.ApplyConfiguration(new Mappings.Pedido());
        modelBuilder.ApplyConfiguration(new Mappings.ItemPedido());
    }
}
