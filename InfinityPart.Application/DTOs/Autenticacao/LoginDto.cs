namespace InfinityPart.Application.DTOs.Autenticacao;

/// <summary>
/// Credenciais enviadas pelo cliente (Desktop/Web) para autenticação.
/// O identificador aceita nome de usuário, e-mail ou CPF, conforme a
/// estrutura atual da entidade Administrador.
/// </summary>
public class LoginDto
{
    public string Identificador { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
}
