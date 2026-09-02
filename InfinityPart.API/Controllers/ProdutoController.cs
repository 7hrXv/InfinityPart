using InfinityPart.Application.DTOs.Produtos;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    // GET: api/Produto
    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDto>> Listar()
    {
        var produtos = _produtoService.Listar();

        return Ok(produtos);
    }

    // GET: api/Produto/1
    [HttpGet("{id}")]
    public ActionResult<ProdutoDto> BuscarPorId(int id)
    {
        var produto = _produtoService.BuscarPorId(id);

        if (produto == null)
            return NotFound();

        return Ok(produto);
    }

    // POST: api/Produto
    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> Criar(CriarProdutoDto dto)
    {
        var produto = await _produtoService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = produto.Id },
            produto);
    }

    // PUT: api/Produto/1
    [HttpPut("{id}")]
    public ActionResult<ProdutoDto> Atualizar(
        int id,
        AtualizarProdutoDto dto)
    {
        if (id != dto.Id)
            return BadRequest("O ID da URL é diferente do ID do produto.");

        var produto = _produtoService.Atualizar(dto);

        if (produto == null)
            return NotFound();

        return Ok(produto);
    }

    // DELETE: api/Produto/1
    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _produtoService.Remover(id);

        if (!removido)
            return NotFound();

        return NoContent();
    }
}