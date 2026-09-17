namespace InfinityPart.Application.DTOs.Autenticacao;

/// <summary>
/// Define ou altera a senha de um administrador.
/// SenhaAtual é obrigatória quando já existe senha cadastrada;
/// no primeiro acesso (sem senha definida) pode vir vazia.
/// </summary>
public class DefinirSenhaDto
{
    public int AdministradorId { get; set; }

    public string? SenhaAtual { get; set; }

    public string NovaSenha { get; set; } = string.Empty;
}
