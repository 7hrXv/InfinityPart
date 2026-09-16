using InfinityPart.Application.DTOs.Marcas;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarcaController : ControllerBase
{
    private readonly IMarcaService _marcaService;

    public MarcaController(IMarcaService marcaService)
    {
        _marcaService = marcaService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_marcaService.Listar());
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var marca = _marcaService.BuscarPorId(id);

        if (marca == null)
            return NotFound();

        return Ok(marca);
    }

    [HttpGet("nome/{nome}")]
    public IActionResult BuscarPorNome(string nome)
    {
        var marca = _marcaService.BuscarPorNome(nome);

        if (marca == null)
            return NotFound();

        return Ok(marca);
    }

    [HttpPost]
    public IActionResult Criar(CriarMarcaDto dto)
    {
        var marca = _marcaService.Criar(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = marca.Id },
            marca);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, AtualizarMarcaDto dto)
    {
        dto.Id = id;

        var marca = _marcaService.Atualizar(dto);

        if (marca == null)
            return NotFound();

        return Ok(marca);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        if (!_marcaService.Remover(id))
            return NotFound();

        return NoContent();
    }
}