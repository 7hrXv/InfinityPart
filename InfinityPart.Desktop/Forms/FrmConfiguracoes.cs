using System.Drawing;
using System.Net.Http;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmConfiguracoes : Form
    {
        private const float LARGURA_REFERENCIA = 900f;
        private const float ALTURA_REFERENCIA = 700f;

        private Panel pnlPrincipal = null!;

        private Label lblTitulo = null!;
        private Label lblSubtitulo = null!;

        private Label lblAdministrador = null!;
        private Label lblApi = null!;
        private Label lblStatus = null!;

        private Button btnTestarApi = null!;
        private Button btnFechar = null!;

        private readonly HttpClient httpClient = new()
        {
            BaseAddress =
                new Uri("http://localhost:5022/api/")
        };

        private readonly Dictionary<Control, InformacaoLayout>
            controlesEscalaveis = new();

        private bool aplicandoEscala;

        private class InformacaoLayout
        {
            public Rectangle BoundsBase { get; }

            public float TamanhoFonteBase { get; }

            public InformacaoLayout(
                Rectangle boundsBase,
                float tamanhoFonteBase)
            {
                BoundsBase = boundsBase;
                TamanhoFonteBase = tamanhoFonteBase;
            }
        }

        public FrmConfiguracoes()
        {
            InicializarFormulario();
        }

        // =========================================================
        // FORMULÁRIO
        // =========================================================

        private void InicializarFormulario()
        {
            Text =
                "InfinityPart - Configurações";

            StartPosition =
                FormStartPosition.CenterParent;

            FormBorderStyle =
                FormBorderStyle.Sizable;

            MaximizeBox = true;
            MinimizeBox = true;

            ClientSize =
                new Size(
                    900,
                    700);

            MinimumSize =
                new Size(
                    760,
                    620);

            AutoScaleMode =
                AutoScaleMode.Dpi;

            BackColor =
                AppTheme.Background;

            ForeColor =
                AppTheme.TextPrimary;

            Font =
                AppTheme.FontBody;

            CriarInterface();

            Resize += (_, _) =>
            {
                AplicarEscala();
            };

            Shown += (_, _) =>
            {
                AplicarEscala();
            };
        }

        // =========================================================
        // ESCALA
        // =========================================================

        private float ObterEscala()
        {
            if (ClientSize.Width <= 0 ||
                ClientSize.Height <= 0)
            {
                return 1f;
            }

            float escalaX =
                ClientSize.Width /
                LARGURA_REFERENCIA;

            float escalaY =
                ClientSize.Height /
                ALTURA_REFERENCIA;

            float escala =
                Math.Min(
                    escalaX,
                    escalaY);

            return Math.Max(
                escala,
                0.80f);
        }

        private void RegistrarControle(
            Control controle,
            Rectangle boundsBase,
            float tamanhoFonteBase)
        {
            controlesEscalaveis[controle] =
                new InformacaoLayout(
                    boundsBase,
                    tamanhoFonteBase);
        }

        private void AplicarEscala()
        {
            if (aplicandoEscala)
                return;

            if (pnlPrincipal == null)
                return;

            if (ClientSize.Width <= 0 ||
                ClientSize.Height <= 0)
                return;

            aplicandoEscala = true;

            try
            {
                float escala =
                    ObterEscala();

                int larguraPainel =
                    (int)Math.Round(
                        780 * escala);

                int alturaPainel =
                    (int)Math.Round(
                        620 * escala);

                pnlPrincipal.Size =
                    new Size(
                        larguraPainel,
                        alturaPainel);

                pnlPrincipal.Location =
                    new Point(
                        Math.Max(
                            0,
                            (ClientSize.Width -
                             pnlPrincipal.Width) / 2),

                        Math.Max(
                            0,
                            (ClientSize.Height -
                             pnlPrincipal.Height) / 2));

                foreach (
                    KeyValuePair<
                        Control,
                        InformacaoLayout> item
                    in controlesEscalaveis)
                {
                    Control controle =
                        item.Key;

                    if (controle.IsDisposed)
                        continue;

                    InformacaoLayout info =
                        item.Value;

                    Rectangle bounds =
                        info.BoundsBase;

                    controle.Bounds =
                        new Rectangle(
                            (int)Math.Round(
                                bounds.X *
                                escala),

                            (int)Math.Round(
                                bounds.Y *
                                escala),

                            Math.Max(
                                1,
                                (int)Math.Round(
                                    bounds.Width *
                                    escala)),

                            Math.Max(
                                1,
                                (int)Math.Round(
                                    bounds.Height *
                                    escala)));

                    if (info.TamanhoFonteBase > 0)
                    {
                        float tamanhoFonte =
                            Math.Max(
                                1f,
                                info.TamanhoFonteBase *
                                escala);

                        controle.Font =
                            new Font(
                                controle.Font.FontFamily,
                                tamanhoFonte,
                                controle.Font.Style);
                    }
                }
            }
            finally
            {
                aplicandoEscala = false;
            }
        }

        // =========================================================
        // INTERFACE
        // =========================================================

        private void CriarInterface()
        {
            pnlPrincipal = new Panel
            {
                Size =
                    new Size(
                        780,
                        620),

                BackColor =
                    AppTheme.Surface,

                BorderStyle =
                    BorderStyle.FixedSingle
            };

            Controls.Add(
                pnlPrincipal);

            // =====================================================
            // TÍTULO
            // =====================================================

            lblTitulo = new Label
            {
                Text =
                    "Configurações",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        35,
                        25,
                        710,
                        50),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    new Font(
                        AppTheme.FontFamily,
                        22f,
                        FontStyle.Bold),

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            pnlPrincipal.Controls.Add(
                lblTitulo);

            RegistrarControle(
                lblTitulo,
                new Rectangle(
                    35,
                    25,
                    710,
                    50),
                22f);

            // =====================================================
            // SUBTÍTULO
            // =====================================================

            lblSubtitulo = new Label
            {
                Text =
                    "Informações do sistema e da sessão atual.",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        35,
                        75,
                        710,
                        35),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSubtitle,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            pnlPrincipal.Controls.Add(
                lblSubtitulo);

            RegistrarControle(
                lblSubtitulo,
                new Rectangle(
                    35,
                    75,
                    710,
                    35),
                AppTheme.FontSubtitle.Size);

            // =====================================================
            // PAINEL SISTEMA
            // =====================================================

            CriarPainelSistema();

            // =====================================================
            // PAINEL SESSÃO
            // =====================================================

            CriarPainelSessao();

            // =====================================================
            // BOTÕES
            // =====================================================

            btnFechar = new Button
            {
                Text =
                    "Voltar",

                Bounds =
                    new Rectangle(
                        420,
                        535,
                        145,
                        45),

                BackColor =
                    AppTheme.SurfaceAlt,

                ForeColor =
                    AppTheme.TextPrimary,

                FlatStyle =
                    FlatStyle.Flat,

                Font =
                    AppTheme.FontBody,

                Cursor =
                    Cursors.Hand
            };

            btnFechar.FlatAppearance.BorderSize = 0;

            btnFechar.Click += (_, _) =>
            {
                DialogResult =
                    DialogResult.Cancel;

                Close();
            };

            pnlPrincipal.Controls.Add(
                btnFechar);

            RegistrarControle(
                btnFechar,
                new Rectangle(
                    420,
                    535,
                    145,
                    45),
                AppTheme.FontBody.Size);

            btnTestarApi = new Button
            {
                Text =
                    "Testar conexão",

                Bounds =
                    new Rectangle(
                        580,
                        535,
                        165,
                        45),

                BackColor =
                    AppTheme.PrimaryRed,

                ForeColor =
                    Color.White,

                FlatStyle =
                    FlatStyle.Flat,

                Font =
                    new Font(
                        AppTheme.FontFamily,
                        AppTheme.FontBody.Size,
                        FontStyle.Bold),

                Cursor =
                    Cursors.Hand
            };

            btnTestarApi.FlatAppearance.BorderSize = 0;

            btnTestarApi.MouseEnter += (_, _) =>
            {
                btnTestarApi.BackColor =
                    AppTheme.PrimaryRedHover;
            };

            btnTestarApi.MouseLeave += (_, _) =>
            {
                btnTestarApi.BackColor =
                    AppTheme.PrimaryRed;
            };

            btnTestarApi.Click += async (_, _) =>
            {
                await TestarConexaoAsync();
            };

            pnlPrincipal.Controls.Add(
                btnTestarApi);

            RegistrarControle(
                btnTestarApi,
                new Rectangle(
                    580,
                    535,
                    165,
                    45),
                AppTheme.FontBody.Size);
        }

        // =========================================================
        // PAINEL SISTEMA
        // =========================================================

        private void CriarPainelSistema()
        {
            var painel = new Panel
            {
                Bounds =
                    new Rectangle(
                        35,
                        130,
                        710,
                        175),

                BackColor =
                    AppTheme.Background
            };

            pnlPrincipal.Controls.Add(
                painel);

            RegistrarControle(
                painel,
                new Rectangle(
                    35,
                    130,
                    710,
                    175),
                0);

            var titulo = new Label
            {
                Text =
                    "Informações do sistema",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        20,
                        15,
                        400,
                        30),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontBodyBold,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                titulo);

            RegistrarControle(
                titulo,
                new Rectangle(
                    20,
                    15,
                    400,
                    30),
                AppTheme.FontBodyBold.Size);

            CriarInformacao(
                painel,
                "Aplicação",
                "InfinityPart",
                20,
                55);

            CriarInformacao(
                painel,
                "Módulo",
                "Desktop Administrativo",
                20,
                90);

            CriarInformacao(
                painel,
                "API",
                "http://localhost:5022/api/",
                20,
                125);
        }

        // =========================================================
        // PAINEL SESSÃO
        // =========================================================

        private void CriarPainelSessao()
        {
            var painel = new Panel
            {
                Bounds =
                    new Rectangle(
                        35,
                        325,
                        710,
                        175),

                BackColor =
                    AppTheme.Background
            };

            pnlPrincipal.Controls.Add(
                painel);

            RegistrarControle(
                painel,
                new Rectangle(
                    35,
                    325,
                    710,
                    175),
                0);

            var titulo = new Label
            {
                Text =
                    "Sessão atual",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        20,
                        15,
                        400,
                        30),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontBodyBold,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                titulo);

            RegistrarControle(
                titulo,
                new Rectangle(
                    20,
                    15,
                    400,
                    30),
                AppTheme.FontBodyBold.Size);

            lblAdministrador = new Label
            {
                Text =
                    "Administrador: " +
                    SessaoAtual.NomeExibicao,

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        20,
                        55,
                        650,
                        30),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontBody,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                lblAdministrador);

            RegistrarControle(
                lblAdministrador,
                new Rectangle(
                    20,
                    55,
                    650,
                    30),
                AppTheme.FontBody.Size);

            lblApi = new Label
            {
                Text =
                    "Servidor: API local",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        20,
                        90,
                        300,
                        30),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                lblApi);

            RegistrarControle(
                lblApi,
                new Rectangle(
                    20,
                    90,
                    300,
                    30),
                AppTheme.FontSmall.Size);

            lblStatus = new Label
            {
                Text =
                    "Status: não testado",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        20,
                        120,
                        400,
                        30),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                lblStatus);

            RegistrarControle(
                lblStatus,
                new Rectangle(
                    20,
                    120,
                    400,
                    30),
                AppTheme.FontSmall.Size);
        }

        // =========================================================
        // INFORMAÇÃO
        // =========================================================

        private void CriarInformacao(
            Panel painel,
            string titulo,
            string valor,
            int x,
            int y)
        {
            var lblTituloInformacao = new Label
            {
                Text =
                    titulo + ":",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        x,
                        y,
                        110,
                        30),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                lblTituloInformacao);

            RegistrarControle(
                lblTituloInformacao,
                new Rectangle(
                    x,
                    y,
                    110,
                    30),
                AppTheme.FontSmall.Size);

            var lblValor = new Label
            {
                Text =
                    valor,

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        x + 110,
                        y,
                        550,
                        30),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontBody,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                lblValor);

            RegistrarControle(
                lblValor,
                new Rectangle(
                    x + 110,
                    y,
                    550,
                    30),
                AppTheme.FontBody.Size);
        }

        // =========================================================
        // TESTAR API
        // =========================================================

        private async Task TestarConexaoAsync()
        {
            try
            {
                btnTestarApi.Enabled = false;

                lblStatus.Text =
                    "Status: testando conexão...";

                lblStatus.ForeColor =
                    AppTheme.TextSecondary;

                using HttpResponseMessage resposta =
                    await httpClient.GetAsync(
                        "Categoria");

                if (resposta.IsSuccessStatusCode)
                {
                    lblStatus.Text =
                        "Status: API conectada com sucesso.";

                    lblStatus.ForeColor =
                        Color.LightGreen;

                    MessageBox.Show(
                        "A conexão com a API foi realizada com sucesso.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text =
                        "Status: API respondeu com erro.";

                    lblStatus.ForeColor =
                        Color.Orange;

                    MessageBox.Show(
                        "A API respondeu, mas retornou um erro.\n\n" +
                        $"Status: {(int)resposta.StatusCode} " +
                        $"{resposta.StatusCode}",
                        "Conexão",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text =
                    "Status: API indisponível.";

                lblStatus.ForeColor =
                    Color.IndianRed;

                MessageBox.Show(
                    "Não foi possível conectar à API.\n\n" +
                    ex.Message,
                    "Erro de conexão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnTestarApi.Enabled = true;
            }
        }

        // =========================================================
        // FECHAMENTO
        // =========================================================

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}