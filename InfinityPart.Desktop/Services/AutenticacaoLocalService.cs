using InfinityPart.Desktop.Models;

namespace InfinityPart.Desktop.Services
{
    /// <summary>
    /// Autenticação LOCAL e TEMPORÁRIA para o FrmLogin, usada apenas em testes
    /// enquanto a InfinityPart.API não está em execução.
    ///
    /// NÃO faz nenhuma chamada de rede: não depende do ApiClient nem da API.
    /// A API e todo o seu código permanecem intactos em InfinityPart.API para
    /// serem usados novamente depois — basta trocar, no FrmLogin, a chamada
    /// para AutenticacaoApiService.LoginAsync(...) quando a API voltar a ser usada.
    ///
    /// Credenciais fixas de teste (apenas para uso local/desenvolvimento):
    ///   Usuário: admin
    ///   Senha:   admin123
    /// </summary>
    public static class AutenticacaoLocalService
    {
        private const string UsuarioTeste = "admin";
        private const string SenhaTeste = "123";

        /// <summary>
        /// Mesma assinatura de AutenticacaoApiService.LoginAsync, para que o FrmLogin
        /// possa alternar entre autenticação local e autenticação via API trocando
        /// apenas o nome da classe chamada.
        /// </summary>
        public static Task<LoginResultadoModel> LoginAsync(string identificador, string senha)
        {
            var resultado = Autenticar(identificador, senha);
            return Task.FromResult(resultado);
        }

        private static LoginResultadoModel Autenticar(string identificador, string senha)
        {
            var credenciaisValidas =
                string.Equals(identificador.Trim(), UsuarioTeste, StringComparison.OrdinalIgnoreCase) &&
                senha == SenhaTeste;

            if (!credenciaisValidas)
            {
                SessaoAtual.Encerrar();

                return new LoginResultadoModel
                {
                    Autenticado = false,
                    Mensagem = "Usuário ou senha inválidos. (login local de teste: admin / admin123)"
                };
            }

            var administrador = new AdministradorModel
            {
                Id = 0,
                Nome = "Administrador (Local)",
                Email = "admin@local.teste",
                Cpf = "000.000.000-00",
                Telefone = string.Empty,
                DataCadastro = DateTime.Now,
                Ativo = true,
                UltimoAcesso = DateTime.Now,
                PossuiSenhaDefinida = true
            };

            SessaoAtual.Administrador = administrador;

            return new LoginResultadoModel
            {
                Autenticado = true,
                Mensagem = "Login local de teste realizado com sucesso.",
                SenhaNaoDefinida = false,
                Administrador = administrador
            };
        }
    }
}
