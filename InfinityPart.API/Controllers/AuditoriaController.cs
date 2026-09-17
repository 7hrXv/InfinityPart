using InfinityPart.Application.DTOs.Auditoria;
using InfinityPart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    [HttpPost]
    public IActionResult RegistrarLog(CriarAuditoriaDto dto)
    {
        var auditoria = _auditoriaService.RegistrarLog(dto);

        return Ok(auditoria);
    }

    [HttpGet("ultimos/{quantidade}")]
    public IActionResult Ultimos(int quantidade)
    {
        var auditorias = _auditoriaService.ObterUltimosLogs(quantidade);

        return Ok(auditorias);
    }

    [HttpGet("usuario/{usuarioId}")]
    public IActionResult PorUsuario(string usuarioId)
    {
        var auditorias = _auditoriaService.ObterPorUsuario(usuarioId);

        return Ok(auditorias);
    }

    [HttpGet("data")]
    public IActionResult PorData(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        var auditorias = _auditoriaService.ObterPorData(
            dataInicio,
            dataFim);

        return Ok(auditorias);
    }

    [HttpGet("erros")]
    public IActionResult Erros()
    {
        var auditorias = _auditoriaService.ObterApenasErros();

        return Ok(auditorias);
    }
}