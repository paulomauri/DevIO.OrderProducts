using Microsoft.EntityFrameworkCore;

namespace DevIO.Auth.DatabaseContext.Mappings
{
    public class UserClaim : IEntityTypeConfiguration<DevIO.Auth.Models.UserClaim>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DevIO.Auth.Models.UserClaim> builder)
        {
            builder.ToTable("UserClaim");

            builder.HasKey(uc => uc.Id);

            builder.Property(uc => uc.Type)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(uc => uc.Value)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(500);

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(uc => uc.UserId);
        }
    }
}
