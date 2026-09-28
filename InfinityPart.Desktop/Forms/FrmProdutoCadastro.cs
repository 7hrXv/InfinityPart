using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmProdutoCadastro : Form
    {
        private const float LARGURA_REFERENCIA = 900f;
        private const float ALTURA_REFERENCIA = 700f;

        private readonly int? produtoId;

        private Panel pnlPrincipal = null!;
        private Label lblTitulo = null!;

        private Label lblNome = null!;
        private Label lblCodigo = null!;
        private Label lblDescricao = null!;
        private Label lblPreco = null!;
        private Label lblEstoque = null!;
        private Label lblMarca = null!;
        private Label lblCategoria = null!;

        private TextBox txtNome = null!;
        private TextBox txtCodigo = null!;
        private TextBox txtDescricao = null!;
        private TextBox txtPreco = null!;
        private TextBox txtEstoque = null!;

        private ComboBox cmbMarca = null!;
        private ComboBox cmbCategoria = null!;

        private Button btnSalvar = null!;
        private Button btnCancelar = null!;
        private Button btnExcluir = null!;

        private readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5022/api/")
        };

        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private bool aplicandoEscala;

        private readonly Dictionary<Control, InformacaoLayout>
            controlesEscalaveis = new();

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

        public FrmProdutoCadastro(int? id = null)
        {
            produtoId = id;

            InicializarFormulario();

            Load += async (_, _) =>
            {
                await CarregarCategoriasAsync();
                await CarregarMarcasAsync();

                if (produtoId.HasValue)
                {
                    await CarregarProdutoAsync(produtoId.Value);
                }
            };
        }

        private void InicializarFormulario()
        {
            Text = produtoId.HasValue
                ? "InfinityPart - Editar Produto"
                : "InfinityPart - Novo Produto";

            StartPosition = FormStartPosition.CenterParent;

            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;

            ClientSize = new Size(900, 700);
            MinimumSize = new Size(760, 620);

            AutoScaleMode = AutoScaleMode.Dpi;

            BackColor = AppTheme.Background;
            ForeColor = AppTheme.TextPrimary;
            Font = AppTheme.FontBody;

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
                Math.Min(escalaX, escalaY);

            return Math.Max(escala, 0.80f);
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

            aplicandoEscala = true;

            try
            {
                float escala = ObterEscala();

                int larguraPainel =
                    (int)Math.Round(780 * escala);

                int alturaPainel =
                    (int)Math.Round(620 * escala);

                pnlPrincipal.Size =
                    new Size(
                        larguraPainel,
                        alturaPainel);

                pnlPrincipal.Location =
                    new Point(
                        (ClientSize.Width - pnlPrincipal.Width) / 2,
                        (ClientSize.Height - pnlPrincipal.Height) / 2);

                foreach (
                    KeyValuePair<Control, InformacaoLayout> item
                    in controlesEscalaveis)
                {
                    Control controle = item.Key;

                    if (controle.IsDisposed)
                        continue;

                    InformacaoLayout info = item.Value;

                    Rectangle baseBounds =
                        info.BoundsBase;

                    controle.Bounds =
                        new Rectangle(
                            (int)Math.Round(
                                baseBounds.X * escala),

                            (int)Math.Round(
                                baseBounds.Y * escala),

                            Math.Max(
                                1,
                                (int)Math.Round(
                                    baseBounds.Width * escala)),

                            Math.Max(
                                1,
                                (int)Math.Round(
                                    baseBounds.Height * escala)));

                    if (info.TamanhoFonteBase > 0)
                    {
                        float tamanhoFonte =
                            Math.Max(
                                1f,
                                info.TamanhoFonteBase * escala);

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
                Size = new Size(780, 620),

                BackColor =
                    AppTheme.Surface,

                BorderStyle =
                    BorderStyle.FixedSingle
            };

            Controls.Add(pnlPrincipal);

            lblTitulo = new Label
            {
                Text = produtoId.HasValue
                    ? "Editar Produto"
                    : "Novo Produto",

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

            pnlPrincipal.Controls.Add(lblTitulo);

            RegistrarControle(
                lblTitulo,
                new Rectangle(
                    35,
                    25,
                    710,
                    50),
                22f);

            CriarCampo(
                "Nome",
                ref lblNome,
                ref txtNome,
                35,
                100);

            CriarCampo(
                "Código",
                ref lblCodigo,
                ref txtCodigo,
                395,
                100);

            CriarCampo(
                "Descrição",
                ref lblDescricao,
                ref txtDescricao,
                35,
                200,
                710,
                90);

            CriarCampo(
                "Preço",
                ref lblPreco,
                ref txtPreco,
                35,
                330);

            CriarCampo(
                "Estoque",
                ref lblEstoque,
                ref txtEstoque,
                395,
                330);

            CriarCampoMarca();

            CriarCampoCategoria();

            // =====================================================
            // BOTÃO CANCELAR
            // =====================================================

            btnCancelar = new Button
            {
                Text = "Cancelar",

                Bounds =
                    new Rectangle(
                        415,
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

            btnCancelar.FlatAppearance.BorderSize = 0;

            btnCancelar.Click += (_, _) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            pnlPrincipal.Controls.Add(btnCancelar);

            RegistrarControle(
                btnCancelar,
                new Rectangle(
                    415,
                    535,
                    145,
                    45),
                AppTheme.FontBody.Size);

            // =====================================================
            // BOTÃO SALVAR
            // =====================================================

            btnSalvar = new Button
            {
                Text = "Salvar",

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

            btnSalvar.FlatAppearance.BorderSize = 0;

            btnSalvar.Click += async (_, _) =>
            {
                await SalvarProdutoAsync();
            };

            pnlPrincipal.Controls.Add(btnSalvar);

            RegistrarControle(
                btnSalvar,
                new Rectangle(
                    580,
                    535,
                    165,
                    45),
                AppTheme.FontBody.Size);

            // =====================================================
            // BOTÃO EXCLUIR
            // =====================================================

            if (produtoId.HasValue)
            {
                btnExcluir = new Button
                {
                    Text = "Excluir Produto",

                    Bounds =
                        new Rectangle(
                            35,
                            535,
                            165,
                            45),

                    BackColor =
                        Color.FromArgb(
                            190,
                            35,
                            35),

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

                btnExcluir.FlatAppearance.BorderSize = 0;

                btnExcluir.Click += async (_, _) =>
                {
                    await ExcluirProdutoAsync();
                };

                pnlPrincipal.Controls.Add(btnExcluir);

                RegistrarControle(
                    btnExcluir,
                    new Rectangle(
                        35,
                        535,
                        165,
                        45),
                    AppTheme.FontBody.Size);
            }
        }

        // =========================================================
        // CAMPO MARCA
        // =========================================================

        private void CriarCampoMarca()
        {
            lblMarca = new Label
            {
                Text = "Marca",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        35,
                        430,
                        330,
                        25),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.MiddleLeft
            };

            pnlPrincipal.Controls.Add(lblMarca);

            RegistrarControle(
                lblMarca,
                new Rectangle(
                    35,
                    430,
                    330,
                    25),
                AppTheme.FontSmall.Size);

            cmbMarca = new ComboBox
            {
                Bounds =
                    new Rectangle(
                        35,
                        458,
                        330,
                        45),

                BackColor =
                    AppTheme.Background,

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontBody,

                DropDownStyle =
                    ComboBoxStyle.DropDownList,

                FormattingEnabled = true
            };

            pnlPrincipal.Controls.Add(cmbMarca);

            RegistrarControle(
                cmbMarca,
                new Rectangle(
                    35,
                    458,
                    330,
                    45),
                AppTheme.FontBody.Size);
        }

        // =========================================================
        // CAMPO CATEGORIA
        // =========================================================

        private void CriarCampoCategoria()
        {
            lblCategoria = new Label
            {
                Text = "Categoria",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        395,
                        430,
                        330,
                        25),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.MiddleLeft
            };

            pnlPrincipal.Controls.Add(lblCategoria);

            RegistrarControle(
                lblCategoria,
                new Rectangle(
                    395,
                    430,
                    330,
                    25),
                AppTheme.FontSmall.Size);

            cmbCategoria = new ComboBox
            {
                Bounds =
                    new Rectangle(
                        395,
                        458,
                        330,
                        45),

                BackColor =
                    AppTheme.Background,

                ForeColor =
                    AppTheme.TextPrimary,

                Font =
                    AppTheme.FontBody,

                DropDownStyle =
                    ComboBoxStyle.DropDownList,

                FormattingEnabled = true
            };

            pnlPrincipal.Controls.Add(cmbCategoria);

            RegistrarControle(
                cmbCategoria,
                new Rectangle(
                    395,
                    458,
                    330,
                    45),
                AppTheme.FontBody.Size);
        }

        // =========================================================
        // CAMPOS DE TEXTO
        // =========================================================

        private void CriarCampo(
            string texto,
            ref Label label,
            ref TextBox textBox,
            int x,
            int y,
            int largura = 330,
            int altura = 45)
        {
            label = new Label
            {
                Text = texto,

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        x,
                        y,
                        largura,
                        25),

                ForeColor =
                    AppTheme.TextSecondary,

                Font =
                    AppTheme.FontSmall,

                TextAlign =
                    ContentAlignment.MiddleLeft
            };

            pnlPrincipal.Controls.Add(label);

            RegistrarControle(
                label,
                new Rectangle(
                    x,
                    y,
                    largura,
                    25),
                AppTheme.FontSmall.Size);

            textBox = new TextBox
            {
                Bounds =
                    new Rectangle(
                        x,
                        y + 28,
                        largura,
                        altura),

                BackColor =
                    AppTheme.Background,

                ForeColor =
                    AppTheme.TextPrimary,

                BorderStyle =
                    BorderStyle.FixedSingle,

                Font =
                    AppTheme.FontBody,

                Multiline =
                    altura > 50,

                ScrollBars =
                    altura > 50
                        ? ScrollBars.Vertical
                        : ScrollBars.None
            };

            pnlPrincipal.Controls.Add(textBox);

            RegistrarControle(
                textBox,
                new Rectangle(
                    x,
                    y + 28,
                    largura,
                    altura),
                AppTheme.FontBody.Size);
        }

        // =========================================================
        // CARREGAR CATEGORIAS
        // =========================================================

        private async Task CarregarCategoriasAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                List<CategoriaModel>? categorias =
                    await httpClient.GetFromJsonAsync<List<CategoriaModel>>(
                        "Categoria",
                        jsonOptions);

                categorias ??= new List<CategoriaModel>();

                cmbCategoria.DataSource = null;

                cmbCategoria.DisplayMember =
                    nameof(CategoriaModel.Nome);

                cmbCategoria.ValueMember =
                    nameof(CategoriaModel.Id);

                cmbCategoria.DataSource =
                    categorias;

                if (categorias.Count == 0)
                {
                    cmbCategoria.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar as categorias.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                cmbCategoria.DataSource = null;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // =========================================================
        // CARREGAR MARCAS
        // =========================================================

        private async Task CarregarMarcasAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                List<MarcaModel>? marcas =
                    await httpClient.GetFromJsonAsync<List<MarcaModel>>(
                        "Marca",
                        jsonOptions);

                marcas ??= new List<MarcaModel>();

                cmbMarca.DataSource = null;

                cmbMarca.DisplayMember =
                    nameof(MarcaModel.Nome);

                cmbMarca.ValueMember =
                    nameof(MarcaModel.Id);

                cmbMarca.DataSource =
                    marcas;

                if (marcas.Count == 0)
                {
                    cmbMarca.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar as marcas.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                cmbMarca.DataSource = null;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // =========================================================
        // CARREGAR PRODUTO
        // =========================================================

        private async Task CarregarProdutoAsync(int id)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                ProdutoModel? produto =
                    await httpClient.GetFromJsonAsync<ProdutoModel>(
                        $"Produto/{id}",
                        jsonOptions);

                if (produto == null)
                {
                    MessageBox.Show(
                        "Produto não encontrado.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    DialogResult = DialogResult.Cancel;
                    Close();

                    return;
                }

                txtNome.Text =
                    produto.Nome ?? string.Empty;

                txtCodigo.Text =
                    produto.Codigo ?? string.Empty;

                txtDescricao.Text =
                    produto.Descricao ?? string.Empty;

                txtPreco.Text =
                    produto.Preco.ToString("0.00");

                txtEstoque.Text =
                    produto.QuantidadeEstoque.ToString();

                // =================================================
                // SELECIONAR MARCA
                // =================================================

                if (produto.MarcaId > 0)
                {
                    cmbMarca.SelectedValue =
                        produto.MarcaId;
                }
                else
                {
                    cmbMarca.SelectedIndex = -1;
                }

                // =================================================
                // SELECIONAR CATEGORIA
                // =================================================

                if (produto.CategoriaId.HasValue)
                {
                    cmbCategoria.SelectedValue =
                        produto.CategoriaId.Value;
                }
                else
                {
                    cmbCategoria.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar o produto.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // =========================================================
        // SALVAR PRODUTO
        // =========================================================

        private async Task SalvarProdutoAsync()
        {
            // -----------------------------------------------------
            // NOME
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show(
                    "Informe o nome do produto.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNome.Focus();
                return;
            }

            // -----------------------------------------------------
            // CÓDIGO
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show(
                    "Informe o código do produto.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCodigo.Focus();
                return;
            }

            // -----------------------------------------------------
            // PREÇO
            // -----------------------------------------------------

            if (!decimal.TryParse(
                    txtPreco.Text.Replace(',', '.'),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal preco))
            {
                MessageBox.Show(
                    "Informe um preço válido.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPreco.Focus();
                return;
            }

            // -----------------------------------------------------
            // ESTOQUE
            // -----------------------------------------------------

            if (!int.TryParse(
                    txtEstoque.Text,
                    out int estoque))
            {
                MessageBox.Show(
                    "Informe uma quantidade de estoque válida.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtEstoque.Focus();
                return;
            }

            // -----------------------------------------------------
            // MARCA
            // -----------------------------------------------------

            if (cmbMarca.SelectedValue == null ||
                cmbMarca.SelectedValue == DBNull.Value)
            {
                MessageBox.Show(
                    "Selecione uma marca.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbMarca.Focus();
                return;
            }

            int marcaId;

            try
            {
                marcaId =
                    Convert.ToInt32(
                        cmbMarca.SelectedValue);
            }
            catch
            {
                MessageBox.Show(
                    "Selecione uma marca válida.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbMarca.Focus();
                return;
            }

            // -----------------------------------------------------
            // CATEGORIA
            // -----------------------------------------------------

            int? categoriaId = null;

            if (cmbCategoria.SelectedValue != null &&
                cmbCategoria.SelectedValue != DBNull.Value)
            {
                try
                {
                    categoriaId =
                        Convert.ToInt32(
                            cmbCategoria.SelectedValue);
                }
                catch
                {
                    categoriaId = null;
                }
            }

            // -----------------------------------------------------
            // PRODUTO
            // -----------------------------------------------------

            var produto =
                new ProdutoRequestModel
                {
                    Nome =
                        txtNome.Text.Trim(),

                    Codigo =
                        txtCodigo.Text.Trim(),

                    Descricao =
                        txtDescricao.Text.Trim(),

                    Preco =
                        preco,

                    QuantidadeEstoque =
                        estoque,

                    Image =
                        string.Empty,

                    MarcaId =
                        marcaId,

                    CategoriaId =
                        categoriaId
                };

            try
            {
                Cursor = Cursors.WaitCursor;

                btnSalvar.Enabled = false;

                HttpResponseMessage resposta;

                // =================================================
                // EDITAR
                // =================================================

                if (produtoId.HasValue)
                {
                    resposta =
                        await httpClient.PutAsJsonAsync(
                            $"Produto/{produtoId.Value}",
                            produto);
                }
                // =================================================
                // CRIAR
                // =================================================
                else
                {
                    resposta =
                        await httpClient.PostAsJsonAsync(
                            "Produto",
                            produto);
                }

                string respostaTexto =
                    await resposta.Content.ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível salvar o produto.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    produtoId.HasValue
                        ? "Produto atualizado com sucesso!"
                        : "Produto criado com sucesso!",
                    "InfinityPart",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao salvar o produto.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSalvar.Enabled = true;

                Cursor = Cursors.Default;
            }
        }

        // =========================================================
        // EXCLUIR PRODUTO
        // =========================================================

        private async Task ExcluirProdutoAsync()
        {
            if (!produtoId.HasValue)
                return;

            DialogResult confirmacao =
                MessageBox.Show(
                    "Tem certeza que deseja excluir este produto?\n\n" +
                    "Essa ação não poderá ser desfeita.",
                    "Excluir Produto",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmacao != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                btnExcluir.Enabled = false;
                btnSalvar.Enabled = false;
                btnCancelar.Enabled = false;

                HttpResponseMessage resposta =
                    await httpClient.DeleteAsync(
                        $"Produto/{produtoId.Value}");

                string respostaTexto =
                    await resposta.Content.ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível excluir o produto.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro ao excluir",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    "Produto excluído com sucesso!",
                    "InfinityPart",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao excluir o produto.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (btnExcluir != null)
                    btnExcluir.Enabled = true;

                btnSalvar.Enabled = true;
                btnCancelar.Enabled = true;

                Cursor = Cursors.Default;
            }
        }

        // =========================================================
        // MODELOS
        // =========================================================

        private class CategoriaModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } =
                string.Empty;

            public string? Descricao { get; set; }
        }

        private class MarcaModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } =
                string.Empty;
        }

        private class ProdutoModel
        {
            public int Id { get; set; }

            public string? Nome { get; set; }

            public string? Codigo { get; set; }

            public string? Descricao { get; set; }

            public decimal Preco { get; set; }

            public int QuantidadeEstoque { get; set; }

            public string? Image { get; set; }

            public int MarcaId { get; set; }

            public int? CategoriaId { get; set; }
        }

        private class ProdutoRequestModel
        {
            public string Nome { get; set; } =
                string.Empty;

            public string Codigo { get; set; } =
                string.Empty;

            public string Descricao { get; set; } =
                string.Empty;

            public decimal Preco { get; set; }

            public int QuantidadeEstoque { get; set; }

            public string Image { get; set; } =
                string.Empty;

            public int MarcaId { get; set; }

            public int? CategoriaId { get; set; }
        }
    }
}