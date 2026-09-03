using InfinityPart.Application.DTOs.Auditoria;
using InfinityPart.Application.Interfaces;
using InfinityPart.Domain.Interfaces;
using InfinityPart.Entidades;

namespace InfinityPart.Application.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _auditoriaRepository;

    public AuditoriaService(IAuditoriaRepository auditoriaRepository)
    {
        _auditoriaRepository = auditoriaRepository;
    }

    public AuditoriaDto RegistrarLog(CriarAuditoriaDto dto)
    {
        var auditoria = new Auditoria
        {
            UsuarioId = dto.UsuarioId,
            UsuarioNome = dto.UsuarioNome,
            Acao = dto.Acao,
            Tabela = dto.Tabela,
            ValoresAntigos = dto.ValoresAntigos,
            ValoresNovos = dto.ValoresNovos
        };

        _auditoriaRepository.RegistrarLog(auditoria);

        return MapearParaDto(auditoria);
    }

    public IEnumerable<AuditoriaDto> ObterUltimosLogs(int quantidade)
    {
        var logs = _auditoriaRepository.ObterUltimosLogs(quantidade);

        return logs.Select(MapearParaDto);
    }

    public IEnumerable<AuditoriaDto> ObterPorUsuario(string usuarioId)
    {
        var logs = _auditoriaRepository.ObterPorUsuario(usuarioId);

        return logs.Select(MapearParaDto);
    }

    public IEnumerable<AuditoriaDto> ObterPorData(
        DateTime dataInicio,
        DateTime dataFim)
    {
        var logs = _auditoriaRepository.ObterPorData(
            dataInicio,
            dataFim);

        return logs.Select(MapearParaDto);
    }

    public IEnumerable<AuditoriaDto> ObterApenasErros()
    {
        var logs = _auditoriaRepository.ObterApenasErros();

        return logs.Select(MapearParaDto);
    }

    private static AuditoriaDto MapearParaDto(Auditoria auditoria)
    {
        return new AuditoriaDto
        {
            Id = auditoria.Id,
            UsuarioId = auditoria.UsuarioId,
            UsuarioNome = auditoria.UsuarioNome,
            Acao = auditoria.Acao,
            Tabela = auditoria.Tabela,
            DataHora = auditoria.DataHora,
            ValoresAntigos = auditoria.ValoresAntigos,
            ValoresNovos = auditoria.ValoresNovos
        };
    }
}