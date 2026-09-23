namespace InfinityPart.Application.DTOs.Autenticacao;

/// <summary>
/// Credenciais utilizadas para autenticação.
/// Para administrador, o identificador pode ser usuário, e-mail ou CPF.
/// Para cliente, o identificador será o e-mail.
/// </summary>
public class LoginDto
{
    public string Identificador { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
}