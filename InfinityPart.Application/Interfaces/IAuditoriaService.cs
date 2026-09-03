using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityPart.Application.DTOs.Auditoria;

namespace InfinityPart.Application.Interfaces;

public interface IAuditoriaService
{
    AuditoriaDto RegistrarLog(CriarAuditoriaDto dto);

    IEnumerable<AuditoriaDto> ObterUltimosLogs(int quantidade);

    IEnumerable<AuditoriaDto> ObterPorUsuario(string usuarioId);

    IEnumerable<AuditoriaDto> ObterPorData(
        DateTime dataInicio,
        DateTime dataFim);

    IEnumerable<AuditoriaDto> ObterApenasErros();
}