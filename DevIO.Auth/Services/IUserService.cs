using DevIO.Auth.Models;

namespace DevIO.Auth.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<User?> AuthenticateAsync(LoginRequest request);
}
