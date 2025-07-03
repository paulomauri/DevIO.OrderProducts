using DevIO.Auth.Models;

namespace DevIO.Auth.Services;

public interface ITokenService
{
    string GenerateAccessToken(string username, string role);
    RefreshToken GenerateRefreshToken(string username);
}
