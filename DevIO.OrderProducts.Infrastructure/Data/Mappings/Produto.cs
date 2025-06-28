

using DevIO.OrderProducts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.OrderProducts.Infrastructure.Data.Mappings;

public class Produto : IEntityTypeConfiguration<DevIO.OrderProducts.Domain.Entities.Produto>
{
    public void Configure(EntityTypeBuilder<DevIO.OrderProducts.Domain.Entities.Produto> builder)
    {
        builder.ToTable("Produto");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Descricao)
            .HasMaxLength(255);

        builder.Property(p => p.Preco)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Estoque)
            .HasColumnType("INT");
    }
}
