using InfinityPart.Application.DTOs.Auth;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(RegistrarUsuarioDto dto)
    {
        try
        {
            var usuario = await _authService.RegistrarAsync(dto);

            return Ok(usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var usuario = await _authService.LoginAsync(dto);

        if (usuario == null)
            return Unauthorized("E-mail ou senha inválidos.");

        return Ok(usuario);
    }
}