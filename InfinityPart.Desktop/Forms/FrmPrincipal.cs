using System.Drawing;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmPrincipal : Form
    {
        private const float LARGURA_REFERENCIA = 1200f;
        private const float ALTURA_REFERENCIA = 800f;

        private const float SIDEBAR_LARGURA_REFERENCIA = 235f;
        private const float HEADER_ALTURA_REFERENCIA = 75f;

        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlConteudo = null!;

        private Label lblTitulo = null!;
        private Label lblUsuario = null!;

        private readonly List<Button> botoesMenu = new();

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

        public FrmPrincipal()
        {
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            Text = "InfinityPart - Administração";

            StartPosition =
                FormStartPosition.CenterScreen;

            WindowState =
                FormWindowState.Maximized;

            FormBorderStyle =
                FormBorderStyle.Sizable;

            MaximizeBox = true;
            MinimizeBox = true;

            ClientSize =
                new Size(1200, 800);

            MinimumSize =
                new Size(1100, 700);

            AutoScaleMode =
                AutoScaleMode.Dpi;

            BackColor =
                AppTheme.Background;

            ForeColor =
                AppTheme.TextPrimary;

            Font =
                AppTheme.FontBody;

            CriarSidebar();

            CriarHeader();

            CriarConteudoInicial();

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
        // ESCALA RESPONSIVA
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

        private void RemoverRegistros(
            Control controle)
        {
            controlesEscalaveis.Remove(controle);

            foreach (Control filho in controle.Controls)
            {
                RemoverRegistros(filho);
            }
        }

        private void AplicarEscala()
        {
            if (aplicandoEscala)
                return;

            if (pnlSidebar == null ||
                pnlHeader == null ||
                pnlConteudo == null)
            {
                return;
            }

            if (ClientSize.Width <= 0 ||
                ClientSize.Height <= 0)
            {
                return;
            }

            aplicandoEscala = true;

            try
            {
                float escala =
                    ObterEscala();

                // -------------------------------------------------
                // SIDEBAR
                // -------------------------------------------------

                pnlSidebar.Width =
                    Math.Max(
                        1,
                        (int)Math.Round(
                            SIDEBAR_LARGURA_REFERENCIA *
                            escala));

                // -------------------------------------------------
                // HEADER
                // -------------------------------------------------

                pnlHeader.Height =
                    Math.Max(
                        1,
                        (int)Math.Round(
                            HEADER_ALTURA_REFERENCIA *
                            escala));

                // -------------------------------------------------
                // CONTROLES
                // -------------------------------------------------

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

                    Rectangle baseBounds =
                        info.BoundsBase;

                    controle.Bounds =
                        new Rectangle(
                            (int)Math.Round(
                                baseBounds.X *
                                escala),

                            (int)Math.Round(
                                baseBounds.Y *
                                escala),

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

                AtualizarPosicaoUsuario();
            }
            finally
            {
                aplicandoEscala = false;
            }
        }

        // =========================================================
        // SIDEBAR
        // =========================================================

        private void CriarSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,

                Width =
                    (int)SIDEBAR_LARGURA_REFERENCIA,

                BackColor =
                    AppTheme.Sidebar
            };

            Controls.Add(
                pnlSidebar);

            var lblLogo = new Label
            {
                Text =
                    "INFINITY" +
                    Environment.NewLine +
                    "PART",

                AutoSize = false,

                TextAlign =
                    ContentAlignment.MiddleCenter,

                Bounds =
                    new Rectangle(
                        0,
                        0,
                        235,
                        70),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    new Font(
                        AppTheme.FontFamily,
                        17f,
                        FontStyle.Bold),

                UseCompatibleTextRendering = true
            };

            pnlSidebar.Controls.Add(
                lblLogo);

            RegistrarControle(
                lblLogo,
                new Rectangle(
                    0,
                    0,
                    235,
                    70),
                17f);

            var linha = new Panel
            {
                Bounds =
                    new Rectangle(
                        90,
                        62,
                        55,
                        3),

                BackColor =
                    AppTheme.PrimaryRed
            };

            pnlSidebar.Controls.Add(
                linha);

            RegistrarControle(
                linha,
                new Rectangle(
                    90,
                    62,
                    55,
                    3),
                0);

            int y = 105;

            CriarBotaoMenu(
                "▣   Dashboard",
                "dashboard",
                ref y);

            CriarBotaoMenu(
                "▣   Produtos",
                "produtos",
                ref y);

            CriarBotaoMenu(
                "▣   Categorias",
                "categorias",
                ref y);

            CriarBotaoMenu(
                "▣   Marcas",
                "marcas",
                ref y);

            CriarBotaoMenu(
                "▣   Clientes",
                "clientes",
                ref y);

            CriarBotaoMenu(
                "▣   Pedidos",
                "pedidos",
                ref y);

            // Nome visual do módulo:
            // Administração
            //
            // O identificador interno continua sendo
            // "administradores" para não quebrar a navegação.

            CriarBotaoMenu(
                "▣   Administração",
                "administradores",
                ref y);

            var separador = new Panel
            {
                Bounds =
                    new Rectangle(
                        20,
                        y + 15,
                        195,
                        1),

                BackColor =
                    AppTheme.Border
            };

            pnlSidebar.Controls.Add(
                separador);

            RegistrarControle(
                separador,
                new Rectangle(
                    20,
                    y + 15,
                    195,
                    1),
                0);

            y += 35;

            CriarBotaoMenu(
                "⚙   Configurações",
                "configuracoes",
                ref y);

            CriarBotaoMenu(
                "↪   Sair",
                "sair",
                ref y);
        }

        private void CriarBotaoMenu(
            string texto,
            string identificador,
            ref int y)
        {
            var botao = new Button
            {
                Text = texto,

                Tag = identificador,

                Bounds =
                    new Rectangle(
                        10,
                        y,
                        215,
                        45),

                BackColor =
                    AppTheme.Sidebar,

                ForeColor =
                    AppTheme.TextSecondary,

                FlatStyle =
                    FlatStyle.Flat,

                Font =
                    AppTheme.FontNav,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                Padding =
                    new Padding(
                        15,
                        0,
                        0,
                        0),

                Cursor =
                    Cursors.Hand
            };

            botao.FlatAppearance.BorderSize = 0;

            botao.MouseEnter += (_, _) =>
            {
                if (botao.Tag?.ToString() != "sair")
                {
                    botao.BackColor =
                        AppTheme.SurfaceAlt;
                }
            };

            botao.MouseLeave += (_, _) =>
            {
                if (botao.Tag?.ToString() != "sair")
                {
                    botao.BackColor =
                        AppTheme.Sidebar;
                }
            };

            botao.Click += Menu_Click;

            pnlSidebar.Controls.Add(
                botao);

            botoesMenu.Add(botao);

            RegistrarControle(
                botao,
                new Rectangle(
                    10,
                    y,
                    215,
                    45),
                AppTheme.FontNav.Size);

            y += 48;
        }

        // =========================================================
        // HEADER
        // =========================================================

        private void CriarHeader()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,

                Height =
                    (int)HEADER_ALTURA_REFERENCIA,

                BackColor =
                    AppTheme.Surface
            };

            Controls.Add(pnlHeader);

            pnlHeader.BringToFront();

            lblTitulo = new Label
            {
                Text = "Dashboard",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        30,
                        15,
                        500,
                        45),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontTitle,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            pnlHeader.Controls.Add(
                lblTitulo);

            RegistrarControle(
                lblTitulo,
                new Rectangle(
                    30,
                    15,
                    500,
                    45),
                AppTheme.FontTitle.Size);

            var linha = new Panel
            {
                BackColor =
                    AppTheme.Border,

                Height = 1,

                Dock =
                    DockStyle.Bottom
            };

            pnlHeader.Controls.Add(
                linha);

            string nomeUsuario =
                SessaoAtual.NomeExibicao;

            lblUsuario = new Label
            {
                Text =
                    "Administrador  •  " +
                    nomeUsuario,

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        700,
                        20,
                        470,
                        35),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.MiddleRight,

                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Right,

                UseCompatibleTextRendering = true
            };

            pnlHeader.Controls.Add(
                lblUsuario);

            RegistrarControle(
                lblUsuario,
                new Rectangle(
                    700,
                    20,
                    470,
                    35),
                AppTheme.FontSmall.Size);
        }

        private void AtualizarPosicaoUsuario()
        {
            if (lblUsuario == null ||
                pnlHeader == null)
            {
                return;
            }

            float escala =
                ObterEscala();

            int largura =
                (int)Math.Round(
                    470 * escala);

            int altura =
                (int)Math.Round(
                    35 * escala);

            int margemDireita =
                (int)Math.Round(
                    30 * escala);

            lblUsuario.Size =
                new Size(
                    largura,
                    altura);

            lblUsuario.Location =
                new Point(
                    pnlHeader.ClientSize.Width -
                    largura -
                    margemDireita,

                    (int)Math.Round(
                        20 * escala));
        }

        // =========================================================
        // CONTEÚDO
        // =========================================================

        private void CriarConteudoInicial()
        {
            pnlConteudo = new Panel
            {
                Dock = DockStyle.Fill,

                BackColor =
                    AppTheme.Background,

                AutoScroll = true
            };

            Controls.Add(
                pnlConteudo);

            pnlConteudo.BringToFront();

            CriarDashboard();
        }

        private void CriarDashboard()
        {
            LimparConteudo();

            lblTitulo.Text =
                "Dashboard";

            var titulo = CriarLabelConteudo(
                "Visão geral",
                new Rectangle(
                    30,
                    30,
                    500,
                    40),
                AppTheme.FontHeading);

            pnlConteudo.Controls.Add(
                titulo);

            var subtitulo = CriarLabelConteudo(
                "Acompanhe os principais indicadores da InfinityPart.",
                new Rectangle(
                    30,
                    68,
                    800,
                    30),
                AppTheme.FontSubtitle);

            pnlConteudo.Controls.Add(
                subtitulo);

            CriarCard(
                "PRODUTOS",
                "Gerenciar catálogo",
                "📦",
                30,
                115,
                "produtos");

            CriarCard(
                "CLIENTES",
                "Gerenciar clientes",
                "👥",
                280,
                115,
                "clientes");

            CriarCard(
                "PEDIDOS",
                "Acompanhar pedidos",
                "🛒",
                530,
                115,
                "pedidos");

            CriarCard(
                "MARCAS",
                "Gerenciar marcas",
                "🏷",
                780,
                115,
                "marcas");

            CriarPainelAtalhos();

            AplicarEscala();
        }

        private Label CriarLabelConteudo(
            string texto,
            Rectangle bounds,
            Font fonte)
        {
            var label = new Label
            {
                Text = texto,

                AutoSize = false,

                Bounds = bounds,

                ForeColor =
                    AppTheme.TextPrimary,

                Font = fonte,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            RegistrarControle(
                label,
                bounds,
                fonte.Size);

            return label;
        }

        private void CriarCard(
            string titulo,
            string descricao,
            string icone,
            int x,
            int y,
            string destino)
        {
            var card = new Panel
            {
                Bounds =
                    new Rectangle(
                        x,
                        y,
                        220,
                        145),

                BackColor =
                    AppTheme.Surface,

                Cursor =
                    Cursors.Hand
            };

            pnlConteudo.Controls.Add(card);

            RegistrarControle(
                card,
                new Rectangle(
                    x,
                    y,
                    220,
                    145),
                0);

            var faixa = new Panel
            {
                Bounds =
                    new Rectangle(
                        0,
                        0,
                        5,
                        145),

                BackColor =
                    AppTheme.PrimaryRed
            };

            card.Controls.Add(faixa);

            RegistrarControle(
                faixa,
                new Rectangle(
                    0,
                    0,
                    5,
                    145),
                0);

            var lblIcone = new Label
            {
                Text = icone,

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        22,
                        15,
                        170,
                        40),

                Font =
                    new Font(
                        "Segoe UI Emoji",
                        22f),

                ForeColor =
                    AppTheme.TextPrimary,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            card.Controls.Add(
                lblIcone);

            RegistrarControle(
                lblIcone,
                new Rectangle(
                    22,
                    15,
                    170,
                    40),
                22f);

            var lblTituloCard = new Label
            {
                Text = titulo,

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        22,
                        62,
                        180,
                        28),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontBodyBold,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            card.Controls.Add(
                lblTituloCard);

            RegistrarControle(
                lblTituloCard,
                new Rectangle(
                    22,
                    62,
                    180,
                    28),
                AppTheme.FontBodyBold.Size);

            // -------------------------------------------------
            // DESCRIÇÃO DO CARD
            // -------------------------------------------------
            // Aumentamos a altura para textos como
            // "Acompanhar pedidos" não serem cortados.

            var lblDescricao = new Label
            {
                Text = descricao,

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        22,
                        90,
                        180,
                        45),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.TopLeft,

                UseCompatibleTextRendering = true
            };

            card.Controls.Add(
                lblDescricao);

            RegistrarControle(
                lblDescricao,
                new Rectangle(
                    22,
                    90,
                    180,
                    45),
                AppTheme.FontSmall.Size);

            EventHandler abrir =
                (_, _) =>
                {
                    AbrirModulo(destino);
                };

            card.Click += abrir;

            lblIcone.Click += abrir;

            lblTituloCard.Click += abrir;

            lblDescricao.Click += abrir;

            card.MouseEnter += (_, _) =>
            {
                card.BackColor =
                    AppTheme.SurfaceAlt;
            };

            card.MouseLeave += (_, _) =>
            {
                card.BackColor =
                    AppTheme.Surface;
            };
        }

        // =========================================================
        // ATALHOS
        // =========================================================

        private void CriarPainelAtalhos()
        {
            var painel = new Panel
            {
                Bounds =
                    new Rectangle(
                        30,
                        310,
                        970,
                        230),

                BackColor =
                    AppTheme.Surface
            };

            pnlConteudo.Controls.Add(
                painel);

            RegistrarControle(
                painel,
                new Rectangle(
                    30,
                    310,
                    970,
                    230),
                0);

            var titulo = new Label
            {
                Text = "Acesso rápido",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        25,
                        20,
                        500,
                        35),

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontHeading,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                titulo);

            RegistrarControle(
                titulo,
                new Rectangle(
                    25,
                    20,
                    500,
                    35),
                AppTheme.FontHeading.Size);

            var descricao = new Label
            {
                Text =
                    "Use os atalhos abaixo para acessar as áreas administrativas.",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        25,
                        55,
                        850,
                        30),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSubtitle,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                descricao);

            RegistrarControle(
                descricao,
                new Rectangle(
                    25,
                    55,
                    850,
                    30),
                AppTheme.FontSubtitle.Size);

            CriarBotaoAtalho(
                painel,
                "Novo produto",
                25,
                100,
                "produtos");

            CriarBotaoAtalho(
                painel,
                "Ver clientes",
                205,
                100,
                "clientes");

            CriarBotaoAtalho(
                painel,
                "Ver pedidos",
                385,
                100,
                "pedidos");

            CriarBotaoAtalho(
                painel,
                "Gerenciar marcas",
                565,
                100,
                "marcas");
        }

        private void CriarBotaoAtalho(
            Panel painel,
            string texto,
            int x,
            int y,
            string destino)
        {
            var botao = new Button
            {
                Text = texto,

                Bounds =
                    new Rectangle(
                        x,
                        y,
                        160,
                        45),

                BackColor =
                    AppTheme.PrimaryRed,

                ForeColor =
                    Color.White,

                FlatStyle =
                    FlatStyle.Flat,

                Font =
                    AppTheme.FontBodyBold,

                Cursor =
                    Cursors.Hand
            };

            botao.FlatAppearance.BorderSize = 0;

            botao.MouseEnter += (_, _) =>
            {
                botao.BackColor =
                    AppTheme.PrimaryRedHover;
            };

            botao.MouseLeave += (_, _) =>
            {
                botao.BackColor =
                    AppTheme.PrimaryRed;
            };

            botao.Click += (_, _) =>
            {
                AbrirModulo(destino);
            };

            painel.Controls.Add(botao);

            RegistrarControle(
                botao,
                new Rectangle(
                    x,
                    y,
                    160,
                    45),
                AppTheme.FontBodyBold.Size);
        }

        // =========================================================
        // LIMPEZA DO CONTEÚDO
        // =========================================================

        private void LimparConteudo()
        {
            if (pnlConteudo == null)
                return;

            foreach (Control controle
                in pnlConteudo.Controls)
            {
                RemoverRegistros(controle);
            }

            pnlConteudo.Controls.Clear();
        }

        // =========================================================
        // MENU
        // =========================================================

        private void Menu_Click(
            object? sender,
            EventArgs e)
        {
            if (sender is not Button botao)
                return;

            string destino =
                botao.Tag?.ToString() ?? "";

            if (destino == "sair")
            {
                Sair();
                return;
            }

            AbrirModulo(destino);
        }

        private void AbrirModulo(
            string identificador)
        {
            switch (identificador)
            {
                case "dashboard":

                    CriarDashboard();

                    break;

                case "produtos":

                    using (var frmProdutos =
                        new FrmProdutos())
                    {
                        frmProdutos.ShowDialog(this);
                    }

                    break;

                case "categorias":

                    using (var frmCategorias =
                        new FrmCategorias())
                    {
                        frmCategorias.ShowDialog(this);
                    }

                    break;

                case "marcas":

                    using (var frmMarcas =
                        new FrmMarcas())
                    {
                        frmMarcas.ShowDialog(this);
                    }

                    break;

                case "clientes":

                    using (var frmClientes =
                        new FrmClientes())
                    {
                        frmClientes.ShowDialog(this);
                    }

                    break;

                case "pedidos":

                    using (var frmPedidos =
                        new FrmPedidos())
                    {
                        frmPedidos.ShowDialog(this);
                    }

                    break;

                case "administradores":

                    using (var frmAdministradores =
                        new FrmAdministradores())
                    {
                        frmAdministradores.ShowDialog(this);
                    }

                    break;

                case "configuracoes":

                    using (var frmConfiguracoes =
                        new FrmConfiguracoes())
                    {
                        frmConfiguracoes.ShowDialog(this);
                    }

                    break;

                case "sair":

                    Sair();

                    break;

                default:

                    CriarDashboard();

                    break;
            }
        }

        // =========================================================
        // MÓDULOS
        // =========================================================

        private void MostrarModuloIndisponivel(
            string modulo)
        {
            LimparConteudo();

            lblTitulo.Text =
                modulo;

            var titulo = CriarLabelConteudo(
                modulo,
                new Rectangle(
                    30,
                    30,
                    700,
                    45),
                AppTheme.FontTitle);

            pnlConteudo.Controls.Add(
                titulo);

            var painel = new Panel
            {
                Bounds =
                    new Rectangle(
                        30,
                        90,
                        700,
                        180),

                BackColor =
                    AppTheme.Surface
            };

            pnlConteudo.Controls.Add(
                painel);

            RegistrarControle(
                painel,
                new Rectangle(
                    30,
                    90,
                    700,
                    180),
                0);

            var icone = new Label
            {
                Text = "◉",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        30,
                        35,
                        50,
                        55),

                ForeColor =
                    AppTheme.PrimaryRed,

                Font =
                    new Font(
                        AppTheme.FontFamily,
                        30f,
                        FontStyle.Bold),

                TextAlign =
                    ContentAlignment.MiddleCenter,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                icone);

            RegistrarControle(
                icone,
                new Rectangle(
                    30,
                    35,
                    50,
                    55),
                30f);

            var texto = new Label
            {
                Text =
                    "Módulo " +
                    modulo +
                    Environment.NewLine +
                    Environment.NewLine +
                    "A interface deste módulo será conectada à API nas próximas etapas.",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        90,
                        35,
                        560,
                        100),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontBody,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                UseCompatibleTextRendering = true
            };

            painel.Controls.Add(
                texto);

            RegistrarControle(
                texto,
                new Rectangle(
                    90,
                    35,
                    560,
                    100),
                AppTheme.FontBody.Size);

            AplicarEscala();
        }

        // =========================================================
        // SAIR
        // =========================================================

        private void Sair()
        {
            var resposta =
                MessageBox.Show(
                    "Deseja realmente sair do sistema?",
                    "Sair",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (resposta !=
                DialogResult.Yes)
            {
                return;
            }

            SessaoAtual.Encerrar();

            Close();
        }
    }
}