using DevIO.OrderProducts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.OrderProducts.Infrastructure.Data.Mappings;

public class ItemPedido : IEntityTypeConfiguration<DevIO.OrderProducts.Domain.Entities.ItemPedido>
{
    public void Configure(EntityTypeBuilder<DevIO.OrderProducts.Domain.Entities.ItemPedido> builder)
    {
        builder.ToTable("ItemPedido");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Observacao)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.PrecoUnitario)
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.Quantidade)
            .IsRequired();
    }
}
