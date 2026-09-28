using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmCategoriaCadastro : Form
    {
        private const float LARGURA_REFERENCIA = 900f;
        private const float ALTURA_REFERENCIA = 700f;

        private readonly int? categoriaId;

        private Panel pnlPrincipal = null!;

        private Label lblTitulo = null!;
        private Label lblNome = null!;
        private Label lblDescricao = null!;

        private TextBox txtNome = null!;
        private TextBox txtDescricao = null!;

        private Button btnSalvar = null!;
        private Button btnCancelar = null!;
        private Button btnExcluir = null!;

        private readonly HttpClient httpClient = new()
        {
            BaseAddress =
                new Uri("http://localhost:5022/api/")
        };

        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private bool aplicandoEscala;

        private readonly Dictionary<
            Control,
            InformacaoLayout>
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

        public FrmCategoriaCadastro(
            int? id = null)
        {
            categoriaId = id;

            InicializarFormulario();

            Load += async (_, _) =>
            {
                if (categoriaId.HasValue)
                {
                    await CarregarCategoriaAsync(
                        categoriaId.Value);
                }
            };
        }

        private void InicializarFormulario()
        {
            Text = categoriaId.HasValue
                ? "InfinityPart - Editar Categoria"
                : "InfinityPart - Nova Categoria";

            StartPosition =
                FormStartPosition.CenterParent;

            FormBorderStyle =
                FormBorderStyle.Sizable;

            MaximizeBox = true;
            MinimizeBox = true;

            ClientSize =
                new Size(900, 700);

            MinimumSize =
                new Size(760, 620);

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

                txtNome.Focus();
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

            // ==========================================
            // TÍTULO
            // ==========================================

            lblTitulo = new Label
            {
                Text = categoriaId.HasValue
                    ? "Editar Categoria"
                    : "Nova Categoria",

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

            // ==========================================
            // NOME
            // ==========================================

            CriarCampo(
                "Nome",
                ref lblNome,
                ref txtNome,
                35,
                105,
                710,
                45);

            // ==========================================
            // DESCRIÇÃO
            // ==========================================

            CriarCampo(
                "Descrição",
                ref lblDescricao,
                ref txtDescricao,
                35,
                210,
                710,
                150);

            // ==========================================
            // CANCELAR
            // ==========================================

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
                DialogResult =
                    DialogResult.Cancel;

                Close();
            };

            pnlPrincipal.Controls.Add(
                btnCancelar);

            RegistrarControle(
                btnCancelar,
                new Rectangle(
                    415,
                    535,
                    145,
                    45),
                AppTheme.FontBody.Size);

            // ==========================================
            // SALVAR
            // ==========================================

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
                await SalvarCategoriaAsync();
            };

            pnlPrincipal.Controls.Add(
                btnSalvar);

            RegistrarControle(
                btnSalvar,
                new Rectangle(
                    580,
                    535,
                    165,
                    45),
                AppTheme.FontBody.Size);

            // ==========================================
            // EXCLUIR
            // ==========================================

            if (categoriaId.HasValue)
            {
                btnExcluir = new Button
                {
                    Text = "Excluir Categoria",

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
                    await ExcluirCategoriaAsync();
                };

                pnlPrincipal.Controls.Add(
                    btnExcluir);

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

        private void CriarCampo(
            string texto,
            ref Label label,
            ref TextBox textBox,
            int x,
            int y,
            int largura,
            int altura)
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

            pnlPrincipal.Controls.Add(
                label);

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

            pnlPrincipal.Controls.Add(
                textBox);

            RegistrarControle(
                textBox,
                new Rectangle(
                    x,
                    y + 28,
                    largura,
                    altura),
                AppTheme.FontBody.Size);
        }

        private async Task CarregarCategoriaAsync(
            int id)
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                CategoriaModel? categoria =
                    await httpClient
                        .GetFromJsonAsync<CategoriaModel>(
                            $"Categoria/{id}",
                            jsonOptions);

                if (categoria == null)
                {
                    MessageBox.Show(
                        "Categoria não encontrada.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    DialogResult =
                        DialogResult.Cancel;

                    Close();

                    return;
                }

                txtNome.Text =
                    categoria.Nome ?? string.Empty;

                txtDescricao.Text =
                    categoria.Descricao ??
                    string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar a categoria.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor =
                    Cursors.Default;
            }
        }

        private async Task SalvarCategoriaAsync()
        {
            if (string.IsNullOrWhiteSpace(
                txtNome.Text))
            {
                MessageBox.Show(
                    "Informe o nome da categoria.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNome.Focus();

                return;
            }

            var categoria =
                new CategoriaRequestModel
                {
                    Nome =
                        txtNome.Text.Trim(),

                    Descricao =
                        txtDescricao.Text.Trim()
                };

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                btnSalvar.Enabled =
                    false;

                HttpResponseMessage resposta;

                if (categoriaId.HasValue)
                {
                    var atualizar =
                        new AtualizarCategoriaRequestModel
                        {
                            Id =
                                categoriaId.Value,

                            Nome =
                                categoria.Nome,

                            Descricao =
                                categoria.Descricao
                        };

                    resposta =
                        await httpClient.PutAsJsonAsync(
                            $"Categoria/{categoriaId.Value}",
                            atualizar);
                }
                else
                {
                    resposta =
                        await httpClient.PostAsJsonAsync(
                            "Categoria",
                            categoria);
                }

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível salvar a categoria.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    categoriaId.HasValue
                        ? "Categoria atualizada com sucesso!"
                        : "Categoria criada com sucesso!",
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
                    "Erro ao salvar a categoria.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSalvar.Enabled =
                    true;

                Cursor =
                    Cursors.Default;
            }
        }

        private async Task ExcluirCategoriaAsync()
        {
            if (!categoriaId.HasValue)
                return;

            DialogResult confirmacao =
                MessageBox.Show(
                    "Tem certeza que deseja excluir esta categoria?\n\n" +
                    "Essa ação não poderá ser desfeita.",
                    "Excluir Categoria",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmacao !=
                DialogResult.Yes)
            {
                return;
            }

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                btnExcluir.Enabled =
                    false;

                btnSalvar.Enabled =
                    false;

                btnCancelar.Enabled =
                    false;

                HttpResponseMessage resposta =
                    await httpClient.DeleteAsync(
                        $"Categoria/{categoriaId.Value}");

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível excluir a categoria.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro ao excluir",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    "Categoria excluída com sucesso!",
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
                    "Erro ao excluir a categoria.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (btnExcluir != null)
                    btnExcluir.Enabled = true;

                btnSalvar.Enabled =
                    true;

                btnCancelar.Enabled =
                    true;

                Cursor =
                    Cursors.Default;
            }
        }

        private class CategoriaModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } = "";

            public string Descricao { get; set; } = "";
        }

        private class CategoriaRequestModel
        {
            public string Nome { get; set; } = "";

            public string Descricao { get; set; } = "";
        }

        private class AtualizarCategoriaRequestModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } = "";

            public string Descricao { get; set; } = "";
        }

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}