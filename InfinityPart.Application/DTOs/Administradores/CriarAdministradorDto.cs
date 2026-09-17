namespace InfinityPart.Application.DTOs.Administradores;

public class CriarAdministradorDto
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    /// <summary>
    /// Senha em texto puro recebida apenas em trânsito; é convertida em hash
    /// antes de qualquer persistência e nunca é armazenada nem retornada.
    /// </summary>
    public string Senha { get; set; } = string.Empty;
}
