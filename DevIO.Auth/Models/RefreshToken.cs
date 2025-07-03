namespace DevIO.Auth.Models;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expiration;
}
