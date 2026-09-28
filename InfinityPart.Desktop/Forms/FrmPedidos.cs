using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmPedidos : System.Windows.Forms.Form
    {
        private const float LARGURA_REFERENCIA = 1200f;
        private const float ALTURA_REFERENCIA = 800f;
        private const float MARGEM_REFERENCIA = 30f;

        private Panel pnlPrincipal = null!;
        private Panel pnlCabecalho = null!;
        private Panel pnlTabela = null!;

        private Label lblTitulo = null!;
        private Label lblQuantidade = null!;

        private TextBox txtPesquisa = null!;
        private Button btnPesquisar = null!;
        private Button btnNovoPedido = null!;

        private DataGridView dgvPedidos = null!;

        private readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri(
                "http://localhost:5022/api/")
        };

        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private bool aplicandoEscala;

        private List<PedidoModel> pedidos = new();

        // =========================================================
        // MODELO
        // =========================================================

        private class PedidoModel
        {
            public int Id { get; set; }

            public DateTime DataPedido { get; set; }

            public decimal ValorTotal { get; set; }

            public string Status { get; set; } = "";

            public string? ApplicationUserId { get; set; }
        }

        public FrmPedidos()
        {
            InicializarFormulario();
        }

        // =========================================================
        // FORMULÁRIO
        // =========================================================

        private void InicializarFormulario()
        {
            Text =
                "InfinityPart - Pedidos";

            StartPosition =
                FormStartPosition.CenterScreen;

            FormBorderStyle =
                FormBorderStyle.Sizable;

            MaximizeBox = true;
            MinimizeBox = true;

            ClientSize =
                new Size(
                    1200,
                    800);

            MinimumSize =
                new Size(
                    1000,
                    650);

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

            Shown += async (_, _) =>
            {
                AplicarEscala();

                await CarregarPedidosAsync();
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

        private void AplicarEscala()
        {
            if (aplicandoEscala)
                return;

            if (ClientSize.Width <= 0 ||
                ClientSize.Height <= 0)
                return;

            if (pnlPrincipal == null ||
                pnlCabecalho == null ||
                pnlTabela == null)
                return;

            aplicandoEscala = true;

            try
            {
                float escala =
                    ObterEscala();

                int margem =
                    Math.Max(
                        20,
                        (int)Math.Round(
                            MARGEM_REFERENCIA *
                            escala));

                // =================================================
                // CABEÇALHO
                // =================================================

                int larguraCabecalho =
                    Math.Max(
                        1,
                        ClientSize.Width -
                        margem * 2);

                int alturaCabecalho =
                    Math.Max(
                        1,
                        (int)Math.Round(
                            120 * escala));

                pnlCabecalho.Bounds =
                    new Rectangle(
                        margem,
                        Math.Max(
                            15,
                            (int)Math.Round(
                                25 * escala)),
                        larguraCabecalho,
                        alturaCabecalho);

                // =================================================
                // TABELA
                // =================================================

                int topoTabela =
                    pnlCabecalho.Bottom +
                    Math.Max(
                        10,
                        (int)Math.Round(
                            15 * escala));

                int alturaTabela =
                    Math.Max(
                        200,
                        ClientSize.Height -
                        topoTabela -
                        margem);

                pnlTabela.Bounds =
                    new Rectangle(
                        margem,
                        topoTabela,
                        larguraCabecalho,
                        alturaTabela);

                // =================================================
                // TÍTULO
                // =================================================

                int tituloLargura =
                    Math.Max(
                        300,
                        (int)Math.Round(
                            500 * escala));

                lblTitulo.Bounds =
                    new Rectangle(
                        0,
                        0,
                        tituloLargura,
                        Math.Max(
                            35,
                            (int)Math.Round(
                                45 * escala)));

                // =================================================
                // QUANTIDADE
                // =================================================

                lblQuantidade.Bounds =
                    new Rectangle(
                        0,
                        Math.Max(
                            40,
                            (int)Math.Round(
                                48 * escala)),
                        Math.Max(
                            300,
                            (int)Math.Round(
                                400 * escala)),
                        Math.Max(
                            25,
                            (int)Math.Round(
                                30 * escala)));

                // =================================================
                // BOTÕES
                // =================================================

                int alturaControles =
                    Math.Max(
                        35,
                        (int)Math.Round(
                            40 * escala));

                int espacamento =
                    Math.Max(
                        8,
                        (int)Math.Round(
                            10 * escala));

                int larguraNovo =
                    Math.Max(
                        160,
                        (int)Math.Round(
                            215 * escala));

                int larguraPesquisar =
                    Math.Max(
                        100,
                        (int)Math.Round(
                            120 * escala));

                int larguraDisponivel =
                    pnlCabecalho.ClientSize.Width;

                btnNovoPedido.Bounds =
                    new Rectangle(
                        larguraDisponivel -
                        larguraNovo,
                        Math.Max(
                            0,
                            (int)Math.Round(
                                5 * escala)),
                        larguraNovo,
                        alturaControles);

                btnPesquisar.Bounds =
                    new Rectangle(
                        btnNovoPedido.Left -
                        espacamento -
                        larguraPesquisar,
                        Math.Max(
                            0,
                            (int)Math.Round(
                                5 * escala)),
                        larguraPesquisar,
                        alturaControles);

                // =================================================
                // PESQUISA
                // =================================================

                int pesquisaEsquerda =
                    Math.Max(
                        (int)Math.Round(
                            480 * escala),
                        tituloLargura);

                int pesquisaDireita =
                    btnPesquisar.Left -
                    espacamento;

                int pesquisaLargura =
                    Math.Max(
                        180,
                        pesquisaDireita -
                        pesquisaEsquerda);

                if (pesquisaLargura < 180)
                {
                    pesquisaEsquerda =
                        Math.Max(
                            10,
                            btnPesquisar.Left -
                            espacamento -
                            180);

                    pesquisaLargura =
                        Math.Max(
                            180,
                            btnPesquisar.Left -
                            espacamento -
                            pesquisaEsquerda);
                }

                txtPesquisa.Bounds =
                    new Rectangle(
                        pesquisaEsquerda,
                        Math.Max(
                            0,
                            (int)Math.Round(
                                5 * escala)),
                        pesquisaLargura,
                        alturaControles);

                // =================================================
                // FONTES
                // =================================================

                lblTitulo.Font =
                    new Font(
                        AppTheme.FontFamily,
                        Math.Max(
                            1f,
                            AppTheme.FontTitle.Size *
                            escala),
                        AppTheme.FontTitle.Style);

                lblQuantidade.Font =
                    new Font(
                        AppTheme.FontFamily,
                        Math.Max(
                            1f,
                            AppTheme.FontSmall.Size *
                            escala),
                        AppTheme.FontSmall.Style);

                txtPesquisa.Font =
                    new Font(
                        AppTheme.FontFamily,
                        Math.Max(
                            9f,
                            AppTheme.FontBody.Size *
                            escala),
                        AppTheme.FontBody.Style);

                btnPesquisar.Font =
                    new Font(
                        AppTheme.FontFamily,
                        Math.Max(
                            9f,
                            AppTheme.FontBody.Size *
                            escala),
                        AppTheme.FontBody.Style);

                btnNovoPedido.Font =
                    new Font(
                        AppTheme.FontFamily,
                        Math.Max(
                            9f,
                            10f * escala),
                        FontStyle.Bold);

                AtualizarGrid();
                AjustarColunasGrid();
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
                Dock = DockStyle.Fill,

                BackColor =
                    AppTheme.Background
            };

            Controls.Add(
                pnlPrincipal);

            CriarCabecalho();
            CriarTabela();
        }

        // =========================================================
        // CABEÇALHO
        // =========================================================

        private void CriarCabecalho()
        {
            pnlCabecalho = new Panel
            {
                Bounds =
                    new Rectangle(
                        30,
                        25,
                        1140,
                        120),

                BackColor =
                    AppTheme.Background
            };

            pnlPrincipal.Controls.Add(
                pnlCabecalho);

            // =====================================================
            // TÍTULO
            // =====================================================

            lblTitulo = new Label
            {
                Text = "Pedidos",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        0,
                        0,
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

            pnlCabecalho.Controls.Add(
                lblTitulo);

            // =====================================================
            // QUANTIDADE
            // =====================================================

            lblQuantidade = new Label
            {
                Text =
                    "Carregando pedidos...",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        0,
                        48,
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

            pnlCabecalho.Controls.Add(
                lblQuantidade);

            // =====================================================
            // PESQUISA
            // =====================================================

            txtPesquisa = new TextBox
            {
                Bounds =
                    new Rectangle(
                        480,
                        5,
                        300,
                        40),

                BackColor =
                    AppTheme.Surface,

                ForeColor =
                    AppTheme.TextPrimary,

                BorderStyle =
                    BorderStyle.FixedSingle,

                Font =
                    AppTheme.FontBody,

                PlaceholderText =
                    "Pesquisar pedido ou status..."
            };

            txtPesquisa.TextChanged += (_, _) =>
            {
                FiltrarPedidos();
            };

            pnlCabecalho.Controls.Add(
                txtPesquisa);

            // =====================================================
            // PESQUISAR
            // =====================================================

            btnPesquisar = new Button
            {
                Text = "Pesquisar",

                Bounds =
                    new Rectangle(
                        790,
                        5,
                        120,
                        40),

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

            btnPesquisar.FlatAppearance.BorderColor =
                AppTheme.Border;

            btnPesquisar.FlatAppearance.BorderSize = 1;

            btnPesquisar.Click += (_, _) =>
            {
                FiltrarPedidos();
            };

            pnlCabecalho.Controls.Add(
                btnPesquisar);

            // =====================================================
            // NOVO PEDIDO
            // =====================================================

            btnNovoPedido = new Button
            {
                Text = "+  Novo Pedido",

                Bounds =
                    new Rectangle(
                        925,
                        5,
                        215,
                        40),

                BackColor =
                    AppTheme.PrimaryRed,

                ForeColor =
                    Color.White,

                FlatStyle =
                    FlatStyle.Flat,

                Font =
                    new Font(
                        AppTheme.FontFamily,
                        10f,
                        FontStyle.Bold),

                Cursor =
                    Cursors.Hand
            };

            btnNovoPedido.FlatAppearance.BorderSize = 0;

            btnNovoPedido.Click += async (_, _) =>
            {
                await AbrirCadastroPedidoAsync();
            };

            pnlCabecalho.Controls.Add(
                btnNovoPedido);
        }

        // =========================================================
        // TABELA
        // =========================================================

        private void CriarTabela()
        {
            pnlTabela = new Panel
            {
                Bounds =
                    new Rectangle(
                        30,
                        160,
                        1140,
                        590),

                BackColor =
                    AppTheme.Surface
            };

            pnlPrincipal.Controls.Add(
                pnlTabela);

            dgvPedidos = new DataGridView
            {
                Dock = DockStyle.Fill,

                BackgroundColor =
                    AppTheme.Surface,

                BorderStyle =
                    BorderStyle.None,

                CellBorderStyle =
                    DataGridViewCellBorderStyle.SingleHorizontal,

                GridColor =
                    AppTheme.Border,

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,

                ReadOnly = true,

                MultiSelect = false,

                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                AutoGenerateColumns = false,

                RowHeadersVisible = false,

                EnableHeadersVisualStyles = false,

                Font =
                    AppTheme.FontBody
            };

            dgvPedidos.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        AppTheme.Sidebar,

                    ForeColor =
                        AppTheme.TextPrimary,

                    Font =
                        new Font(
                            AppTheme.FontFamily,
                            9.5f,
                            FontStyle.Bold),

                    Alignment =
                        DataGridViewContentAlignment.MiddleLeft,

                    Padding =
                        new Padding(
                            10,
                            0,
                            10,
                            0)
                };

            dgvPedidos.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        AppTheme.Surface,

                    ForeColor =
                        AppTheme.TextPrimary,

                    SelectionBackColor =
                        AppTheme.SurfaceAlt,

                    SelectionForeColor =
                        AppTheme.TextPrimary,

                    Font =
                        AppTheme.FontBody,

                    Padding =
                        new Padding(
                            10,
                            0,
                            10,
                            0)
                };

            dgvPedidos.RowTemplate.Height = 48;

            CriarColunas();

            dgvPedidos.CellDoubleClick += (_, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    _ = AbrirEdicaoPedidoAsync();
                }
            };

            pnlTabela.Controls.Add(
                dgvPedidos);

            AjustarColunasGrid();
        }

        // =========================================================
        // COLUNAS
        // =========================================================

        private void CriarColunas()
        {
            dgvPedidos.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colId",

                    HeaderText = "ID",

                    DataPropertyName = "Id"
                });

            dgvPedidos.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colData",

                    HeaderText = "Data",

                    DataPropertyName =
                        "DataPedido",

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format =
                                "dd/MM/yyyy HH:mm"
                        }
                });

            dgvPedidos.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colValor",

                    HeaderText = "Valor Total",

                    DataPropertyName =
                        "ValorTotal",

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",

                            Alignment =
                                DataGridViewContentAlignment
                                    .MiddleRight
                        }
                });

            dgvPedidos.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colStatus",

                    HeaderText = "Status",

                    DataPropertyName =
                        "Status"
                });

            dgvPedidos.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colUsuario",

                    HeaderText = "Usuário",

                    DataPropertyName =
                        "ApplicationUserId"
                });

            dgvPedidos.Columns.Add(
                new DataGridViewButtonColumn
                {
                    Name = "colEditar",

                    HeaderText = "",

                    Text = "Editar",

                    UseColumnTextForButtonValue = true,

                    FlatStyle =
                        FlatStyle.Flat
                });

            dgvPedidos.CellContentClick +=
                DgvPedidos_CellContentClick;
        }

        private void AjustarColunasGrid()
        {
            if (dgvPedidos == null ||
                dgvPedidos.Columns.Count == 0)
            {
                return;
            }

            dgvPedidos.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvPedidos.Columns[
                "colId"]!.FillWeight = 7;

            dgvPedidos.Columns[
                "colData"]!.FillWeight = 20;

            dgvPedidos.Columns[
                "colValor"]!.FillWeight = 15;

            dgvPedidos.Columns[
                "colStatus"]!.FillWeight = 18;

            dgvPedidos.Columns[
                "colUsuario"]!.FillWeight = 28;

            dgvPedidos.Columns[
                "colEditar"]!.FillWeight = 10;
        }

        // =========================================================
        // CARREGAR PEDIDOS
        // =========================================================

        private async Task CarregarPedidosAsync()
        {
            try
            {
                lblQuantidade.Text =
                    "Carregando pedidos...";

                dgvPedidos.Enabled = false;

                HttpResponseMessage resposta =
                    await httpClient.GetAsync(
                        "Pedido");

                if (!resposta.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"A API retornou o status " +
                        $"{(int)resposta.StatusCode}.");
                }

                List<PedidoModel>? dados =
                    await resposta.Content
                        .ReadFromJsonAsync<
                            List<PedidoModel>>(
                                jsonOptions);

                pedidos =
                    dados ??
                    new List<PedidoModel>();

                dgvPedidos.DataSource = null;

                dgvPedidos.DataSource =
                    pedidos;

                lblQuantidade.Text =
                    $"{pedidos.Count} pedido(s) encontrado(s)";
            }
            catch (Exception ex)
            {
                lblQuantidade.Text =
                    "Erro ao carregar pedidos.";

                MessageBox.Show(
                    "Não foi possível carregar os pedidos.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                dgvPedidos.Enabled = true;
            }
        }

        // =========================================================
        // FILTRAR
        // =========================================================

        private void FiltrarPedidos()
        {
            string pesquisa =
                txtPesquisa.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                pesquisa))
            {
                dgvPedidos.DataSource = null;

                dgvPedidos.DataSource =
                    pedidos;

                lblQuantidade.Text =
                    $"{pedidos.Count} pedido(s) encontrado(s)";

                return;
            }

            List<PedidoModel> resultado =
                pedidos
                    .Where(p =>
                        p.Id
                            .ToString()
                            .Contains(
                                pesquisa,
                                StringComparison
                                    .OrdinalIgnoreCase)

                        ||

                        p.Status.Contains(
                            pesquisa,
                            StringComparison
                                .OrdinalIgnoreCase)

                        ||

                        (p.ApplicationUserId ?? "")
                            .Contains(
                                pesquisa,
                                StringComparison
                                    .OrdinalIgnoreCase))
                    .ToList();

            dgvPedidos.DataSource = null;

            dgvPedidos.DataSource =
                resultado;

            lblQuantidade.Text =
                $"{resultado.Count} pedido(s) encontrado(s)";
        }

        // =========================================================
        // BOTÃO EDITAR
        // =========================================================

        private void DgvPedidos_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex ==
                dgvPedidos.Columns[
                    "colEditar"]!.Index)
            {
                _ = AbrirEdicaoPedidoAsync();
            }
        }

        // =========================================================
        // NOVO PEDIDO
        // =========================================================

        private async Task AbrirCadastroPedidoAsync()
        {
            using var formulario =
                new FrmPedidoCadastro();

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado ==
                DialogResult.OK)
            {
                await CarregarPedidosAsync();
            }
        }

        // =========================================================
        // EDITAR PEDIDO
        // =========================================================

        private async Task AbrirEdicaoPedidoAsync()
        {
            if (dgvPedidos.CurrentRow == null)
                return;

            if (dgvPedidos.CurrentRow.DataBoundItem
                is not PedidoModel pedido)
            {
                return;
            }

            using var formulario =
                new FrmPedidoCadastro(
                    pedido.Id);

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado ==
                DialogResult.OK)
            {
                await CarregarPedidosAsync();
            }
        }

        // =========================================================
        // ATUALIZAR GRID
        // =========================================================

        private void AtualizarGrid()
        {
            if (dgvPedidos == null ||
                dgvPedidos.IsDisposed)
            {
                return;
            }

            float escala =
                ObterEscala();

            dgvPedidos.RowTemplate.Height =
                Math.Max(
                    38,
                    (int)Math.Round(
                        48 * escala));

            dgvPedidos.DefaultCellStyle.Font =
                new Font(
                    AppTheme.FontFamily,
                    Math.Max(
                        8f,
                        AppTheme.FontBody.Size *
                        escala),
                    AppTheme.FontBody.Style);

            dgvPedidos.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    AppTheme.FontFamily,
                    Math.Max(
                        8f,
                        9.5f * escala),
                    FontStyle.Bold);
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