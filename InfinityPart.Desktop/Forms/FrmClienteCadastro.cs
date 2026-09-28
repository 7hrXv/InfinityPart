using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmClienteCadastro : Form
    {
        private const float LARGURA_REFERENCIA = 900f;
        private const float ALTURA_REFERENCIA = 700f;

        private readonly int? clienteId;

        private Panel pnlPrincipal = null!;
        private Label lblTitulo = null!;

        private Label lblNome = null!;
        private Label lblCpf = null!;
        private Label lblEmail = null!;
        private Label lblTelefone = null!;
        private Label lblCep = null!;
        private Label lblEndereco = null!;
        private Label lblNumero = null!;
        private Label lblCidade = null!;
        private Label lblEstado = null!;

        private TextBox txtNome = null!;
        private TextBox txtCpf = null!;
        private TextBox txtEmail = null!;
        private TextBox txtTelefone = null!;
        private TextBox txtCep = null!;
        private TextBox txtEndereco = null!;
        private TextBox txtNumero = null!;
        private TextBox txtCidade = null!;
        private TextBox txtEstado = null!;

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

        public FrmClienteCadastro(int? id = null)
        {
            clienteId = id;

            InicializarFormulario();

            Load += async (_, _) =>
            {
                if (clienteId.HasValue)
                {
                    await CarregarClienteAsync(
                        clienteId.Value);
                }
            };
        }

        private void InicializarFormulario()
        {
            Text = clienteId.HasValue
                ? "InfinityPart - Editar Cliente"
                : "InfinityPart - Novo Cliente";

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
                        (ClientSize.Width -
                         pnlPrincipal.Width) / 2,

                        (ClientSize.Height -
                         pnlPrincipal.Height) / 2);

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

            lblTitulo = new Label
            {
                Text = clienteId.HasValue
                    ? "Editar Cliente"
                    : "Novo Cliente",

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
            // LINHA 1
            // ==========================================

            CriarCampo(
                "Nome",
                ref lblNome,
                ref txtNome,
                35,
                95,
                330);

            CriarCampo(
                "CPF",
                ref lblCpf,
                ref txtCpf,
                395,
                95,
                330);

            // ==========================================
            // LINHA 2
            // ==========================================

            CriarCampo(
                "E-mail",
                ref lblEmail,
                ref txtEmail,
                35,
                180,
                330);

            CriarCampo(
                "Telefone",
                ref lblTelefone,
                ref txtTelefone,
                395,
                180,
                330);

            // ==========================================
            // LINHA 3
            // ==========================================

            CriarCampo(
                "CEP",
                ref lblCep,
                ref txtCep,
                35,
                265,
                210);

            CriarCampo(
                "Número",
                ref lblNumero,
                ref txtNumero,
                495,
                265,
                230);

            CriarCampo(
                "Endereço",
                ref lblEndereco,
                ref txtEndereco,
                260,
                265,
                210);

            // ==========================================
            // LINHA 4
            // ==========================================

            CriarCampo(
                "Cidade",
                ref lblCidade,
                ref txtCidade,
                35,
                350,
                330);

            CriarCampo(
                "Estado",
                ref lblEstado,
                ref txtEstado,
                395,
                350,
                330);

            // ==========================================
            // BOTÕES
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
                await SalvarClienteAsync();
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

            if (clienteId.HasValue)
            {
                btnExcluir = new Button
                {
                    Text = "Excluir Cliente",

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
                    await ExcluirClienteAsync();
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
                    AppTheme.FontBody
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

        private async Task CarregarClienteAsync(int id)
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                ClienteModel? cliente =
                    await httpClient.GetFromJsonAsync<ClienteModel>(
                        $"Cliente/{id}",
                        jsonOptions);

                if (cliente == null)
                {
                    MessageBox.Show(
                        "Cliente não encontrado.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    DialogResult =
                        DialogResult.Cancel;

                    Close();

                    return;
                }

                txtNome.Text =
                    cliente.Nome ?? string.Empty;

                txtCpf.Text =
                    cliente.Cpf ?? string.Empty;

                txtEmail.Text =
                    cliente.Email ?? string.Empty;

                txtTelefone.Text =
                    cliente.Telefone ?? string.Empty;

                txtCep.Text =
                    cliente.Cep ?? string.Empty;

                txtEndereco.Text =
                    cliente.Endereco ?? string.Empty;

                txtNumero.Text =
                    cliente.Numero ?? string.Empty;

                txtCidade.Text =
                    cliente.Cidade ?? string.Empty;

                txtEstado.Text =
                    cliente.Estado ?? string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar o cliente.\n\n" +
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

        private async Task SalvarClienteAsync()
        {
            // ==========================================
            // VALIDAÇÕES
            // ==========================================

            if (string.IsNullOrWhiteSpace(
                txtNome.Text))
            {
                MessageBox.Show(
                    "Informe o nome do cliente.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNome.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtCpf.Text))
            {
                MessageBox.Show(
                    "Informe o CPF do cliente.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCpf.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtEmail.Text))
            {
                MessageBox.Show(
                    "Informe o e-mail do cliente.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtEmail.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtTelefone.Text))
            {
                MessageBox.Show(
                    "Informe o telefone do cliente.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTelefone.Focus();

                return;
            }

            // ==========================================
            // OBJETO
            // ==========================================

            var cliente =
                new ClienteRequestModel
                {
                    Nome =
                        txtNome.Text.Trim(),

                    Cpf =
                        txtCpf.Text.Trim(),

                    Email =
                        txtEmail.Text.Trim(),

                    Telefone =
                        txtTelefone.Text.Trim(),

                    Cep =
                        txtCep.Text.Trim(),

                    Endereco =
                        txtEndereco.Text.Trim(),

                    Numero =
                        txtNumero.Text.Trim(),

                    Cidade =
                        txtCidade.Text.Trim(),

                    Estado =
                        txtEstado.Text.Trim()
                };

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                btnSalvar.Enabled = false;

                HttpResponseMessage resposta;

                if (clienteId.HasValue)
                {
                    resposta =
                        await httpClient.PutAsJsonAsync(
                            $"Cliente/{clienteId.Value}",
                            cliente);
                }
                else
                {
                    resposta =
                        await httpClient.PostAsJsonAsync(
                            "Cliente",
                            cliente);
                }

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível salvar o cliente.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    clienteId.HasValue
                        ? "Cliente atualizado com sucesso!"
                        : "Cliente criado com sucesso!",
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
                    "Erro ao salvar o cliente.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSalvar.Enabled = true;

                Cursor =
                    Cursors.Default;
            }
        }

        private async Task ExcluirClienteAsync()
        {
            if (!clienteId.HasValue)
                return;

            DialogResult confirmacao =
                MessageBox.Show(
                    "Tem certeza que deseja excluir este cliente?\n\n" +
                    "Essa ação não poderá ser desfeita.",
                    "Excluir Cliente",
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

                btnExcluir.Enabled = false;
                btnSalvar.Enabled = false;
                btnCancelar.Enabled = false;

                HttpResponseMessage resposta =
                    await httpClient.DeleteAsync(
                        $"Cliente/{clienteId.Value}");

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível excluir o cliente.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro ao excluir",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    "Cliente excluído com sucesso!",
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
                    "Erro ao excluir o cliente.\n\n" +
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

                Cursor =
                    Cursors.Default;
            }
        }

        // =========================================================
        // MODELOS
        // =========================================================

        private class ClienteModel
        {
            public int Id { get; set; }

            public string? Nome { get; set; }

            public string? Cpf { get; set; }

            public string? Email { get; set; }

            public string? Telefone { get; set; }

            public string? Cep { get; set; }

            public string? Endereco { get; set; }

            public string? Numero { get; set; }

            public string? Cidade { get; set; }

            public string? Estado { get; set; }
        }

        private class ClienteRequestModel
        {
            public string Nome { get; set; } =
                string.Empty;

            public string Cpf { get; set; } =
                string.Empty;

            public string Email { get; set; } =
                string.Empty;

            public string Telefone { get; set; } =
                string.Empty;

            public string Cep { get; set; } =
                string.Empty;

            public string Endereco { get; set; } =
                string.Empty;

            public string Numero { get; set; } =
                string.Empty;

            public string Cidade { get; set; } =
                string.Empty;

            public string Estado { get; set; } =
                string.Empty;
        }
    }
}