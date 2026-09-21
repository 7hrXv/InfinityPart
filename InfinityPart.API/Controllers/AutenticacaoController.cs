using InfinityPart.Application.DTOs.Autenticacao;
using InfinityPart.Application.Exceptions;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController : ControllerBase
{
    private readonly IAutenticacaoService _autenticacaoService;

    public AutenticacaoController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    // =========================================================
    // LOGIN DE ADMINISTRADOR
    // =========================================================

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var resultado = _autenticacaoService.Autenticar(dto);

        if (!resultado.Autenticado)
            return Unauthorized(resultado);

        return Ok(resultado);
    }

    // =========================================================
    // LOGIN DE CLIENTE
    // =========================================================

    [HttpPost("login-cliente")]
    public IActionResult LoginCliente([FromBody] LoginDto dto)
    {
        var resultado = _autenticacaoService.AutenticarCliente(dto);

        if (!resultado.Autenticado)
            return Unauthorized(resultado);

        return Ok(resultado);
    }

    // =========================================================
    // DEFINIR / ALTERAR SENHA DO ADMINISTRADOR
    // =========================================================

    [HttpPost("definir-senha")]
    public IActionResult DefinirSenha([FromBody] DefinirSenhaDto dto)
    {
        try
        {
            _autenticacaoService.DefinirSenha(dto);

            return NoContent();
        }
        catch (RecursoNaoEncontradoException ex)
        {
            return NotFound(new
            {
                mensagem = ex.Message
            });
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
    }
}