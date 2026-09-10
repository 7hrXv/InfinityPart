using InfinityPart.Application.DTOs.Categorias;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        var categorias = _categoriaService.Listar();

        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var categoria = _categoriaService.BuscarPorId(id);

        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    public IActionResult Criar(CriarCategoriaDto dto)
    {
        var categoria = _categoriaService.Criar(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = categoria.Id },
            categoria);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, AtualizarCategoriaDto dto)
    {
        dto.Id = id;

        var categoria = _categoriaService.Atualizar(dto);

        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _categoriaService.Remover(id);

        if (!removido)
            return NotFound();

        return NoContent();
    }
}