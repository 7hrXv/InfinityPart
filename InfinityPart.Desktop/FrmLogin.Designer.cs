using InfinityPart.Desktop.Controls;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer? components = null;

        // Card central
        private Panel pnlCard = null!;
        private Label lblLogo = null!;
        private Label lblLogoAccent = null!;
        private Label lblSubtitulo = null!;

        // Usuário / E-mail / CPF
        private Label lblIdentificador = null!;
        private Panel pnlIdentificador = null!;
        private TextBox txtIdentificador = null!;

        // Senha
        private Label lblSenha = null!;
        private Panel pnlSenha = null!;
        private TextBox txtSenha = null!;
        private CheckBox chkMostrarSenha = null!;

        // Ações e feedback
        private AppButton btnEntrar = null!;
        private Label lblErro = null!;
        private ProgressBar prgCarregando = null!;
        private Label lblCarregando = null!;
        private Label lblApi = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            SuspendLayout();

            // ===== Form =====
            AutoScaleMode = AutoScaleMode.Font;
            Text = "InfinityPart — Acesso ao sistema";
            BackColor = AppTheme.Background;
            ForeColor = AppTheme.TextPrimary;
            Font = AppTheme.FontBody;
            ClientSize = new Size(880, 560);
            MinimumSize = new Size(700, 520);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // ===== Card =====
            pnlCard = new Panel
            {
                Width = 420,
                Height = 430,
                BackColor = AppTheme.Surface
            };

            lblLogo = new Label
            {
                Text = "INFINITY",
                Font = new Font(AppTheme.FontFamily, 18f, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                AutoSize = true,
                Location = new Point(36, 36)
            };

            lblLogoAccent = new Label
            {
                Text = "PART",
                Font = new Font(AppTheme.FontFamily, 18f, FontStyle.Bold),
                ForeColor = AppTheme.PrimaryRed,
                AutoSize = true
            };

            lblSubtitulo = new Label
            {
                Text = "Entre com suas credenciais de administrador.",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextSecondary,
                AutoSize = true,
                Location = new Point(38, 76)
            };

            // ===== Campo: usuário / e-mail / CPF =====
            lblIdentificador = new Label
            {
                Text = "Usuário, e-mail ou CPF",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextSecondary,
                AutoSize = true,
                Location = new Point(38, 120)
            };

            pnlIdentificador = new Panel
            {
                Location = new Point(36, 142),
                Size = new Size(348, 38),
                BackColor = AppTheme.SurfaceAlt,
                Padding = new Padding(10, 9, 10, 9)
            };

            txtIdentificador = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = AppTheme.SurfaceAlt,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontBody,
                MaxLength = 150,
                TabIndex = 0
            };

            pnlIdentificador.Controls.Add(txtIdentificador);

            // ===== Campo: senha =====
            lblSenha = new Label
            {
                Text = "Senha",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextSecondary,
                AutoSize = true,
                Location = new Point(38, 194)
            };

            pnlSenha = new Panel
            {
                Location = new Point(36, 216),
                Size = new Size(348, 38),
                BackColor = AppTheme.SurfaceAlt,
                Padding = new Padding(10, 9, 10, 9)
            };

            txtSenha = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = AppTheme.SurfaceAlt,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontBody,
                UseSystemPasswordChar = true,
                MaxLength = 128,
                TabIndex = 1
            };

            pnlSenha.Controls.Add(txtSenha);

            chkMostrarSenha = new CheckBox
            {
                Text = "Mostrar senha",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextMuted,
                BackColor = AppTheme.Surface,
                AutoSize = true,
                Location = new Point(36, 260),
                TabIndex = 2
            };

            // ===== Botão Entrar =====
            btnEntrar = new AppButton
            {
                Text = "Entrar",
                Style = AppButtonStyle.Primary,
                Location = new Point(36, 292),
                Size = new Size(348, 42),
                TabIndex = 3
            };

            // ===== Indicador de carregamento =====
            prgCarregando = new ProgressBar
            {
                Location = new Point(36, 344),
                Size = new Size(348, 4),
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Visible = false
            };

            lblCarregando = new Label
            {
                Text = "Autenticando...",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextSecondary,
                AutoSize = true,
                Location = new Point(36, 354),
                Visible = false
            };

            // ===== Mensagem de erro =====
            lblErro = new Label
            {
                Text = string.Empty,
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.PrimaryRedHover,
                Location = new Point(36, 354),
                Size = new Size(348, 40),
                Visible = false
            };

            lblApi = new Label
            {
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextMuted,
                AutoSize = true,
                Location = new Point(38, 400)
            };

            pnlCard.Controls.Add(lblLogo);
            pnlCard.Controls.Add(lblLogoAccent);
            pnlCard.Controls.Add(lblSubtitulo);
            pnlCard.Controls.Add(lblIdentificador);
            pnlCard.Controls.Add(pnlIdentificador);
            pnlCard.Controls.Add(lblSenha);
            pnlCard.Controls.Add(pnlSenha);
            pnlCard.Controls.Add(chkMostrarSenha);
            pnlCard.Controls.Add(btnEntrar);
            pnlCard.Controls.Add(prgCarregando);
            pnlCard.Controls.Add(lblCarregando);
            pnlCard.Controls.Add(lblErro);
            pnlCard.Controls.Add(lblApi);

            Controls.Add(pnlCard);

            AcceptButton = btnEntrar;

            CentralizarCard();
            PosicionarLogo();

            ResumeLayout(false);
        }

        private void CentralizarCard()
        {
            pnlCard.Location = new Point(
                Math.Max(0, (ClientSize.Width - pnlCard.Width) / 2),
                Math.Max(0, (ClientSize.Height - pnlCard.Height) / 2));
        }

        private void PosicionarLogo()
        {
            lblLogoAccent.Location = new Point(lblLogo.Right - 4, lblLogo.Top);
        }
    }
}
