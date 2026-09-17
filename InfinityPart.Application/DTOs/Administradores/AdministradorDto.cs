namespace InfinityPart.Application.DTOs.Administradores;

public class AdministradorDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public DateTime DataCadastro { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime? UltimoAcesso { get; set; }

    /// <summary>
    /// Indica se o administrador já definiu senha (nunca expõe o hash).
    /// </summary>
    public bool PossuiSenhaDefinida { get; set; }
}
