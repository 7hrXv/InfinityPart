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

    // GET: api/Cliente
    [HttpGet]
    public IActionResult Listar()
    {
        var clientes = _clienteService.Listar();

        return Ok(clientes);
    }

    // GET: api/Cliente/5
    [HttpGet("{id:int}")]
    public IActionResult BuscarPorId(int id)
    {
        var cliente = _clienteService.BuscarPorId(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    // POST: api/Cliente
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteDto dto)
    {
        var cliente = await _clienteService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = cliente.Id },
            cliente);
    }

    // PUT: api/Cliente/5
    [HttpPut("{id:int}")]
    public IActionResult Atualizar(
        int id,
        [FromBody] AtualizarClienteDto dto)
    {
        dto.Id = id;

        var cliente = _clienteService.Atualizar(dto);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    // DELETE: api/Cliente/5
    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        var removido = _clienteService.Remover(id);

        if (!removido)
            return NotFound();

        return NoContent();
    }
}