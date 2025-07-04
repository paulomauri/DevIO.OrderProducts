using DevIO.Auth.DatabaseContext;
using DevIO.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Auth.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _hasher = new();

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            return false;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Role = request.Role
        };

        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User?> AuthenticateAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Claims)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        return result == PasswordVerificationResult.Success ? user : null;
    }
}
