using InfinityPart.Desktop.Models;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    public class FrmMarcas : Form
    {
        private Guna2DataGridView dgv;
        private Guna2TextBox txtSearch;
        private Guna2Button btnNovo;
        private Guna2Button btnEditar;
        private Guna2Button btnExcluir;
        private Guna2Button btnAtualizar;

        private List<MarcaModel> _marcas = new();

        public FrmMarcas()
        {
            Text = "Marcas";
            Dock = DockStyle.Fill;

            // Top layout modeled after FrmProdutos
            var pnlTop = new Guna2Panel { Dock = DockStyle.Top, Height = 64, Padding = new Padding(8), BackColor = Color.Transparent };
            var searchPanel = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(6) };
            var searchInner = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(6) };
            searchInner.BackColor = Color.FromArgb(200, Theme.AppTheme.SurfaceAlt);

            var picLupa = UIHelpers.CreateSearchIcon(AppTheme.TextMuted, 16);
            txtSearch = new Guna2TextBox { PlaceholderText = "Pesquisar...", Dock = DockStyle.Fill, BorderStyle = BorderStyle.None };
            txtSearch.Margin = new Padding(6);

            // same order as FrmProdutos: add textbox then icon (styling helper will place the icon visually)
            searchInner.Controls.Add(txtSearch);
            searchInner.Controls.Add(picLupa);
            searchPanel.Controls.Add(searchInner);

            var flButtons = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.LeftToRight, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = false, Padding = new Padding(6), Height = 48 };
            btnNovo = new Guna2Button { Text = "Novo", Width = 110, Height = 38, Margin = new Padding(8, 6, 8, 6) };
            btnEditar = new Guna2Button { Text = "Editar", Width = 110, Height = 38, Margin = new Padding(8, 6, 8, 6), Enabled = false };
            btnExcluir = new Guna2Button { Text = "Excluir", Width = 110, Height = 38, Margin = new Padding(8, 6, 8, 6), Enabled = false };
            btnAtualizar = new Guna2Button { Text = "Atualizar", Width = 110, Height = 38, Margin = new Padding(8, 6, 8, 6) };
            flButtons.Controls.Add(btnAtualizar);
            flButtons.Controls.Add(btnExcluir);
            flButtons.Controls.Add(btnEditar);
            flButtons.Controls.Add(btnNovo);
            pnlTop.Controls.Add(searchPanel);
            pnlTop.Controls.Add(flButtons);

            dgv = new Guna2DataGridView { Dock = DockStyle.Fill };

            var lblTitle = new Guna2HtmlLabel
            {
                Text = "GERENCIAR MARCAS",
                Dock = DockStyle.Top,
                Height = 42,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Theme.AppTheme.TextPrimary,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold)
            };

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(18), BackColor = AppTheme.Background };
            // match FrmProdutos order: dgv, top panel, title so docking behaves the same
            main.Controls.Add(dgv);
            main.Controls.Add(pnlTop);
            main.Controls.Add(lblTitle);

            Controls.Add(main);

            // Responsividade: ajustar largura do painel de pesquisa ao redimensionar (mesmo comportamento do FrmProdutos)
            Resize += (_, _) =>
            {
                try
                {
                    var buttonsWidth = flButtons.PreferredSize.Width + flButtons.Margin.Horizontal + flButtons.Padding.Horizontal;
                    var available = ClientSize.Width - buttonsWidth - 80; // reservar margens
                    searchPanel.Width = Math.Max(180, Math.Min(420, available));
                }
                catch { }
            };

            // Rounded search and buttons
            UIHelpers.RoundControl(searchInner, 10);
            UIHelpers.RoundControl(btnNovo, 10);
            UIHelpers.RoundControl(btnEditar, 10);
            UIHelpers.RoundControl(btnExcluir, 10);
            UIHelpers.RoundControl(btnAtualizar, 10);

            Load += (_, _) => _ = CarregarAsync();
            btnAtualizar.Click += (_, _) => _ = CarregarAsync();
            btnNovo.Click += (_, _) => MostrarEditor(null);
            btnEditar.Click += (_, _) => EditarSelecionado();
            btnExcluir.Click += async (_, _) => await ExcluirSelecionadoAsync();
            txtSearch.TextChanged += (_, _) => Filtrar();

            dgv.SelectionChanged += (_, _) => OnSelectionChanged();

            // Estilo visual consistente
            BackColor = Theme.AppTheme.Background;
            Font = Theme.AppTheme.FontBody;

            UIHelpers.StyleTextBox(txtSearch);
            UIHelpers.StyleButton(btnNovo, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnEditar, AppTheme.PrimaryRedHover, AppTheme.PrimaryRed);
            UIHelpers.StyleButton(btnExcluir, AppTheme.PrimaryRedDark, AppTheme.PrimaryRed);
            UIHelpers.StyleButton(btnAtualizar, AppTheme.SurfaceAlt, AppTheme.Surface);

            // Ensure grid styles
            UIHelpers.StyleGrid(dgv);
            dgv.ScrollBars = ScrollBars.Vertical;

            UIHelpers.StyleGrid(dgv);
            dgv.ScrollBars = ScrollBars.Vertical;
        }

        private Task CarregarAsync()
        {
            _marcas = LocalStore.Marcas.ToList();
            AtualizarGrid(_marcas);
            return Task.CompletedTask;
        }

        private void AtualizarGrid(List<MarcaModel> list)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();
            dgv.DataSource = list.Select(m => new { m.Id, m.Nome, m.Cnpj }).ToList();

            try
            {
                if (dgv.Columns.Contains("Nome")) dgv.Columns["Nome"].FillWeight = 200;
                if (dgv.Columns.Contains("Cnpj")) dgv.Columns["Cnpj"].FillWeight = 160;
            }
            catch { }
        }

        private void Filtrar()
        {
            var q = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q))
                AtualizarGrid(_marcas);
            else
                AtualizarGrid(_marcas.Where(p => p.Nome.ToLower().Contains(q)).ToList());
        }

        private MarcaModel? Selecionado()
        {
            if (dgv.SelectedRows.Count == 0)
                return null;

            var idObj = dgv.SelectedRows[0].Cells[0].Value;
            if (idObj == null)
                return null;

            if (!int.TryParse(idObj.ToString(), out var id))
                return null;

            return _marcas.FirstOrDefault(p => p.Id == id);
        }

        private void EditarSelecionado()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione uma marca.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MostrarEditor(selecionado);
        }

        private async Task ExcluirSelecionadoAsync()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione uma marca.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show($"Excluir a marca '{selecionado.Nome}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes)
                return;

            // Exclusão local
            LocalStore.Marcas.RemoveAll(m => m.Id == selecionado.Id);
            await CarregarAsync();
        }

        private void OnSelectionChanged()
        {
            var has = dgv.SelectedRows.Count > 0;
            btnEditar.Enabled = has;
            btnExcluir.Enabled = has;
        }

        private void MostrarEditor(MarcaModel? marca)
        {
            // Investigar: layout anterior estava calculando posições manualmente e
            // misturando Dock = Top para o título com posições absolutas para os
            // campos. Para tornar o layout previsível e evitar quebras causadas
            // por AutoSize/AutoLayout, usamos um container simples (FlowLayoutPanel)
            // em fluxo vertical e um painel dedicado para botões centralizados.

            const int dialogWidth = 500;
            const int dialogHeight = 390;
            const int outerPad = 16;
            const int labelH = 20;
            const int gapLabelField = 6;
            const int fieldH = 34;
            const int verticalSpacingBetweenFields = 16; // mais espaço vertical entre campos
            const int btnPanelHeight = 64;

            using var frm = new Form { StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, Text = marca == null ? "Nova Marca" : "Editar Marca" };
            frm.ClientSize = new Size(dialogWidth, dialogHeight);

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(outerPad), BackColor = Theme.AppTheme.Surface };

            var lblTitle = new Guna2HtmlLabel
            {
                Text = marca == null ? "NOVA MARCA" : "EDITAR MARCA",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Height = 36,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Top
            };

            // Container vertical para labels+campos, facilita espaçamento uniforme
            var fields = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = false,
                Location = new Point(outerPad, lblTitle.Height + outerPad),
                Size = new Size(frm.ClientSize.Width - outerPad * 2, frm.ClientSize.Height - lblTitle.Height - btnPanelHeight - outerPad * 3),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // ID (opcional)
            Guna2TextBox? txtId = null;
            if (marca != null)
            {
                var lblId = new Guna2HtmlLabel { Text = "ID da Marca", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true };
                txtId = new Guna2TextBox { Text = marca.Id.ToString(), ReadOnly = true, Width = fields.Width, Height = fieldH, BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };
                lblId.Margin = new Padding(0, 0, 0, gapLabelField);
                txtId.Margin = new Padding(0, 0, 0, verticalSpacingBetweenFields);
                fields.Controls.Add(lblId);
                fields.Controls.Add(txtId);
            }

            // Nome
            var lblNome = new Guna2HtmlLabel { Text = "Nome", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true };
            var txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = fields.Width, Height = fieldH, ReadOnly = false, Enabled = true };
            lblNome.Margin = new Padding(0, 0, 0, gapLabelField);
            txtNome.Margin = new Padding(0, 0, 0, verticalSpacingBetweenFields);
            fields.Controls.Add(lblNome);
            fields.Controls.Add(txtNome);

            // CNPJ
            var lblCnpj = new Guna2HtmlLabel { Text = "CNPJ", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true };
            var txtCnpj = new Guna2TextBox { PlaceholderText = "CNPJ", Width = fields.Width, Height = fieldH, ReadOnly = false, Enabled = true };
            lblCnpj.Margin = new Padding(0, 0, 0, gapLabelField);
            txtCnpj.Margin = new Padding(0, 0, 0, verticalSpacingBetweenFields + 6);
            fields.Controls.Add(lblCnpj);
            fields.Controls.Add(txtCnpj);

            // Botões centralizados na parte inferior
            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = btnPanelHeight, BackColor = Color.Transparent };
            var flButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = false, Height = 36 };
            var btnCancelar = new Guna2Button { Text = "Cancelar", Width = 140, Height = 36 };
            var btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Height = 36 };
            flButtons.Controls.Add(btnCancelar);
            flButtons.Controls.Add(btnSalvar);
            // centralizar os botões dentro do painel de botões (usar dimensões do painel)
            btnPanel.Controls.Add(flButtons);
            flButtons.Width = btnCancelar.Width + btnSalvar.Width + 12; // 12px gap
            flButtons.Left = Math.Max(0, (btnPanel.ClientSize.Width - flButtons.Width) / 2);
            flButtons.Top = Math.Max(0, (btnPanel.Height - flButtons.Height) / 2);
            flButtons.Anchor = AnchorStyles.None;

            if (marca != null)
            {
                txtNome.Text = marca.Nome;
                txtCnpj.Text = marca.Cnpj;
            }

            main.Controls.Add(lblTitle);
            main.Controls.Add(fields);
            main.Controls.Add(btnPanel);
            frm.Controls.Add(main);

            // Aplica estilo sem alterar comportamento
            foreach (Control c in new Control[] { txtNome, txtCnpj, txtId })
            {
                if (c is Guna2TextBox tb)
                {
                    UIHelpers.StyleTextBoxFlat(tb, 10);
                    UIHelpers.RoundControl(tb, 10);
                }
            }

            UIHelpers.StyleButton(btnSalvar, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnCancelar, AppTheme.SurfaceAlt, AppTheme.Surface);
            UIHelpers.RoundControl(btnSalvar, 10);
            UIHelpers.RoundControl(btnCancelar, 10);
            UIHelpers.RoundControl(main, 10);

            // Eventos de ação mantidos
            btnCancelar.Click += (_, _) => frm.Close();

            btnSalvar.Click += async (_, _) =>
            {
                try
                {
                    if (marca == null)
                    {
                        var criar = new CriarMarcaModel { Nome = txtNome.Text.Trim(), Cnpj = txtCnpj.Text.Trim() };
                        var novo = new MarcaModel { Id = LocalStore.NextMarcaId(), Nome = criar.Nome, Cnpj = criar.Cnpj };
                        LocalStore.Marcas.Add(novo);
                    }
                    else
                    {
                        marca.Nome = txtNome.Text.Trim();
                        marca.Cnpj = txtCnpj.Text.Trim();
                        var idx = LocalStore.Marcas.FindIndex(m => m.Id == marca.Id);
                        if (idx >= 0) LocalStore.Marcas[idx] = marca;
                    }

                    frm.Close();
                    await CarregarAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            };

            frm.ShowDialog();
        }
    }
}
