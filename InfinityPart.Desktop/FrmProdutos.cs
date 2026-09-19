using InfinityPart.Desktop.Models;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    public class FrmProdutos : Form
    {
        private Guna2DataGridView dgv;
        private Guna2TextBox txtSearch;
        private Guna2Button btnNovo;
        private Guna2Button btnEditar;
        private Guna2Button btnExcluir;
        private Guna2Button btnAtualizar;

        private List<ProdutoModel> _produtos = new();

        public FrmProdutos()
        {
            Text = "Produtos";
            Dock = DockStyle.Fill;

            // Main container
            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(18) };

            // Title
            var lblTitle = new Guna2HtmlLabel
            {
                Text = "GERENCIAR PRODUTOS",
                Dock = DockStyle.Top,
                Height = 42,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Theme.AppTheme.TextPrimary,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold)
            };

            // Top area: search (left) + buttons (right)
            var pnlTop = new Guna2Panel { Dock = DockStyle.Top, Height = 64, Padding = new Padding(8), BackColor = Color.Transparent };

            var searchPanel = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(6) };
            var searchInner = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(6) };
            searchInner.BackColor = Color.FromArgb(200, Theme.AppTheme.SurfaceAlt);

            var picLupa = UIHelpers.CreateSearchIcon(AppTheme.TextMuted, 16);
            txtSearch = new Guna2TextBox { PlaceholderText = "Pesquisar...", Dock = DockStyle.Fill, BorderStyle = BorderStyle.None };
            txtSearch.Margin = new Padding(6);

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

            main.Controls.Add(dgv);
            main.Controls.Add(pnlTop);
            main.Controls.Add(lblTitle);

            Controls.Add(main);

            // Responsividade: ajustar largura do painel de pesquisa ao redimensionar
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

            Load += (_, _) => _ = CarregarAsync();
            btnAtualizar.Click += (_, _) => _ = CarregarAsync();
            btnNovo.Click += (_, _) => MostrarEditor(null);
            btnEditar.Click += (_, _) => EditarSelecionado();
            btnExcluir.Click += async (_, _) => await ExcluirSelecionadoAsync();
            txtSearch.TextChanged += (_, _) => Filtrar();

            dgv.SelectionChanged += (_, _) => OnSelectionChanged();

            // Grid básico
            // Estilo visual consistente
            BackColor = Theme.AppTheme.Background;
            Font = Theme.AppTheme.FontBody;

            UIHelpers.StyleTextBox(txtSearch);
            UIHelpers.StyleButton(btnNovo, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnEditar, AppTheme.PrimaryRedHover, AppTheme.PrimaryRed);
            UIHelpers.StyleButton(btnExcluir, AppTheme.PrimaryRedDark, AppTheme.PrimaryRed);
            UIHelpers.StyleButton(btnAtualizar, AppTheme.SurfaceAlt, AppTheme.Surface);

            // Rounded search box and buttons
            UIHelpers.RoundControl(searchInner, 10);
            UIHelpers.RoundControl(btnNovo, 6);
            UIHelpers.RoundControl(btnEditar, 6);
            UIHelpers.RoundControl(btnExcluir, 6);
            UIHelpers.RoundControl(btnAtualizar, 6);

            UIHelpers.StyleGrid(dgv);
            dgv.ScrollBars = ScrollBars.Vertical;
        }

        private Task CarregarAsync()
        {
            // Carrega localmente sem usar a API
            _produtos = LocalStore.Produtos.ToList();
            AtualizarGrid(_produtos);
            return Task.CompletedTask;
        }

        private void AtualizarGrid(List<ProdutoModel> list)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();
            dgv.DataSource = list.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Codigo,
                p.Preco,
                p.QuantidadeEstoque,
                p.MarcaId
            }).ToList();

            // Ajuste de colunas para evitar corte e melhorar responsividade
            try
            {
                if (dgv.Columns.Contains("Nome")) dgv.Columns["Nome"].FillWeight = 200;
                if (dgv.Columns.Contains("Codigo")) dgv.Columns["Codigo"].FillWeight = 100;
                if (dgv.Columns.Contains("Preco")) dgv.Columns["Preco"].FillWeight = 80;
                if (dgv.Columns.Contains("QuantidadeEstoque")) dgv.Columns["QuantidadeEstoque"].FillWeight = 100;
                if (dgv.Columns.Contains("MarcaId")) dgv.Columns["MarcaId"].FillWeight = 60;
            }
            catch { }
        }

        private void Filtrar()
        {
            var q = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q))
                AtualizarGrid(_produtos);
            else
                AtualizarGrid(_produtos.Where(p => p.Nome.ToLower().Contains(q) || p.Codigo.ToLower().Contains(q)).ToList());
        }

        private ProdutoModel? Selecionado()
        {
            if (dgv.SelectedRows.Count == 0)
                return null;

            var idObj = dgv.SelectedRows[0].Cells[0].Value;
            if (idObj == null)
                return null;

            if (!int.TryParse(idObj.ToString(), out var id))
                return null;

            return _produtos.FirstOrDefault(p => p.Id == id);
        }

        private void EditarSelecionado()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione um produto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MostrarEditor(selecionado);
        }

        private async Task ExcluirSelecionadoAsync()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione um produto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show($"Excluir o produto '{selecionado.Nome}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes)
                return;

            // Exclusão local
            LocalStore.Produtos.RemoveAll(p => p.Id == selecionado.Id);
            await CarregarAsync();
        }

        private void OnSelectionChanged()
        {
            var has = dgv.SelectedRows.Count > 0;
            btnEditar.Enabled = has;
            btnExcluir.Enabled = has;
        }

        private void MostrarEditor(ProdutoModel? produto)
        {
            using var frm = new Form { Width = 520, Height = 420, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, Text = produto == null ? "Novo Produto" : "Editar Produto" };
            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Theme.AppTheme.Surface };
            var lblTitle = new Guna2HtmlLabel { Text = produto == null ? "NOVO PRODUTO" : "EDITAR PRODUTO", Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold) };

            var txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = 440, Location = new System.Drawing.Point(20, 50) };
            var txtCodigo = new Guna2TextBox { PlaceholderText = "Código", Width = 440, Location = new System.Drawing.Point(20, 100) };
            var txtPreco = new Guna2TextBox { PlaceholderText = "Preço", Width = 220, Location = new System.Drawing.Point(20, 150) };
            var txtQuantidade = new Guna2TextBox { PlaceholderText = "Quantidade em estoque", Width = 200, Location = new System.Drawing.Point(260, 150) };
            var txtMarcaId = new Guna2TextBox { PlaceholderText = "MarcaId", Width = 440, Location = new System.Drawing.Point(20, 200) };

            var btnCancelar = new Guna2Button { Text = "Cancelar", Width = 140, Location = new System.Drawing.Point(220, 260) };
            var btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Location = new System.Drawing.Point(380, 260) };

            // preencher valores se editar
            if (produto != null)
            {
                txtNome.Text = produto.Nome;
                txtCodigo.Text = produto.Codigo;
                txtPreco.Text = produto.Preco.ToString();
                txtQuantidade.Text = produto.QuantidadeEstoque.ToString();
                txtMarcaId.Text = produto.MarcaId.ToString();
            }

            main.Controls.Add(lblTitle);
            main.Controls.Add(txtNome);
            main.Controls.Add(txtCodigo);
            main.Controls.Add(txtPreco);
            main.Controls.Add(txtQuantidade);
            main.Controls.Add(txtMarcaId);
            main.Controls.Add(btnCancelar);
            main.Controls.Add(btnSalvar);

            frm.Controls.Add(main);

            // estilos
            UIHelpers.StyleTextBox(txtNome);
            UIHelpers.StyleTextBox(txtCodigo);
            UIHelpers.StyleTextBox(txtPreco);
            UIHelpers.StyleTextBox(txtQuantidade);
            UIHelpers.StyleTextBox(txtMarcaId);
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
                    if (produto == null)
                    {
                        var criar = new CriarProdutoModel
                        {
                            Nome = txtNome.Text.Trim(),
                            Codigo = txtCodigo.Text.Trim(),
                            Preco = decimal.TryParse(txtPreco.Text, out var p) ? p : 0,
                            QuantidadeEstoque = int.TryParse(txtQuantidade.Text, out var q) ? q : 0,
                            MarcaId = int.TryParse(txtMarcaId.Text, out var m) ? m : 0
                        };

                        var novoProduto = new ProdutoModel
                        {
                            Id = LocalStore.NextProdutoId(),
                            Nome = criar.Nome,
                            Codigo = criar.Codigo,
                            Descricao = criar.Descricao,
                            Preco = criar.Preco,
                            QuantidadeEstoque = criar.QuantidadeEstoque,
                            MarcaId = criar.MarcaId
                        };

                        LocalStore.Produtos.Add(novoProduto);
                    }
                    else
                    {
                        produto.Nome = txtNome.Text.Trim();
                        produto.Codigo = txtCodigo.Text.Trim();
                        produto.Preco = decimal.TryParse(txtPreco.Text, out var p2) ? p2 : produto.Preco;
                        produto.QuantidadeEstoque = int.TryParse(txtQuantidade.Text, out var q2) ? q2 : produto.QuantidadeEstoque;
                        produto.MarcaId = int.TryParse(txtMarcaId.Text, out var m2) ? m2 : produto.MarcaId;

                        var idx = LocalStore.Produtos.FindIndex(p => p.Id == produto.Id);
                        if (idx >= 0) LocalStore.Produtos[idx] = produto;
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
