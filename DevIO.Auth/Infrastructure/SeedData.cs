using DevIO.Auth.DatabaseContext;
using DevIO.Auth.Models;
using Microsoft.AspNetCore.Identity;

namespace DevIO.Auth.Infrastructure;

public static class SeedData
{
    public static void EnsureSeeded(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!context.Users.Any())
        {
            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Role = "Admin"
            };

            admin.PasswordHash = hasher.HashPassword(admin, "admin");

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
