using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityPart.Application.DTOs.Auditoria;

public class AuditoriaDto
{
    public int Id { get; set; }

    public string UsuarioId { get; set; } = string.Empty;
    public string UsuarioNome { get; set; } = string.Empty;

    public string Acao { get; set; } = string.Empty;
    public string Tabela { get; set; } = string.Empty;

    public DateTime DataHora { get; set; }

    public string? ValoresAntigos { get; set; }
    public string? ValoresNovos { get; set; }
}