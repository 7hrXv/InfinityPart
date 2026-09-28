using System.Drawing;
using InfinityPart.Desktop.Models;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmLogin : Form
    {
        private const float LARGURA_REFERENCIA = 900f;
        private const float ALTURA_REFERENCIA = 700f;

        private const float PAINEL_LARGURA_REFERENCIA = 450f;
        private const float PAINEL_ALTURA_REFERENCIA = 590f;

        private Panel pnlPrincipal = null!;
        private Panel pnlLogin = null!;

        private Panel pnlSenha = null!;

        private TextBox txtIdentificador = null!;
        private TextBox txtSenha = null!;

        private Button btnEntrar = null!;
        private Button btnMostrarSenha = null!;

        private CheckBox chkMostrarSenha = null!;

        private Label lblStatus = null!;

        private readonly List<ControleEscalavel> controlesEscalaveis = new();

        private bool carregando;
        private bool aplicandoEscala;

        private class ControleEscalavel
        {
            public Control Controle { get; }
            public Rectangle BoundsBase { get; }
            public float TamanhoFonteBase { get; }

            public ControleEscalavel(
                Control controle,
                Rectangle boundsBase,
                float tamanhoFonteBase)
            {
                Controle = controle;
                BoundsBase = boundsBase;
                TamanhoFonteBase = tamanhoFonteBase;
            }
        }

        public FrmLogin()
        {
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            Text = "InfinityPart - Login";

            StartPosition = FormStartPosition.CenterScreen;

            FormBorderStyle = FormBorderStyle.Sizable;

            MaximizeBox = true;
            MinimizeBox = true;

            ClientSize = new Size(900, 700);

            MinimumSize = new Size(760, 620);

            AutoScaleMode = AutoScaleMode.Dpi;

            BackColor = AppTheme.Background;
            ForeColor = AppTheme.TextPrimary;

            Font = new Font(
                AppTheme.FontFamily,
                10f);

            pnlPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppTheme.Background
            };

            pnlPrincipal.Resize += PnlPrincipal_Resize;

            Controls.Add(pnlPrincipal);

            CriarLogin();

            Shown += (_, _) =>
            {
                AplicarEscala();
                txtIdentificador.Focus();
            };
        }

        private void CriarLogin()
        {
            pnlLogin = new Panel
            {
                Size = new Size(
                    (int)PAINEL_LARGURA_REFERENCIA,
                    (int)PAINEL_ALTURA_REFERENCIA),
                BackColor = Color.Transparent
            };

            pnlPrincipal.Controls.Add(pnlLogin);

            CriarLogo();

            CriarTitulo();

            CriarCampoUsuario();

            CriarCampoSenha();

            CriarMostrarSenha();

            CriarBotaoEntrar();

            CriarStatus();

            AplicarEscala();
        }

        private void PnlPrincipal_Resize(
            object? sender,
            EventArgs e)
        {
            AplicarEscala();
        }

        private void AplicarEscala()
        {
            if (aplicandoEscala ||
                pnlPrincipal == null ||
                pnlLogin == null)
            {
                return;
            }

            if (pnlPrincipal.ClientSize.Width <= 0 ||
                pnlPrincipal.ClientSize.Height <= 0)
            {
                return;
            }

            aplicandoEscala = true;

            try
            {
                float escalaX =
                    pnlPrincipal.ClientSize.Width /
                    LARGURA_REFERENCIA;

                float escalaY =
                    pnlPrincipal.ClientSize.Height /
                    ALTURA_REFERENCIA;

                /*
                 * Usa a menor escala para manter
                 * a proporção do formulário.
                 */
                float escala =
                    Math.Min(escalaX, escalaY);

                /*
                 * Evita que o conteúdo fique menor
                 * do que o tamanho mínimo planejado.
                 */
                escala = Math.Max(escala, 0.82f);

                int novaLarguraPainel =
                    Math.Max(
                        1,
                        (int)Math.Round(
                            PAINEL_LARGURA_REFERENCIA *
                            escala));

                int novaAlturaPainel =
                    Math.Max(
                        1,
                        (int)Math.Round(
                            PAINEL_ALTURA_REFERENCIA *
                            escala));

                pnlLogin.Size = new Size(
                    novaLarguraPainel,
                    novaAlturaPainel);

                pnlLogin.Location = new Point(
                    Math.Max(
                        0,
                        (pnlPrincipal.ClientSize.Width -
                         pnlLogin.Width) / 2),

                    Math.Max(
                        0,
                        (pnlPrincipal.ClientSize.Height -
                         pnlLogin.Height) / 2)
                );

                foreach (
                    ControleEscalavel item
                    in controlesEscalaveis)
                {
                    Control controle =
                        item.Controle;

                    Rectangle baseBounds =
                        item.BoundsBase;

                    controle.Bounds = new Rectangle(
                        (int)Math.Round(
                            baseBounds.X * escala),

                        (int)Math.Round(
                            baseBounds.Y * escala),

                        Math.Max(
                            1,
                            (int)Math.Round(
                                baseBounds.Width *
                                escala)),

                        Math.Max(
                            1,
                            (int)Math.Round(
                                baseBounds.Height *
                                escala))
                    );

                    if (item.TamanhoFonteBase > 0)
                    {
                        float novoTamanhoFonte =
                            Math.Max(
                                1f,
                                item.TamanhoFonteBase *
                                escala);

                        controle.Font =
                            new Font(
                                controle.Font.FontFamily,
                                novoTamanhoFonte,
                                controle.Font.Style);
                    }
                }
            }
            finally
            {
                aplicandoEscala = false;
            }
        }

        private void RegistrarControle(
            Control controle,
            Rectangle boundsBase,
            float tamanhoFonteBase)
        {
            controlesEscalaveis.Add(
                new ControleEscalavel(
                    controle,
                    boundsBase,
                    tamanhoFonteBase));
        }

        private void CriarLogo()
        {
            var lblLogo = new Label
            {
                Text = "INFINITYPART",
                AutoSize = false,
                Bounds = new Rectangle(
                    0,
                    0,
                    450,
                    62),
                ForeColor = AppTheme.TextPrimary,
                Font = new Font(
                    AppTheme.FontFamily,
                    28f,
                    FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                UseCompatibleTextRendering = true
            };

            pnlLogin.Controls.Add(lblLogo);

            RegistrarControle(
                lblLogo,
                new Rectangle(
                    0,
                    0,
                    450,
                    62),
                28f);

            var linha = new Panel
            {
                Bounds = new Rectangle(
                    197,
                    68,
                    55,
                    3),
                BackColor = AppTheme.PrimaryRed
            };

            pnlLogin.Controls.Add(linha);

            RegistrarControle(
                linha,
                new Rectangle(
                    197,
                    68,
                    55,
                    3),
                0);
        }

        private void CriarTitulo()
        {
            var lblTitulo = new Label
            {
                Text = "Bem-vindo",
                AutoSize = false,
                Bounds = new Rectangle(
                    0,
                    105,
                    450,
                    52),
                ForeColor = AppTheme.TextPrimary,
                Font = new Font(
                    AppTheme.FontFamily,
                    22f,
                    FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                UseCompatibleTextRendering = true
            };

            pnlLogin.Controls.Add(lblTitulo);

            RegistrarControle(
                lblTitulo,
                new Rectangle(
                    0,
                    105,
                    450,
                    52),
                22f);

            var lblSubtitulo = new Label
            {
                Text =
                    "Entre com suas credenciais de administrador.",
                AutoSize = false,
                Bounds = new Rectangle(
                    0,
                    157,
                    450,
                    48),
                ForeColor = AppTheme.TextSecondary,
                Font = AppTheme.FontSubtitle,
                TextAlign = ContentAlignment.MiddleCenter,
                UseCompatibleTextRendering = true
            };

            pnlLogin.Controls.Add(lblSubtitulo);

            RegistrarControle(
                lblSubtitulo,
                new Rectangle(
                    0,
                    157,
                    450,
                    48),
                AppTheme.FontSubtitle.Size);
        }

        private void CriarCampoUsuario()
        {
            var lblUsuario = CriarLabel(
                "Usuário, e-mail ou CPF",
                new Rectangle(
                    25,
                    220,
                    400,
                    24));

            pnlLogin.Controls.Add(lblUsuario);

            RegistrarControle(
                lblUsuario,
                new Rectangle(
                    25,
                    220,
                    400,
                    24),
                AppTheme.FontBodyBold.Size);

            txtIdentificador = CriarTextBox(
                "Digite seu usuário, e-mail ou CPF");

            txtIdentificador.Bounds =
                new Rectangle(
                    25,
                    253,
                    400,
                    40);

            pnlLogin.Controls.Add(
                txtIdentificador);

            RegistrarControle(
                txtIdentificador,
                new Rectangle(
                    25,
                    253,
                    400,
                    40),
                10f);
        }

        private void CriarCampoSenha()
        {
            var lblSenha = CriarLabel(
                "Senha",
                new Rectangle(
                    25,
                    318,
                    400,
                    24));

            pnlLogin.Controls.Add(lblSenha);

            RegistrarControle(
                lblSenha,
                new Rectangle(
                    25,
                    318,
                    400,
                    24),
                AppTheme.FontBodyBold.Size);

            pnlSenha = new Panel
            {
                Bounds = new Rectangle(
                    25,
                    351,
                    400,
                    40),
                BackColor = AppTheme.SurfaceAlt
            };

            pnlLogin.Controls.Add(pnlSenha);

            RegistrarControle(
                pnlSenha,
                new Rectangle(
                    25,
                    351,
                    400,
                    40),
                0);

            txtSenha = new TextBox
            {
                Bounds = new Rectangle(
                    0,
                    0,
                    355,
                    40),
                BackColor = AppTheme.SurfaceAlt,
                ForeColor = AppTheme.TextPrimary,
                BorderStyle = BorderStyle.None,
                Font = new Font(
                    AppTheme.FontFamily,
                    10f),
                PlaceholderText = "Digite sua senha",
                UseSystemPasswordChar = true
            };

            pnlSenha.Controls.Add(txtSenha);

            RegistrarControle(
                txtSenha,
                new Rectangle(
                    0,
                    0,
                    355,
                    40),
                10f);

            btnMostrarSenha = new Button
            {
                Text = "👁",
                Bounds = new Rectangle(
                    355,
                    3,
                    40,
                    34),
                BackColor = AppTheme.SurfaceAlt,
                ForeColor = AppTheme.TextSecondary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI Emoji",
                    10f),
                Cursor = Cursors.Hand,
                TabStop = false
            };

            btnMostrarSenha.FlatAppearance.BorderSize = 0;

            btnMostrarSenha.Click +=
                BtnMostrarSenha_Click;

            pnlSenha.Controls.Add(
                btnMostrarSenha);

            RegistrarControle(
                btnMostrarSenha,
                new Rectangle(
                    355,
                    3,
                    40,
                    34),
                10f);
        }

        private void CriarMostrarSenha()
        {
            chkMostrarSenha = new CheckBox
            {
                Text = "Mostrar senha",
                AutoSize = false,
                Bounds = new Rectangle(
                    25,
                    405,
                    180,
                    25),
                ForeColor = AppTheme.TextSecondary,
                Font = AppTheme.FontSmall,
                Cursor = Cursors.Hand
            };

            chkMostrarSenha.CheckedChanged +=
                ChkMostrarSenha_CheckedChanged;

            pnlLogin.Controls.Add(
                chkMostrarSenha);

            RegistrarControle(
                chkMostrarSenha,
                new Rectangle(
                    25,
                    405,
                    180,
                    25),
                AppTheme.FontSmall.Size);
        }

        private void CriarBotaoEntrar()
        {
            btnEntrar = new Button
            {
                Text = "ENTRAR",
                Bounds = new Rectangle(
                    25,
                    465,
                    400,
                    46),
                BackColor = AppTheme.PrimaryRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    AppTheme.FontFamily,
                    10f,
                    FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnEntrar.FlatAppearance.BorderSize = 0;

            btnEntrar.MouseEnter += (_, _) =>
            {
                if (!carregando)
                {
                    btnEntrar.BackColor =
                        AppTheme.PrimaryRedHover;
                }
            };

            btnEntrar.MouseLeave += (_, _) =>
            {
                if (!carregando)
                {
                    btnEntrar.BackColor =
                        AppTheme.PrimaryRed;
                }
            };

            btnEntrar.Click += BtnEntrar_Click;

            pnlLogin.Controls.Add(
                btnEntrar);

            RegistrarControle(
                btnEntrar,
                new Rectangle(
                    25,
                    465,
                    400,
                    46),
                10f);

            AcceptButton = btnEntrar;
        }

        private void CriarStatus()
        {
            lblStatus = new Label
            {
                Text = "",
                AutoSize = false,
                Bounds = new Rectangle(
                    25,
                    525,
                    400,
                    55),
                ForeColor = AppTheme.TextSecondary,
                Font = AppTheme.FontSmall,
                TextAlign = ContentAlignment.TopLeft,
                UseCompatibleTextRendering = true
            };

            pnlLogin.Controls.Add(
                lblStatus);

            RegistrarControle(
                lblStatus,
                new Rectangle(
                    25,
                    525,
                    400,
                    55),
                AppTheme.FontSmall.Size);
        }

        private Label CriarLabel(
            string texto,
            Rectangle bounds)
        {
            return new Label
            {
                Text = texto,
                AutoSize = false,
                Bounds = bounds,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontBodyBold,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = Padding.Empty,
                UseCompatibleTextRendering = true
            };
        }

        private TextBox CriarTextBox(
            string placeholder)
        {
            return new TextBox
            {
                BackColor = AppTheme.SurfaceAlt,
                ForeColor = AppTheme.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font(
                    AppTheme.FontFamily,
                    10f),
                PlaceholderText = placeholder,
                Margin = Padding.Empty
            };
        }

        private void BtnMostrarSenha_Click(
            object? sender,
            EventArgs e)
        {
            chkMostrarSenha.Checked =
                !chkMostrarSenha.Checked;
        }

        private void ChkMostrarSenha_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            txtSenha.UseSystemPasswordChar =
                !chkMostrarSenha.Checked;
        }

        private async void BtnEntrar_Click(
            object? sender,
            EventArgs e)
        {
            await FazerLoginAsync();
        }

        private async Task FazerLoginAsync()
        {
            if (carregando)
                return;

            string identificador =
                txtIdentificador.Text.Trim();

            string senha =
                txtSenha.Text;

            if (string.IsNullOrWhiteSpace(
                identificador))
            {
                MostrarStatus(
                    "Digite seu usuário, e-mail ou CPF.",
                    AppTheme.Warning);

                txtIdentificador.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(
                senha))
            {
                MostrarStatus(
                    "Digite sua senha.",
                    AppTheme.Warning);

                txtSenha.Focus();
                return;
            }

            try
            {
                DefinirCarregando(true);

                MostrarStatus(
                    "Conectando à API...",
                    AppTheme.TextSecondary);

                LoginResultadoModel resultado =
                    await AutenticacaoApiService.LoginAsync(
                        identificador,
                        senha);

                if (!resultado.Autenticado ||
                    resultado.Administrador == null)
                {
                    MostrarStatus(
                        string.IsNullOrWhiteSpace(
                            resultado.Mensagem)
                            ? "Usuário ou senha inválidos."
                            : resultado.Mensagem,
                        AppTheme.Danger);

                    txtSenha.Clear();
                    txtSenha.Focus();

                    return;
                }

                MostrarStatus(
                    "Login realizado com sucesso!",
                    AppTheme.Success);

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (HttpRequestException)
            {
                MostrarStatus(
                    "Não foi possível conectar à API." +
                    Environment.NewLine +
                    "Verifique se o InfinityPart.API está em execução.",
                    AppTheme.Danger);
            }
            catch (TaskCanceledException)
            {
                MostrarStatus(
                    "A API demorou muito para responder.",
                    AppTheme.Warning);
            }
            catch (Exception ex)
            {
                MostrarStatus(
                    "Erro ao realizar login: " +
                    ex.Message,
                    AppTheme.Danger);
            }
            finally
            {
                DefinirCarregando(false);
            }
        }

        private void DefinirCarregando(
            bool estado)
        {
            carregando = estado;

            txtIdentificador.Enabled = !estado;
            txtSenha.Enabled = !estado;
            chkMostrarSenha.Enabled = !estado;
            btnMostrarSenha.Enabled = !estado;
            btnEntrar.Enabled = !estado;

            btnEntrar.Text =
                estado
                    ? "CONECTANDO..."
                    : "ENTRAR";

            btnEntrar.BackColor =
                estado
                    ? AppTheme.PrimaryRedDark
                    : AppTheme.PrimaryRed;
        }

        private void MostrarStatus(
            string mensagem,
            Color cor)
        {
            lblStatus.Text = mensagem;
            lblStatus.ForeColor = cor;
        }
    }
}