using InfinityPart.Desktop.Models;

namespace InfinityPart.Desktop.Services
{
    /// <summary>
    /// Consome os endpoints de autenticação da API.
    /// Nenhuma credencial é gravada em disco nem fixada em código:
    /// a senha digitada é enviada à API e validada contra o hash do banco.
    /// </summary>
    public static class AutenticacaoApiService
    {
        /// <summary>
        /// POST api/autenticacao/login.
        /// Em caso de sucesso preenche a SessaoAtual com o administrador autenticado.
        /// </summary>
        public static async Task<LoginResultadoModel> LoginAsync(string identificador, string senha)
        {
            var corpo = new LoginRequestModel
            {
                Identificador = identificador,
                Senha = senha
            };

            var resposta = await ApiClient.PostComRespostaAsync<LoginResultadoModel, LoginRequestModel>(
                "autenticacao/login", corpo);

            var resultado = resposta.Dados ?? new LoginResultadoModel
            {
                Autenticado = false,
                Mensagem = "Resposta inesperada da API."
            };

            if (resposta.Sucesso && resultado.Autenticado && resultado.Administrador != null)
            {
                SessaoAtual.Administrador = resultado.Administrador;
            }
            else
            {
                SessaoAtual.Encerrar();

                if (string.IsNullOrWhiteSpace(resultado.Mensagem))
                    resultado.Mensagem = "Usuário ou senha inválidos.";
            }

            return resultado;
        }
    }
}
