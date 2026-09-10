using InfinityPart.Application.DTOs.Clientes;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        var clientes = _clienteService.Listar();

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var cliente = _clienteService.BuscarPorId(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public IActionResult Criar(CriarClienteDto dto)
    {
        var cliente = _clienteService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = cliente.Id },
            cliente);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, AtualizarClienteDto dto)
    {
        dto.Id = id;

        var cliente = _clienteService.Atualizar(dto);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _clienteService.Remover(id);

        if (!removido)
            return NotFound();

        return NoContent();
    }
}