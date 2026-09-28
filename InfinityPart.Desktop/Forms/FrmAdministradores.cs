using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmAdministradores : System.Windows.Forms.Form
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
        private Button btnNovoAdministrador = null!;

        private DataGridView dgvAdministradores = null!;

        private readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5022/api/")
        };

        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private bool aplicandoEscala;

        private List<AdministradorModel> administradores = new();

        private class AdministradorModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public string Cpf { get; set; } = string.Empty;

            public string Telefone { get; set; } = string.Empty;

            public DateTime DataCadastro { get; set; }

            public bool Ativo { get; set; }

            public DateTime? UltimoAcesso { get; set; }

            public bool PossuiSenhaDefinida { get; set; }
        }

        public FrmAdministradores()
        {
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            Text = "InfinityPart - Administradores";

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
                await CarregarAdministradoresAsync();
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
                            MARGEM_REFERENCIA * escala));

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
                        180,
                        (int)Math.Round(
                            230 * escala));

                int larguraPesquisar =
                    Math.Max(
                        100,
                        (int)Math.Round(
                            120 * escala));

                int larguraDisponivel =
                    pnlCabecalho.ClientSize.Width;

                btnNovoAdministrador.Bounds =
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
                        btnNovoAdministrador.Left -
                        espacamento -
                        larguraPesquisar,
                        Math.Max(
                            0,
                            (int)Math.Round(
                                5 * escala)),
                        larguraPesquisar,
                        alturaControles);

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

                btnNovoAdministrador.Font =
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

            lblTitulo = new Label
            {
                Text = "Administradores",
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

            lblQuantidade = new Label
            {
                Text =
                    "Carregando administradores...",
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
                    "Pesquisar administrador..."
            };

            txtPesquisa.TextChanged += (_, _) =>
            {
                FiltrarAdministradores();
            };

            pnlCabecalho.Controls.Add(
                txtPesquisa);

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
                FiltrarAdministradores();
            };

            pnlCabecalho.Controls.Add(
                btnPesquisar);

            btnNovoAdministrador = new Button
            {
                Text = "+  Novo Administrador",
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

            btnNovoAdministrador.FlatAppearance.BorderSize = 0;

            btnNovoAdministrador.Click += async (_, _) =>
            {
                await AbrirCadastroAdministradorAsync();
            };

            pnlCabecalho.Controls.Add(
                btnNovoAdministrador);
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

            dgvAdministradores = new DataGridView
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

            dgvAdministradores.ColumnHeadersDefaultCellStyle =
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

            dgvAdministradores.DefaultCellStyle =
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

            dgvAdministradores.RowTemplate.Height = 48;

            CriarColunas();

            dgvAdministradores.CellDoubleClick += (_, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    _ = AbrirEdicaoAdministradorAsync();
                }
            };

            pnlTabela.Controls.Add(
                dgvAdministradores);

            AjustarColunasGrid();
        }

        private void CriarColunas()
        {
            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colId",
                    HeaderText = "ID",
                    DataPropertyName = "Id"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colNome",
                    HeaderText = "Administrador",
                    DataPropertyName = "Nome"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colEmail",
                    HeaderText = "E-mail",
                    DataPropertyName = "Email"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colCpf",
                    HeaderText = "CPF",
                    DataPropertyName = "Cpf"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colTelefone",
                    HeaderText = "Telefone",
                    DataPropertyName = "Telefone"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colStatus",
                    HeaderText = "Status",
                    DataPropertyName = "Ativo"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colSenha",
                    HeaderText = "Senha",
                    DataPropertyName = "PossuiSenhaDefinida"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colUltimoAcesso",
                    HeaderText = "Último acesso",
                    DataPropertyName = "UltimoAcesso"
                });

            dgvAdministradores.Columns.Add(
                new DataGridViewButtonColumn
                {
                    Name = "colEditar",
                    HeaderText = "",
                    Text = "Editar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle =
                        FlatStyle.Flat
                });

            dgvAdministradores.CellFormatting +=
                DgvAdministradores_CellFormatting;

            dgvAdministradores.CellContentClick +=
                DgvAdministradores_CellContentClick;
        }

        private void AjustarColunasGrid()
        {
            if (dgvAdministradores == null ||
                dgvAdministradores.Columns.Count == 0)
                return;

            dgvAdministradores.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvAdministradores.Columns["colId"]!.FillWeight = 5;
            dgvAdministradores.Columns["colNome"]!.FillWeight = 18;
            dgvAdministradores.Columns["colEmail"]!.FillWeight = 19;
            dgvAdministradores.Columns["colCpf"]!.FillWeight = 12;
            dgvAdministradores.Columns["colTelefone"]!.FillWeight = 12;
            dgvAdministradores.Columns["colStatus"]!.FillWeight = 10;
            dgvAdministradores.Columns["colSenha"]!.FillWeight = 9;
            dgvAdministradores.Columns["colUltimoAcesso"]!.FillWeight = 14;
            dgvAdministradores.Columns["colEditar"]!.FillWeight = 9;
        }

        private void DgvAdministradores_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex ==
                dgvAdministradores.Columns[
                    "colStatus"]!.Index)
            {
                if (e.Value is bool ativo)
                {
                    e.Value =
                        ativo
                            ? "Ativo"
                            : "Inativo";

                    e.FormattingApplied = true;
                }
            }

            if (e.ColumnIndex ==
                dgvAdministradores.Columns[
                    "colSenha"]!.Index)
            {
                if (e.Value is bool possuiSenha)
                {
                    e.Value =
                        possuiSenha
                            ? "Definida"
                            : "Não definida";

                    e.FormattingApplied = true;
                }
            }

            if (e.ColumnIndex ==
                dgvAdministradores.Columns[
                    "colUltimoAcesso"]!.Index)
            {
                if (e.Value is DateTime data)
                {
                    e.Value =
                        data.ToLocalTime()
                            .ToString(
                                "dd/MM/yyyy HH:mm");

                    e.FormattingApplied = true;
                }
                else if (e.Value == null)
                {
                    e.Value = "Nunca";

                    e.FormattingApplied = true;
                }
            }
        }

        private async Task CarregarAdministradoresAsync()
        {
            try
            {
                lblQuantidade.Text =
                    "Carregando administradores...";

                dgvAdministradores.Enabled = false;

                HttpResponseMessage resposta =
                    await httpClient.GetAsync(
                        "Administrador");

                if (!resposta.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"A API retornou o status " +
                        $"{(int)resposta.StatusCode}.");
                }

                List<AdministradorModel>? dados =
                    await resposta.Content
                        .ReadFromJsonAsync<
                            List<AdministradorModel>>(
                            jsonOptions);

                administradores =
                    dados ??
                    new List<AdministradorModel>();

                dgvAdministradores.DataSource = null;

                dgvAdministradores.DataSource =
                    administradores;

                lblQuantidade.Text =
                    $"{administradores.Count} administrador(es) encontrado(s)";
            }
            catch (Exception ex)
            {
                lblQuantidade.Text =
                    "Erro ao carregar administradores.";

                MessageBox.Show(
                    "Não foi possível carregar os administradores.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                dgvAdministradores.Enabled = true;
            }
        }

        private void FiltrarAdministradores()
        {
            string pesquisa =
                txtPesquisa.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                pesquisa))
            {
                dgvAdministradores.DataSource = null;

                dgvAdministradores.DataSource =
                    administradores;

                lblQuantidade.Text =
                    $"{administradores.Count} administrador(es) encontrado(s)";

                return;
            }

            List<AdministradorModel> resultado =
                administradores
                    .Where(a =>
                        a.Nome.Contains(
                            pesquisa,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        a.Email.Contains(
                            pesquisa,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        a.Cpf.Contains(
                            pesquisa,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();

            dgvAdministradores.DataSource = null;

            dgvAdministradores.DataSource =
                resultado;

            lblQuantidade.Text =
                $"{resultado.Count} administrador(es) encontrado(s)";
        }

        private void DgvAdministradores_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex ==
                dgvAdministradores.Columns[
                    "colEditar"]!.Index)
            {
                _ = AbrirEdicaoAdministradorAsync();
            }
        }

        private async Task AbrirCadastroAdministradorAsync()
        {
            using var formulario =
                new FrmAdministradorCadastro();

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado ==
                DialogResult.OK)
            {
                await CarregarAdministradoresAsync();
            }
        }

        private async Task AbrirEdicaoAdministradorAsync()
        {
            if (dgvAdministradores.CurrentRow == null)
                return;

            if (dgvAdministradores.CurrentRow.DataBoundItem
                is not AdministradorModel administrador)
                return;

            using var formulario =
                new FrmAdministradorCadastro(
                    administrador.Id);

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado ==
                DialogResult.OK)
            {
                await CarregarAdministradoresAsync();
            }
        }

        private void AtualizarGrid()
        {
            if (dgvAdministradores == null ||
                dgvAdministradores.IsDisposed)
                return;

            float escala =
                ObterEscala();

            dgvAdministradores.RowTemplate.Height =
                Math.Max(
                    38,
                    (int)Math.Round(
                        48 * escala));

            dgvAdministradores.DefaultCellStyle.Font =
                new Font(
                    AppTheme.FontFamily,
                    Math.Max(
                        8f,
                        AppTheme.FontBody.Size *
                        escala),
                    AppTheme.FontBody.Style);

            dgvAdministradores.ColumnHeadersDefaultCellStyle.Font =
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