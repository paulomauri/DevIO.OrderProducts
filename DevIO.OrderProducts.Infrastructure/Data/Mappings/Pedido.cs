using DevIO.OrderProducts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.OrderProducts.Infrastructure.Data.Mappings;

public class Pedido : IEntityTypeConfiguration<DevIO.OrderProducts.Domain.Entities.Pedido>
{
    public void Configure(EntityTypeBuilder<DevIO.OrderProducts.Domain.Entities.Pedido> builder)
    {
        builder.ToTable("Pedido");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.DataCriacao)
            .HasColumnType("Datetime")
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.HasMany(p => p.Itens)
            .WithOne()
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
