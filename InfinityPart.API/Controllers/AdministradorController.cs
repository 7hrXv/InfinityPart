using InfinityPart.Application.DTOs.Administradores;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdministradorController : ControllerBase
{
    private readonly IAdministradorService _administradorService;

    public AdministradorController(IAdministradorService administradorService)
    {
        _administradorService = administradorService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_administradorService.Listar());
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var administrador = _administradorService.BuscarPorId(id);

        if (administrador == null)
            return NotFound();

        return Ok(administrador);
    }

    [HttpGet("cpf/{cpf}")]
    public IActionResult BuscarPorCpf(string cpf)
    {
        var administrador = _administradorService.BuscarPorCpf(cpf);

        if (administrador == null)
            return NotFound();

        return Ok(administrador);
    }

    [HttpPost]
    public IActionResult Criar(CriarAdministradorDto dto)
    {
        var administrador = _administradorService.Criar(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = administrador.Id },
            administrador);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, AtualizarAdministradorDto dto)
    {
        dto.Id = id;

        var administrador = _administradorService.Atualizar(dto);

        if (administrador == null)
            return NotFound();

        return Ok(administrador);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        if (!_administradorService.Remover(id))
            return NotFound();

        return NoContent();
    }
}