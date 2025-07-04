using DevIO.Auth.Infrastructure;
using DevIO.Auth.Models;
using DevIO.Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevIO.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;


    public AuthController(IOptions<JwtSettings> jwtSettings, ITokenService tokenService, IUserService userService)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.AuthenticateAsync(request);
        if (user is null)
            return Unauthorized("Credenciais inválidas.");

        var accessToken = _tokenService.GenerateAccessToken(user.Username, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken(user.Username);

        // Você pode persistir o refresh token aqui também (memória, banco ou Redis)
        InMemoryRefreshTokenStore.Save(refreshToken);

        return Ok(new
        {
            access_token = accessToken,
            refresh_token = refreshToken.Token
        });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] string refreshToken)
    {
        var token = InMemoryRefreshTokenStore.Get(refreshToken);
        if (token is null || token.IsExpired)
            return Unauthorized("Refresh token inválido ou expirado.");

        var newAccessToken = _tokenService.GenerateAccessToken(token.Username, "Admin");
        var newRefreshToken = _tokenService.GenerateRefreshToken(token.Username);

        InMemoryRefreshTokenStore.Remove(refreshToken);
        InMemoryRefreshTokenStore.Save(newRefreshToken);

        return Ok(new
        {
            access_token = newAccessToken,
            refresh_token = newRefreshToken.Token
        });
    }
}
