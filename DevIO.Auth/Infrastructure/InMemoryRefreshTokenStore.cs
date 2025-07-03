using DevIO.Auth.Models;

namespace DevIO.Auth.Infrastructure;

public static class InMemoryRefreshTokenStore
{
    private static readonly List<RefreshToken> Tokens = new();

    public static void Save(RefreshToken token)
    {
        Tokens.RemoveAll(t => t.Username == token.Username);
        Tokens.Add(token);
    }

    public static RefreshToken? Get(string token) =>
        Tokens.FirstOrDefault(t => t.Token == token);

    public static void Remove(string token) =>
        Tokens.RemoveAll(t => t.Token == token);
}
