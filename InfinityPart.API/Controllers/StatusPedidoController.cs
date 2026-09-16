using InfinityPart.Application.DTOs.StatusPedidos;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusPedidoController : ControllerBase
{
    private readonly IStatusPedidoService _statusPedidoService;

    public StatusPedidoController(IStatusPedidoService statusPedidoService)
    {
        _statusPedidoService = statusPedidoService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_statusPedidoService.Listar());
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var status = _statusPedidoService.BuscarPorId(id);

        if (status == null)
            return NotFound();

        return Ok(status);
    }

    [HttpGet("nome/{nome}")]
    public IActionResult BuscarPorNome(string nome)
    {
        var status = _statusPedidoService.BuscarPorNome(nome);

        if (status == null)
            return NotFound();

        return Ok(status);
    }

    [HttpPost]
    public IActionResult Criar(CriarStatusPedidoDto dto)
    {
        var status = _statusPedidoService.Criar(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = status.Id },
            status);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, AtualizarStatusPedidoDto dto)
    {
        dto.Id = id;

        var status = _statusPedidoService.Atualizar(dto);

        if (status == null)
            return NotFound();

        return Ok(status);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        if (!_statusPedidoService.Remover(id))
            return NotFound();

        return NoContent();
    }
}