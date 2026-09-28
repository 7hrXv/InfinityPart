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
    public class FrmPedidoCadastro : System.Windows.Forms.Form
    {
        private const float LARGURA_REFERENCIA = 900f;
        private const float ALTURA_REFERENCIA = 700f;

        private readonly int? pedidoId;

        private Panel pnlPrincipal = null!;
        private Label lblTitulo = null!;

        private Label lblCliente = null!;
        private Label lblValorTotal = null!;
        private Label lblStatus = null!;
        private Label lblDataPedido = null!;

        private ComboBox cmbCliente = null!;
        private TextBox txtValorTotal = null!;
        private ComboBox cmbStatus = null!;
        private TextBox txtDataPedido = null!;

        private Button btnSalvar = null!;
        private Button btnCancelar = null!;
        private Button btnExcluir = null!;

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

        // =========================================================
        // MODELOS
        // =========================================================

        private class ClienteModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } =
                string.Empty;

            public string Cpf { get; set; } =
                string.Empty;

            public string Email { get; set; } =
                string.Empty;
        }

        private class StatusPedidoModel
        {
            public int Id { get; set; }

            public string Nome { get; set; } =
                string.Empty;
        }

        private class PedidoModel
        {
            public int Id { get; set; }

            public DateTime DataPedido { get; set; }

            public decimal ValorTotal { get; set; }

            public int ClienteId { get; set; }

            public int StatusPedidoId { get; set; }
        }

        private class CriarPedidoRequest
        {
            public int ClienteId { get; set; }

            public decimal ValorTotal { get; set; }
        }

        private class AtualizarPedidoRequest
        {
            public int Id { get; set; }

            public int ClienteId { get; set; }

            public int StatusPedidoId { get; set; }

            public decimal ValorTotal { get; set; }
        }

        // =========================================================
        // CONSTRUTOR
        // =========================================================

        public FrmPedidoCadastro(int? id = null)
        {
            pedidoId = id;

            InicializarFormulario();

            Load += async (_, _) =>
            {
                await CarregarClientesAsync();
                await CarregarStatusAsync();

                if (pedidoId.HasValue)
                {
                    await CarregarPedidoAsync(
                        pedidoId.Value);
                }
                else
                {
                    txtDataPedido.Text =
                        DateTime.Now.ToString(
                            "dd/MM/yyyy HH:mm");
                }
            };
        }

        // =========================================================
        // FORMULÁRIO
        // =========================================================

        private void InicializarFormulario()
        {
            Text =
                pedidoId.HasValue
                    ? "InfinityPart - Editar Pedido"
                    : "InfinityPart - Novo Pedido";

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

            aplicandoEscala = true;

            try
            {
                float escala =
                    ObterEscala();

                int larguraPainel =
                    Math.Max(
                        1,
                        (int)Math.Round(
                            780 * escala));

                int alturaPainel =
                    Math.Max(
                        1,
                        (int)Math.Round(
                            620 * escala));

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

            lblTitulo = new Label
            {
                Text =
                    pedidoId.HasValue
                        ? "Editar Pedido"
                        : "Novo Pedido",

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
            // CLIENTE
            // =====================================================

            lblCliente = new Label
            {
                Text = "Cliente",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        35,
                        105,
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
                lblCliente);

            RegistrarControle(
                lblCliente,
                new Rectangle(
                    35,
                    105,
                    330,
                    25),
                AppTheme.FontSmall.Size);

            cmbCliente = new ComboBox
            {
                Bounds =
                    new Rectangle(
                        35,
                        133,
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

            cmbCliente.DisplayMember =
                "Nome";

            cmbCliente.ValueMember =
                "Id";

            pnlPrincipal.Controls.Add(
                cmbCliente);

            RegistrarControle(
                cmbCliente,
                new Rectangle(
                    35,
                    133,
                    330,
                    45),
                AppTheme.FontBody.Size);

            // =====================================================
            // VALOR TOTAL
            // =====================================================

            CriarCampo(
                "Valor Total",
                ref lblValorTotal,
                ref txtValorTotal,
                395,
                105,
                330);

            // =====================================================
            // STATUS
            // =====================================================

            lblStatus = new Label
            {
                Text = "Status do Pedido",

                AutoSize = false,

                Bounds =
                    new Rectangle(
                        35,
                        210,
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
                lblStatus);

            RegistrarControle(
                lblStatus,
                new Rectangle(
                    35,
                    210,
                    330,
                    25),
                AppTheme.FontSmall.Size);

            cmbStatus = new ComboBox
            {
                Bounds =
                    new Rectangle(
                        35,
                        238,
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

            cmbStatus.DisplayMember =
                "Nome";

            cmbStatus.ValueMember =
                "Id";

            pnlPrincipal.Controls.Add(
                cmbStatus);

            RegistrarControle(
                cmbStatus,
                new Rectangle(
                    35,
                    238,
                    330,
                    45),
                AppTheme.FontBody.Size);

            // =====================================================
            // DATA
            // =====================================================

            CriarCampo(
                "Data do Pedido",
                ref lblDataPedido,
                ref txtDataPedido,
                395,
                210,
                330);

            txtDataPedido.ReadOnly = true;

            // =====================================================
            // CANCELAR
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

            // =====================================================
            // SALVAR
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
                await SalvarPedidoAsync();
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

            // =====================================================
            // EXCLUIR
            // =====================================================

            if (pedidoId.HasValue)
            {
                btnExcluir = new Button
                {
                    Text = "Excluir Pedido",

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
                    await ExcluirPedidoAsync();
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

        // =========================================================
        // CAMPO PADRÃO
        // =========================================================

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

        // =========================================================
        // CARREGAR CLIENTES
        // =========================================================

        private async Task CarregarClientesAsync()
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                List<ClienteModel>? clientes =
                    await httpClient.GetFromJsonAsync<
                        List<ClienteModel>>(
                        "Cliente",
                        jsonOptions);

                cmbCliente.DataSource = null;

                if (clientes == null ||
                    clientes.Count == 0)
                {
                    MessageBox.Show(
                        "Nenhum cliente cadastrado.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                cmbCliente.DataSource =
                    clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar os clientes.\n\n" +
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

        // =========================================================
        // CARREGAR STATUS
        // =========================================================

        private async Task CarregarStatusAsync()
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                List<StatusPedidoModel>? status =
                    await httpClient.GetFromJsonAsync<
                        List<StatusPedidoModel>>(
                        "StatusPedido",
                        jsonOptions);

                cmbStatus.DataSource = null;

                if (status == null ||
                    status.Count == 0)
                {
                    MessageBox.Show(
                        "Nenhum status de pedido cadastrado.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                cmbStatus.DataSource =
                    status;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar os status do pedido.\n\n" +
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

        // =========================================================
        // CARREGAR PEDIDO
        // =========================================================

        private async Task CarregarPedidoAsync(int id)
        {
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                PedidoModel? pedido =
                    await httpClient.GetFromJsonAsync<
                        PedidoModel>(
                        $"Pedido/{id}",
                        jsonOptions);

                if (pedido == null)
                {
                    MessageBox.Show(
                        "Pedido não encontrado.",
                        "InfinityPart",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    DialogResult =
                        DialogResult.Cancel;

                    Close();

                    return;
                }

                txtValorTotal.Text =
                    pedido.ValorTotal.ToString(
                        "0.00");

                txtDataPedido.Text =
                    pedido.DataPedido
                        .ToLocalTime()
                        .ToString(
                            "dd/MM/yyyy HH:mm");

                cmbCliente.SelectedValue =
                    pedido.ClienteId;

                cmbStatus.SelectedValue =
                    pedido.StatusPedidoId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar o pedido.\n\n" +
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

        // =========================================================
        // SALVAR
        // =========================================================

        private async Task SalvarPedidoAsync()
        {
            if (cmbCliente.SelectedValue == null)
            {
                MessageBox.Show(
                    "Selecione um cliente.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCliente.Focus();

                return;
            }

            if (cmbStatus.SelectedValue == null)
            {
                MessageBox.Show(
                    "Selecione um status.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbStatus.Focus();

                return;
            }

            if (!decimal.TryParse(
                txtValorTotal.Text.Replace(
                    ',',
                    '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal valorTotal))
            {
                MessageBox.Show(
                    "Informe um valor total válido.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtValorTotal.Focus();

                return;
            }

            int clienteId =
                Convert.ToInt32(
                    cmbCliente.SelectedValue);

            int statusPedidoId =
                Convert.ToInt32(
                    cmbStatus.SelectedValue);

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                btnSalvar.Enabled = false;

                HttpResponseMessage resposta;

                if (pedidoId.HasValue)
                {
                    AtualizarPedidoRequest pedido =
                        new AtualizarPedidoRequest
                        {
                            Id =
                                pedidoId.Value,

                            ClienteId =
                                clienteId,

                            StatusPedidoId =
                                statusPedidoId,

                            ValorTotal =
                                valorTotal
                        };

                    resposta =
                        await httpClient.PutAsJsonAsync(
                            $"Pedido/{pedidoId.Value}",
                            pedido);
                }
                else
                {
                    CriarPedidoRequest pedido =
                        new CriarPedidoRequest
                        {
                            ClienteId =
                                clienteId,

                            ValorTotal =
                                valorTotal
                        };

                    resposta =
                        await httpClient.PostAsJsonAsync(
                            "Pedido",
                            pedido);
                }

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível salvar o pedido.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    pedidoId.HasValue
                        ? "Pedido atualizado com sucesso!"
                        : "Pedido criado com sucesso!",
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
                    "Erro ao salvar o pedido.\n\n" +
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

        // =========================================================
        // EXCLUIR
        // =========================================================

        private async Task ExcluirPedidoAsync()
        {
            if (!pedidoId.HasValue)
                return;

            DialogResult confirmacao =
                MessageBox.Show(
                    "Tem certeza que deseja excluir este pedido?\n\n" +
                    "Essa ação não poderá ser desfeita.",
                    "Excluir Pedido",
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
                        $"Pedido/{pedidoId.Value}");

                string respostaTexto =
                    await resposta.Content
                        .ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível excluir o pedido.\n\n" +
                        $"Status: {(int)resposta.StatusCode}\n\n" +
                        respostaTexto,
                        "Erro ao excluir",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    "Pedido excluído com sucesso!",
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
                    "Erro ao excluir o pedido.\n\n" +
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
        // FECHAR
        // =========================================================

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}