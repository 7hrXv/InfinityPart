namespace InfinittyPart.Domain.Entidades;

public class Administrador
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Hash da senha (PBKDF2). Nunca guarda a senha em texto puro.
    /// Vazio significa que o administrador ainda não definiu senha e por isso
    /// não consegue autenticar até que a primeira senha seja definida.
    /// </summary>
    public string SenhaHash { get; set; } = string.Empty;

    /// <summary>
    /// Administrador inativo não consegue autenticar.
    /// </summary>
    public bool Ativo { get; set; } = true;

    public DateTime? UltimoAcesso { get; set; }

    public bool PossuiSenhaDefinida => !string.IsNullOrWhiteSpace(SenhaHash);

    public void RegistrarAcesso()
    {
        UltimoAcesso = DateTime.UtcNow;
    }
}
