using Microsoft.EntityFrameworkCore;

namespace DevIO.Auth.DatabaseContext.Mappings
{
    public class User : IEntityTypeConfiguration<DevIO.Auth.Models.User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DevIO.Auth.Models.User> builder)
        {
            builder.ToTable("User");
     
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Username)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);
            
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(1000);
        }
    }
}
