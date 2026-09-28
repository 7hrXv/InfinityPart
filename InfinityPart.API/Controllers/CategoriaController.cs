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

    // GET: api/Categoria
    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDto>> Listar()
    {
        var categorias = _categoriaService.Listar();

        return Ok(categorias);
    }

    // GET: api/Categoria/1
    [HttpGet("{id}")]
    public ActionResult<CategoriaDto> BuscarPorId(int id)
    {
        var categoria = _categoriaService.BuscarPorId(id);

        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    // POST: api/Categoria
    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Criar(
        CriarCategoriaDto dto)
    {
        var categoria = await _categoriaService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = categoria.Id },
            categoria);
    }

    // PUT: api/Categoria/1
    [HttpPut("{id}")]
    public ActionResult<CategoriaDto> Atualizar(
        int id,
        AtualizarCategoriaDto dto)
    {
        if (id != dto.Id)
            return BadRequest(
                "O ID da URL é diferente do ID da categoria.");

        var categoria = _categoriaService.Atualizar(dto);

        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    // DELETE: api/Categoria/1
    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _categoriaService.Remover(id);

        if (!removido)
            return NotFound();

        return NoContent();
    }
}