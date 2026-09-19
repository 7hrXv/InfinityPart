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
            UIHelpers.RoundControl(btnNovo, 6);
            UIHelpers.RoundControl(btnEditar, 6);
            UIHelpers.RoundControl(btnExcluir, 6);
            UIHelpers.RoundControl(btnAtualizar, 6);

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
            using var frm = new Form { Width = 480, Height = 300, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, Text = marca == null ? "Nova Marca" : "Editar Marca" };
            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Theme.AppTheme.Surface };
            var lblTitle = new Guna2HtmlLabel { Text = marca == null ? "NOVA MARCA" : "EDITAR MARCA", Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold) };

            var txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = 420, Location = new System.Drawing.Point(20, 50) };
            var txtCnpj = new Guna2TextBox { PlaceholderText = "CNPJ", Width = 420, Location = new System.Drawing.Point(20, 100) };

            var btnCancelar = new Guna2Button { Text = "Cancelar", Width = 140, Location = new System.Drawing.Point(200, 170) };
            var btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Location = new System.Drawing.Point(360, 170) };

            if (marca != null)
            {
                txtNome.Text = marca.Nome;
                txtCnpj.Text = marca.Cnpj;
            }

            main.Controls.Add(lblTitle);
            main.Controls.Add(txtNome);
            main.Controls.Add(txtCnpj);
            main.Controls.Add(btnCancelar);
            main.Controls.Add(btnSalvar);

            frm.Controls.Add(main);

            UIHelpers.StyleTextBox(txtNome);
            UIHelpers.StyleTextBox(txtCnpj);
            UIHelpers.StyleButton(btnSalvar, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnCancelar, AppTheme.SurfaceAlt, AppTheme.Surface);
            UIHelpers.RoundControl(main, 8);
            UIHelpers.RoundControl(btnSalvar, 6);
            UIHelpers.RoundControl(btnCancelar, 6);

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
