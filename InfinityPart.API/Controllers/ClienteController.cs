using InfinityPart.Application.DTOs.Clientes;
using InfinityPart.Application.Exceptions;
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

    [HttpGet("{id:int}")]
    public IActionResult BuscarPorId(int id)
    {
        var cliente = _clienteService.BuscarPorId(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarClienteDto dto)
    {
        try
        {
            var cliente =
                await _clienteService.CriarAsync(dto);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = cliente.Id },
                cliente
            );
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Atualizar(
        int id,
        [FromBody] AtualizarClienteDto dto)
    {
        try
        {
            dto.Id = id;

            var cliente =
                _clienteService.Atualizar(dto);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        try
        {
            var removido =
                _clienteService.Remover(id);

            if (!removido)
                return NotFound();

            return NoContent();
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
    }
}