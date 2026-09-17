namespace InfinityPart.Desktop.Models
{
    /// <summary>
    /// Corpo enviado para POST api/autenticacao/login.
    /// O identificador aceita usuário (nome), e-mail ou CPF.
    /// </summary>
    public class LoginRequestModel
    {
        public string Identificador { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }

    /// <summary>
    /// Resposta de POST api/autenticacao/login (200 ou 401).
    /// </summary>
    public class LoginResultadoModel
    {
        public bool Autenticado { get; set; }

        public string Mensagem { get; set; } = string.Empty;

        public bool SenhaNaoDefinida { get; set; }

        public AdministradorModel? Administrador { get; set; }
    }
}
