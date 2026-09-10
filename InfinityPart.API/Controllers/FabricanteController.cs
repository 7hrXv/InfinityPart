using InfinityPart.Application.DTOs.Fabricantes;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricanteController : ControllerBase
{
    private readonly IFabricanteService _fabricanteService;

    public FabricanteController(IFabricanteService fabricanteService)
    {
        _fabricanteService = fabricanteService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        var fabricantes = _fabricanteService.Listar();

        return Ok(fabricantes);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var fabricante = _fabricanteService.BuscarPorId(id);

        if (fabricante == null)
            return NotFound();

        return Ok(fabricante);
    }

    [HttpPost]
    public IActionResult Criar(CriarFabricanteDto dto)
    {
        var fabricante = _fabricanteService.Criar(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = fabricante.Id },
            fabricante);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(
        int id,
        AtualizarFabricanteDto dto)
    {
        dto.Id = id;

        var fabricante = _fabricanteService.Atualizar(dto);

        if (fabricante == null)
            return NotFound();

        return Ok(fabricante);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _fabricanteService.Remover(id);

        if (!removido)
            return NotFound();

        return NoContent();
    }
}