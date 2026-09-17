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

    /// <summary>
    /// POST api/autenticacao/login
    /// Valida as credenciais do administrador (usuário/e-mail/CPF + senha).
    /// Retorna 200 com os dados do administrador ou 401 com a mensagem de erro.
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var resultado = _autenticacaoService.Autenticar(dto);

        if (!resultado.Autenticado)
            return Unauthorized(resultado);

        return Ok(resultado);
    }

    /// <summary>
    /// POST api/autenticacao/definir-senha
    /// Define a primeira senha ou troca a senha existente (exige a senha atual).
    /// </summary>
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
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
