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
            UIHelpers.RoundControl(btnNovo, 10);
            UIHelpers.RoundControl(btnEditar, 10);
            UIHelpers.RoundControl(btnExcluir, 10);
            UIHelpers.RoundControl(btnAtualizar, 10);

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
            bool editando = produto != null;

            // Layout constants (ajustados para nunca sobrepor/cortar labels e campos)
            const int dialogWidth = 560;
            const int leftX = 24;
            const int rightX = 300;
            const int fieldWidthLeft = 220;
            const int fieldWidthRight = 200;
            const int labelH = 20;
            const int gapLabelField = 8;
            const int fieldH = 34;
            const int rowSpacing = 26;
            const int rowUnit = labelH + gapLabelField + fieldH + rowSpacing;
            const int contentTop = 60;
            const int btnPanelHeight = 64;
            const int gapBeforeButtons = 24;
            const int bottomPadding = 16;

            using var frm = new Form
            {
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Text = editando ? "Editar Produto" : "Novo Produto"
            };

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Theme.AppTheme.Surface };
            var lblTitle = new Guna2HtmlLabel { Text = editando ? "EDITAR PRODUTO" : "NOVO PRODUTO", Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold) };

            int row = 0;

            // ID (somente leitura) - somente ao editar
            Guna2HtmlLabel? lblId = null;
            Guna2TextBox? txtId = null;
            if (produto != null)
            {
                var idY = contentTop + row * rowUnit;
                lblId = new Guna2HtmlLabel { Text = "ID do Produto", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, idY), AutoSize = true };
                txtId = new Guna2TextBox { Text = produto.Id.ToString(), ReadOnly = true, Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, idY + labelH + gapLabelField), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };
                row++;
            }

            // Nome (esquerda) / Preço (direita)
            var nomeY = contentTop + row * rowUnit;
            var lblNome = new Guna2HtmlLabel { Text = "Nome", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, nomeY), AutoSize = true };
            var txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, nomeY + labelH + gapLabelField) };

            var lblPreco = new Guna2HtmlLabel { Text = "Preço", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(rightX, nomeY), AutoSize = true };
            var txtPreco = new Guna2TextBox { PlaceholderText = "Preço", Width = fieldWidthRight, Height = fieldH, Location = new Point(rightX, nomeY + labelH + gapLabelField) };
            row++;

            // Código (esquerda) / Quantidade em estoque (direita)
            var codigoY = contentTop + row * rowUnit;
            var lblCodigo = new Guna2HtmlLabel { Text = "Código", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, codigoY), AutoSize = true };
            var txtCodigo = new Guna2TextBox { PlaceholderText = "Código", Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, codigoY + labelH + gapLabelField) };

            var lblQuantidade = new Guna2HtmlLabel { Text = "Quantidade em Estoque", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(rightX, codigoY), AutoSize = true };
            var txtQuantidade = new Guna2TextBox { PlaceholderText = "Quantidade em estoque", Width = fieldWidthRight, Height = fieldH, Location = new Point(rightX, codigoY + labelH + gapLabelField) };
            row++;

            // MarcaId (largura total)
            var marcaY = contentTop + row * rowUnit;
            var lblMarcaId = new Guna2HtmlLabel { Text = "MarcaId", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, marcaY), AutoSize = true };
            var txtMarcaId = new Guna2TextBox { PlaceholderText = "MarcaId", Width = dialogWidth - leftX * 2, Height = fieldH, Location = new Point(leftX, marcaY + labelH + gapLabelField) };
            row++;

            // Altura do diálogo calculada dinamicamente para nunca cortar campos/botões
            var marcaFieldBottom = marcaY + labelH + gapLabelField + fieldH;
            var clientHeight = marcaFieldBottom + gapBeforeButtons + btnPanelHeight + bottomPadding;
            frm.ClientSize = new Size(dialogWidth, clientHeight);

            // Painel de botões (Salvar/Cancelar) usando FlowLayoutPanel: garante que os botões
            // fiquem sempre totalmente visíveis e alinhados, independente de paddings internos.
            var btnPanel = new Guna2Panel { Dock = DockStyle.Bottom, Height = btnPanelHeight, BackColor = Color.Transparent };
            var btnCancelar = new Guna2Button { Text = "Cancelar", Width = 140, Height = 36 };
            var btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Height = 36 };

            var flBtnEditor = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Padding = new Padding(0, (btnPanelHeight - 36) / 2, 16, 0)
            };
            btnCancelar.Margin = new Padding(0, 0, 12, 0);
            btnSalvar.Margin = new Padding(0);
            flBtnEditor.Controls.Add(btnCancelar);
            flBtnEditor.Controls.Add(btnSalvar);
            btnPanel.Controls.Add(flBtnEditor);

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
            if (produto != null)
            {
                main.Controls.Add(lblId!);
                main.Controls.Add(txtId!);
            }
            main.Controls.Add(lblNome);
            main.Controls.Add(txtNome);
            main.Controls.Add(lblCodigo);
            main.Controls.Add(txtCodigo);
            main.Controls.Add(lblPreco);
            main.Controls.Add(txtPreco);
            main.Controls.Add(lblQuantidade);
            main.Controls.Add(txtQuantidade);
            main.Controls.Add(lblMarcaId);
            main.Controls.Add(txtMarcaId);
            main.Controls.Add(btnPanel);

            frm.Controls.Add(main);

            // Estilos: sem borda branca, fundo #222222, texto branco, cantos arredondados (8)
            foreach (Control c in new Control[] { txtNome, txtCodigo, txtPreco, txtQuantidade, txtMarcaId, txtId! })
            {
                if (c is Guna2TextBox tb)
                {
                    UIHelpers.StyleTextBoxFlat(tb, 10);
                }
            }

            // Validações de digitação (não alteram a lógica de gravação existente)
            Validacoes.SomenteLetras(txtNome);
            Validacoes.SomenteDecimal(txtPreco);
            Validacoes.SomenteNumeros(txtQuantidade);
            Validacoes.SomenteNumeros(txtMarcaId);

            UIHelpers.StyleButton(btnSalvar, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnCancelar, AppTheme.SurfaceAlt, AppTheme.Surface);
            UIHelpers.RoundControl(btnSalvar, 10);
            UIHelpers.RoundControl(btnCancelar, 10);
            UIHelpers.RoundControl(main, 10);

            btnCancelar.Click += (_, _) => frm.Close();

            btnSalvar.Click += async (_, _) =>
            {
                try
                {
                    // Validações obrigatórias antes de salvar (Novo/Editar)
                    if (!Validacoes.ObrigatorioPreenchido(txtNome.Text) || !Validacoes.SomenteLetrasTexto(txtNome.Text))
                    {
                        Validacoes.MostrarErro("Informe um Nome válido (somente letras e espaços).");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtCodigo.Text))
                    {
                        Validacoes.MostrarErro("O campo Código é obrigatório.");
                        return;
                    }

                    if (!Validacoes.DecimalValido(txtPreco.Text, out var precoValidado) || precoValidado < 0)
                    {
                        Validacoes.MostrarErro("Informe um Preço válido (número, podendo ter decimais).");
                        return;
                    }

                    if (!Validacoes.InteiroValido(txtQuantidade.Text, out var quantidadeValidada) || quantidadeValidada < 0)
                    {
                        Validacoes.MostrarErro("Informe uma Quantidade em Estoque válida (somente números inteiros).");
                        return;
                    }

                    if (!Validacoes.InteiroValido(txtMarcaId.Text, out var marcaIdValidado) || marcaIdValidado <= 0)
                    {
                        Validacoes.MostrarErro("Informe um MarcaId válido (somente números).");
                        return;
                    }

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
