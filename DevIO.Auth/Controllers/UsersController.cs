using DevIO.Auth.Models;
using DevIO.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UsersController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var success = await _userService.RegisterAsync(request);
        if (!success)
            return BadRequest("Usuário já existe.");

        return Ok("Usuário registrado com sucesso.");
    }

}
