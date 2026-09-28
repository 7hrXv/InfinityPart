using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmClientes : Form
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
        private Button btnNovoCliente = null!;
        private DataGridView dgvClientes = null!;

        private readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5022/api/")
        };

        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private bool aplicandoEscala;

        private List<ClienteModel> clientes = new();

        private class ClienteModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } = "";

            public string Cpf { get; set; } = "";

            public string Email { get; set; } = "";

            public string Telefone { get; set; } = "";

            public string Cep { get; set; } = "";

            public string Endereco { get; set; } = "";

            public string Numero { get; set; } = "";

            public string Cidade { get; set; } = "";

            public string Estado { get; set; } = "";
        }

        public FrmClientes()
        {
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            Text = "InfinityPart - Clientes";

            StartPosition =
                FormStartPosition.CenterScreen;

            FormBorderStyle =
                FormBorderStyle.Sizable;

            MaximizeBox = true;
            MinimizeBox = true;

            ClientSize =
                new Size(1200, 800);

            MinimumSize =
                new Size(1000, 650);

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
                await CarregarClientesAsync();
            };
        }

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

                // ==========================================
                // CABEÇALHO
                // ==========================================

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

                // ==========================================
                // TABELA
                // ==========================================

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

                // ==========================================
                // TÍTULO
                // ==========================================

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

                // ==========================================
                // QUANTIDADE
                // ==========================================

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

                // ==========================================
                // BOTÕES
                // ==========================================

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

                btnNovoCliente.Bounds =
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
                        btnNovoCliente.Left -
                        espacamento -
                        larguraPesquisar,
                        Math.Max(
                            0,
                            (int)Math.Round(
                                5 * escala)),
                        larguraPesquisar,
                        alturaControles);

                // ==========================================
                // PESQUISA
                // ==========================================

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

                // ==========================================
                // FONTES
                // ==========================================

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

                btnNovoCliente.Font =
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

            // ==========================================
            // TÍTULO
            // ==========================================

            lblTitulo = new Label
            {
                Text = "Clientes",
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

            // ==========================================
            // QUANTIDADE
            // ==========================================

            lblQuantidade = new Label
            {
                Text =
                    "Carregando clientes...",
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

            // ==========================================
            // PESQUISA
            // ==========================================

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
                    "Pesquisar cliente ou CPF..."
            };

            txtPesquisa.TextChanged += (_, _) =>
            {
                FiltrarClientes();
            };

            pnlCabecalho.Controls.Add(
                txtPesquisa);

            // ==========================================
            // PESQUISAR
            // ==========================================

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
                FiltrarClientes();
            };

            pnlCabecalho.Controls.Add(
                btnPesquisar);

            // ==========================================
            // NOVO CLIENTE
            // ==========================================

            btnNovoCliente = new Button
            {
                Text = "+  Novo Cliente",
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

            btnNovoCliente.FlatAppearance.BorderSize = 0;

            btnNovoCliente.Click += async (_, _) =>
            {
                await AbrirCadastroClienteAsync();
            };

            pnlCabecalho.Controls.Add(
                btnNovoCliente);
        }

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

            dgvClientes = new DataGridView
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

            dgvClientes.ColumnHeadersDefaultCellStyle =
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

            dgvClientes.DefaultCellStyle =
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

            dgvClientes.RowTemplate.Height = 48;

            CriarColunas();

            dgvClientes.CellDoubleClick += (_, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    _ = AbrirEdicaoClienteAsync();
                }
            };

            pnlTabela.Controls.Add(
                dgvClientes);

            AjustarColunasGrid();
        }

        private void CriarColunas()
        {
            dgvClientes.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colId",
                    HeaderText = "ID",
                    DataPropertyName = "Id"
                });

            dgvClientes.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colNome",
                    HeaderText = "Cliente",
                    DataPropertyName = "Nome"
                });

            dgvClientes.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colCpf",
                    HeaderText = "CPF",
                    DataPropertyName = "Cpf"
                });

            dgvClientes.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colEmail",
                    HeaderText = "E-mail",
                    DataPropertyName = "Email"
                });

            dgvClientes.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colTelefone",
                    HeaderText = "Telefone",
                    DataPropertyName = "Telefone"
                });

            dgvClientes.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colCidade",
                    HeaderText = "Cidade",
                    DataPropertyName = "Cidade"
                });

            dgvClientes.Columns.Add(
                new DataGridViewButtonColumn
                {
                    Name = "colEditar",
                    HeaderText = "",
                    Text = "Editar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle =
                        FlatStyle.Flat
                });

            dgvClientes.CellContentClick +=
                DgvClientes_CellContentClick;
        }

        private void AjustarColunasGrid()
        {
            if (dgvClientes == null ||
                dgvClientes.Columns.Count == 0)
                return;

            dgvClientes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvClientes.Columns["colId"]!.FillWeight = 6;
            dgvClientes.Columns["colNome"]!.FillWeight = 24;
            dgvClientes.Columns["colCpf"]!.FillWeight = 15;
            dgvClientes.Columns["colEmail"]!.FillWeight = 23;
            dgvClientes.Columns["colTelefone"]!.FillWeight = 14;
            dgvClientes.Columns["colCidade"]!.FillWeight = 12;
            dgvClientes.Columns["colEditar"]!.FillWeight = 9;
        }

        private async Task CarregarClientesAsync()
        {
            try
            {
                lblQuantidade.Text =
                    "Carregando clientes...";

                dgvClientes.Enabled = false;

                HttpResponseMessage resposta =
                    await httpClient.GetAsync(
                        "Cliente");

                if (!resposta.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"A API retornou o status " +
                        $"{(int)resposta.StatusCode}.");
                }

                List<ClienteModel>? dados =
                    await resposta.Content
                        .ReadFromJsonAsync<List<ClienteModel>>(
                            jsonOptions);

                clientes =
                    dados ??
                    new List<ClienteModel>();

                dgvClientes.DataSource = null;

                dgvClientes.DataSource =
                    clientes;

                lblQuantidade.Text =
                    $"{clientes.Count} cliente(s) encontrado(s)";
            }
            catch (Exception ex)
            {
                lblQuantidade.Text =
                    "Erro ao carregar clientes.";

                MessageBox.Show(
                    "Não foi possível carregar os clientes.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                dgvClientes.Enabled = true;
            }
        }

        private void FiltrarClientes()
        {
            string pesquisa =
                txtPesquisa.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                pesquisa))
            {
                dgvClientes.DataSource = null;

                dgvClientes.DataSource =
                    clientes;

                lblQuantidade.Text =
                    $"{clientes.Count} cliente(s) encontrado(s)";

                return;
            }

            List<ClienteModel> resultado =
                clientes
                    .Where(c =>
                        c.Nome.Contains(
                            pesquisa,
                            StringComparison
                                .OrdinalIgnoreCase)
                        ||
                        c.Cpf.Contains(
                            pesquisa,
                            StringComparison
                                .OrdinalIgnoreCase)
                        ||
                        c.Email.Contains(
                            pesquisa,
                            StringComparison
                                .OrdinalIgnoreCase))
                    .ToList();

            dgvClientes.DataSource = null;

            dgvClientes.DataSource =
                resultado;

            lblQuantidade.Text =
                $"{resultado.Count} cliente(s) encontrado(s)";
        }

        private void DgvClientes_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex ==
                dgvClientes.Columns[
                    "colEditar"]!.Index)
            {
                _ = AbrirEdicaoClienteAsync();
            }
        }

        private async Task AbrirCadastroClienteAsync()
        {
            using var formulario =
                new FrmClienteCadastro();

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado ==
                DialogResult.OK)
            {
                await CarregarClientesAsync();
            }
        }

        private async Task AbrirEdicaoClienteAsync()
        {
            if (dgvClientes.CurrentRow == null)
                return;

            if (dgvClientes.CurrentRow.DataBoundItem
                is not ClienteModel cliente)
                return;

            using var formulario =
                new FrmClienteCadastro(
                    cliente.Id);

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado ==
                DialogResult.OK)
            {
                await CarregarClientesAsync();
            }
        }

        private void AtualizarGrid()
        {
            if (dgvClientes == null ||
                dgvClientes.IsDisposed)
                return;

            float escala =
                ObterEscala();

            dgvClientes.RowTemplate.Height =
                Math.Max(
                    38,
                    (int)Math.Round(
                        48 * escala));

            dgvClientes.DefaultCellStyle.Font =
                new Font(
                    AppTheme.FontFamily,
                    Math.Max(
                        8f,
                        AppTheme.FontBody.Size *
                        escala),
                    AppTheme.FontBody.Style);

            dgvClientes.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    AppTheme.FontFamily,
                    Math.Max(
                        8f,
                        9.5f * escala),
                    FontStyle.Bold);
        }

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}