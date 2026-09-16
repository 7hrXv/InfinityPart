using InfinityPart.Application.DTOs.Pedidos;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        var pedidos = _pedidoService.Listar();

        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var pedido = _pedidoService.BuscarPorId(id);

        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    [HttpGet("cliente/{clienteId}")]
    public IActionResult BuscarPorClienteId(int clienteId)
    {
        var pedidos = _pedidoService.BuscarPorClienteId(clienteId);

        return Ok(pedidos);
    }

    [HttpPost]
    public IActionResult Criar(CriarPedidoDto dto)
    {
        var pedido = _pedidoService.Criar(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = pedido.Id },
            pedido);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, AtualizarPedidoDto dto)
    {
        dto.Id = id;

        var pedido = _pedidoService.Atualizar(dto);

        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _pedidoService.Remover(id);

        if (!removido)
            return NotFound();

        return NoContent();
    }
}