using InfinityPart.Application.DTOs.ItensPedido;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemPedidoController : ControllerBase
{
    private readonly IItemPedidoService _itemPedidoService;

    public ItemPedidoController(IItemPedidoService itemPedidoService)
    {
        _itemPedidoService = itemPedidoService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_itemPedidoService.Listar());
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var item = _itemPedidoService.BuscarPorId(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("pedido/{pedidoId}")]
    public IActionResult BuscarPorPedidoId(int pedidoId)
    {
        return Ok(_itemPedidoService.BuscarPorPedidoId(pedidoId));
    }

    [HttpGet("produto/{produtoId}")]
    public IActionResult BuscarPorProdutoId(int produtoId)
    {
        return Ok(_itemPedidoService.BuscarPorProdutoId(produtoId));
    }

    [HttpPost]
    public IActionResult Criar(CriarItemPedidoDto dto)
    {
        var item = _itemPedidoService.Criar(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = item.Id },
            item);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, AtualizarItemPedidoDto dto)
    {
        dto.Id = id;

        var item = _itemPedidoService.Atualizar(dto);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        if (!_itemPedidoService.Remover(id))
            return NotFound();

        return NoContent();
    }
}