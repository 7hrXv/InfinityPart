using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Forms
{
    public class FrmAdministradorCadastro : System.Windows.Forms.Form
    {
        private const float LARGURA_REFERENCIA = 900f;
        private const float ALTURA_REFERENCIA = 700f;

        private readonly int? administradorId;

        private Panel pnlPrincipal = null!;
        private Label lblTitulo = null!;

        private Label lblNome = null!;
        private Label lblCpf = null!;
        private Label lblEmail = null!;
        private Label lblTelefone = null!;
        private Label lblSenha = null!;
        private Label lblAtivo = null!;

        private TextBox txtNome = null!;
        private TextBox txtCpf = null!;
        private TextBox txtEmail = null!;
        private TextBox txtTelefone = null!;
        private TextBox txtSenha = null!;

        private CheckBox chkAtivo = null!;

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

        public FrmAdministradorCadastro(int? id = null)
        {
            administradorId = id;

            InicializarFormulario();

            Load += async (_, _) =>
            {
                if (administradorId.HasValue)
                {
                    await CarregarAdministradorAsync(
                        administradorId.Value);
                }
            };
        }

        private void InicializarFormulario()
        {
            Text = administradorId.HasValue
                ? "InfinityPart - Editar Administrador"
                : "InfinityPart - Novo Administrador";

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
                Text = administradorId.HasValue
                    ? "Editar Administrador"
                    : "Novo Administrador",

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

            if (!administradorId.HasValue)
            {
                CriarCampoSenha(
                    35,
                    265,
                    690);
            }
            else
            {
                lblAtivo = new Label
                {
                    Text = "Status do administrador",

                    AutoSize = false,

                    Bounds =
                        new Rectangle(
                            35,
                            265,
                            330,
                            25),

                    ForeColor =
                        AppTheme.TextSecondary,

                    Font =
                        AppTheme.FontSmall,

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

                pnlPrincipal.Controls.Add(
                    lblAtivo);

                RegistrarControle(
                    lblAtivo,
                    new Rectangle(
                        35,
                        265,
                        330,
                        25),
                    AppTheme.FontSmall.Size);

                chkAtivo = new CheckBox
                {
                    Text = "Administrador ativo",

                    Checked = true,

                    AutoSize = false,

                    Bounds =
                        new Rectangle(
                            35,
                            293,
                            330,
                            45),

                    ForeColor =
                        AppTheme.TextPrimary,

                    Font =
                        AppTheme.FontBody,

                    BackColor =
                        AppTheme.Surface
                };

                pnlPrincipal.Controls.Add(
                    chkAtivo);

                RegistrarControle(
                    chkAtivo,
                    new Rectangle(
                        35,
                        293,
                        330,
                        45),
                    AppTheme.FontBody.Size);
            }

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
                await SalvarAdministradorAsync();
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

            if (administradorId.HasValue)
            {
                btnExcluir = new Button
                {
                    Text = "Excluir Administrador",

                    Bounds =
                        new Rectangle(
                            35,
                            535,
                            200,
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
                    await ExcluirAdministradorAsync();
                };

                pnlPrincipal.Controls.Add(
                    btnExcluir);

                RegistrarControle(
                    btnExcluir,
                    new Rectangle(
                        35,
                        535,
                        200,
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

        private void CriarCampoSenha(
            int x,
            int y,
            int largura)
        {
            lblSenha = new Label
            {
                Text = "Senha",

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
                lblSenha);

            RegistrarControle(
                lblSenha,
                new Rectangle(
                    x,
                    y,
                    largura,
                    25),
                AppTheme.FontSmall.Size);

            txtSenha = new TextBox
            {
                Bounds =
                    new Rectangle(
                        x,
                        y + 28,
                        largura,
                        45),

                BackColor =
                    AppTheme.Background,

                ForeColor =
                    AppTheme.TextPrimary,

                BorderStyle =
                    BorderStyle.FixedSingle,

                Font =
                    AppTheme.FontBody,

                UseSystemPasswordChar = true
            };

            pnlPrincipal.Controls.Add(
                txtSenha);

            RegistrarControle(
                txtSenha,
                new Rectangle(
                    x,
                    y + 28,
                    largura,
                    45),
                AppTheme.FontBody.Size);
        }

        private async Task CarregarAdministradorAsync(
            int id)
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                AdministradorModel? administrador =
                    await httpClient.GetFromJsonAsync<
                        AdministradorModel>(
                        $"Administrador/{id}",
                        jsonOptions);

                if (administrador == null)
                {
                    MessageBox.Show(
                        "Administrador não encontrado.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    DialogResult =
                        DialogResult.Cancel;

                    Close();

                    return;
                }

                txtNome.Text =
                    administrador.Nome;

                txtCpf.Text =
                    administrador.Cpf;

                txtEmail.Text =
                    administrador.Email;

                txtTelefone.Text =
                    administrador.Telefone;

                chkAtivo.Checked =
                    administrador.Ativo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar o administrador.\n\n" +
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

        private async Task SalvarAdministradorAsync()
        {
            if (string.IsNullOrWhiteSpace(
                txtNome.Text))
            {
                MessageBox.Show(
                    "Informe o nome do administrador.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNome.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtEmail.Text))
            {
                MessageBox.Show(
                    "Informe o e-mail do administrador.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtEmail.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtCpf.Text))
            {
                MessageBox.Show(
                    "Informe o CPF do administrador.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCpf.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtTelefone.Text))
            {
                MessageBox.Show(
                    "Informe o telefone do administrador.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTelefone.Focus();

                return;
            }

            if (!administradorId.HasValue &&
                string.IsNullOrWhiteSpace(
                    txtSenha.Text))
            {
                MessageBox.Show(
                    "Informe a senha do administrador.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSenha.Focus();

                return;
            }

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                btnSalvar.Enabled = false;

                HttpResponseMessage resposta;

                if (administradorId.HasValue)
                {
                    var administrador =
                        new AtualizarAdministradorRequest
                        {
                            Id =
                                administradorId.Value,

                            Nome =
                                txtNome.Text.Trim(),

                            Email =
                                txtEmail.Text.Trim(),

                            Cpf =
                                txtCpf.Text.Trim(),

                            Telefone =
                                txtTelefone.Text.Trim(),

                            Ativo =
                                chkAtivo.Checked
                        };

                    resposta =
                        await httpClient.PutAsJsonAsync(
                            $"Administrador/{administradorId.Value}",
                            administrador);
                }
                else
                {
                    var administrador =
                        new CriarAdministradorRequest
                        {
                            Nome =
                                txtNome.Text.Trim(),

                            Email =
                                txtEmail.Text.Trim(),

                            Cpf =
                                txtCpf.Text.Trim(),

                            Telefone =
                                txtTelefone.Text.Trim(),

                            Senha =
                                txtSenha.Text
                        };

                    resposta =
                        await httpClient.PostAsJsonAsync(
                            "Administrador",
                            administrador);
                }

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível salvar o administrador.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    administradorId.HasValue
                        ? "Administrador atualizado com sucesso!"
                        : "Administrador criado com sucesso!",
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
                    "Erro ao salvar o administrador.\n\n" +
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

        private async Task ExcluirAdministradorAsync()
        {
            if (!administradorId.HasValue)
                return;

            DialogResult confirmacao =
                MessageBox.Show(
                    "Tem certeza que deseja excluir este administrador?\n\n" +
                    "Essa ação não poderá ser desfeita.",
                    "Excluir Administrador",
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
                        $"Administrador/{administradorId.Value}");

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível excluir o administrador.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro ao excluir",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    "Administrador excluído com sucesso!",
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
                    "Erro ao excluir o administrador.\n\n" +
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

        private class CriarAdministradorRequest
        {
            public string Nome { get; set; } =
                string.Empty;

            public string Email { get; set; } =
                string.Empty;

            public string Cpf { get; set; } =
                string.Empty;

            public string Telefone { get; set; } =
                string.Empty;

            public string Senha { get; set; } =
                string.Empty;
        }

        private class AtualizarAdministradorRequest
        {
            public int Id { get; set; }

            public string Nome { get; set; } =
                string.Empty;

            public string Email { get; set; } =
                string.Empty;

            public string Cpf { get; set; } =
                string.Empty;

            public string Telefone { get; set; } =
                string.Empty;

            public bool Ativo { get; set; }
        }

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}