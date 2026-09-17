using InfinityPart.Application.DTOs.Administradores;

namespace InfinityPart.Application.DTOs.Autenticacao;

/// <summary>
/// Resultado da tentativa de autenticação. Nunca expõe hash nem senha.
/// </summary>
public class LoginResultadoDto
{
    public bool Autenticado { get; set; }

    public string Mensagem { get; set; } = string.Empty;

    /// <summary>
    /// Indica que as credenciais estão corretas, porém o administrador
    /// ainda não possui senha definida (primeiro acesso).
    /// </summary>
    public bool SenhaNaoDefinida { get; set; }

    public AdministradorDto? Administrador { get; set; }

    public static LoginResultadoDto Falha(string mensagem) => new()
    {
        Autenticado = false,
        Mensagem = mensagem
    };

    public static LoginResultadoDto Sucesso(AdministradorDto administrador) => new()
    {
        Autenticado = true,
        Mensagem = "Autenticado com sucesso.",
        Administrador = administrador
    };
}
