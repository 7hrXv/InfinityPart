using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    /// <summary>
    /// Tela de autenticação.
    ///
    /// MODO ATUAL: autenticação LOCAL TEMPORÁRIA (AutenticacaoLocalService), apenas
    /// para testes — não depende da API estar em execução. Credenciais de teste:
    /// usuário "admin", senha "admin123".
    ///
    /// A integração com a API (AutenticacaoApiService, POST api/autenticacao/login)
    /// continua intacta e pode ser restaurada trocando a chamada em EntrarAsync().
    /// </summary>
    public partial class FrmLogin : Form
    {
        private bool _autenticando;

        public FrmLogin()
        {
            InitializeComponent();

            btnEntrar.Click += async (_, _) => await EntrarAsync();
            chkMostrarSenha.CheckedChanged += (_, _) =>
                txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;

            txtIdentificador.TextChanged += (_, _) => LimparErro();
            txtSenha.TextChanged += (_, _) => LimparErro();

            Load += (_, _) =>
            {
                // Modo local temporário: não consulta a API (ApiClient.BaseUrl) para logar.
                lblApi.Text = "Modo local (teste) — usuário: admin / senha: admin123";
                txtIdentificador.Focus();
            };

            Resize += (_, _) => CentralizarCard();
        }

        private async Task EntrarAsync()
        {
            if (_autenticando)
                return;

            var identificador = txtIdentificador.Text.Trim();
            var senha = txtSenha.Text;

            if (identificador.Length == 0)
            {
                MostrarErro("Informe o usuário, e-mail ou CPF.");
                txtIdentificador.Focus();
                return;
            }

            if (senha.Length == 0)
            {
                MostrarErro("Informe a senha.");
                txtSenha.Focus();
                return;
            }

            DefinirCarregando(true);

            try
            {
                // TEMPORÁRIO: autenticação local, sem chamar a API.
                // Para voltar a autenticar via API, troque a linha abaixo por:
                //   var resultado = await AutenticacaoApiService.LoginAsync(identificador, senha);
                var resultado = await AutenticacaoLocalService.LoginAsync(identificador, senha);

                if (resultado.Autenticado && resultado.Administrador != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                MostrarErro(resultado.Mensagem);
                txtSenha.Clear();
                txtSenha.Focus();
            }
            catch (Exception ex)
            {
                MostrarErro("Erro inesperado ao autenticar. Tente novamente.");
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                DefinirCarregando(false);
            }
        }

        private void DefinirCarregando(bool carregando)
        {
            _autenticando = carregando;

            prgCarregando.Visible = carregando;
            lblCarregando.Visible = carregando;

            if (carregando)
                lblErro.Visible = false;

            btnEntrar.Enabled = !carregando;
            btnEntrar.Text = carregando ? "Entrando..." : "Entrar";

            txtIdentificador.Enabled = !carregando;
            txtSenha.Enabled = !carregando;
            chkMostrarSenha.Enabled = !carregando;

            Cursor = carregando ? Cursors.WaitCursor : Cursors.Default;
        }

        private void MostrarErro(string mensagem)
        {
            lblErro.Text = mensagem;
            lblErro.ForeColor = AppTheme.PrimaryRedHover;
            lblErro.Visible = true;
        }

        private void LimparErro()
        {
            if (lblErro.Visible)
                lblErro.Visible = false;
        }
    }
}
