using InfinityPart.Desktop.Models;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    public class FrmClientes : Form
    {
        private Guna2DataGridView dgv;
        private Guna2TextBox txtSearch;
        private Guna2Button btnNovo;
        private Guna2Button btnEditar;
        private Guna2Button btnExcluir;
        private Guna2Button btnAtualizar;

        private List<ClienteModel> _clientes = new();

        public FrmClientes()
        {
            Text = "Clientes";
            Dock = DockStyle.Fill;

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

            // Title
            var lblTitle = new Guna2HtmlLabel { Text = "GERENCIAR CLIENTES", Dock = DockStyle.Top, Height = 42, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Theme.AppTheme.TextPrimary, Font = new Font("Segoe UI", 14f, FontStyle.Bold) };

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(18) };
            main.Controls.Add(dgv);
            main.Controls.Add(pnlTop);
            main.Controls.Add(lblTitle);

            Controls.Add(main);

            Resize += (_, _) =>
            {
                try
                {
                    var buttonsWidth = flButtons.PreferredSize.Width + flButtons.Margin.Horizontal + flButtons.Padding.Horizontal;
                    var available = ClientSize.Width - buttonsWidth - 80;
                    searchPanel.Width = Math.Max(160, Math.Min(420, available));
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

            UIHelpers.StyleGrid(dgv);
            dgv.ScrollBars = ScrollBars.Vertical;
        }

        private Task CarregarAsync()
        {
            _clientes = LocalStore.Clientes.ToList();
            AtualizarGrid(_clientes);
            return Task.CompletedTask;
        }

        private void AtualizarGrid(List<ClienteModel> list)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();
            dgv.DataSource = list.Select(c => new { c.Id, c.Nome, c.Cpf, c.Email, c.Telefone }).ToList();

            try
            {
                if (dgv.Columns.Contains("Nome")) dgv.Columns["Nome"].FillWeight = 200;
                if (dgv.Columns.Contains("Cpf")) dgv.Columns["Cpf"].FillWeight = 120;
                if (dgv.Columns.Contains("Email")) dgv.Columns["Email"].FillWeight = 160;
                if (dgv.Columns.Contains("Telefone")) dgv.Columns["Telefone"].FillWeight = 100;
            }
            catch { }
        }

        private void Filtrar()
        {
            var q = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q))
                AtualizarGrid(_clientes);
            else
                AtualizarGrid(_clientes.Where(p => p.Nome.ToLower().Contains(q) || p.Cpf.ToLower().Contains(q)).ToList());
        }

        private ClienteModel? Selecionado()
        {
            if (dgv.SelectedRows.Count == 0)
                return null;

            var idObj = dgv.SelectedRows[0].Cells[0].Value;
            if (idObj == null)
                return null;

            if (!int.TryParse(idObj.ToString(), out var id))
                return null;

            return _clientes.FirstOrDefault(p => p.Id == id);
        }

        private void EditarSelecionado()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione um cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MostrarEditor(selecionado);
        }

        private async Task ExcluirSelecionadoAsync()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione um cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show($"Excluir o cliente '{selecionado.Nome}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes)
                return;

            // Exclusão local
            LocalStore.Clientes.RemoveAll(c => c.Id == selecionado.Id);
            await CarregarAsync();
        }

        private void OnSelectionChanged()
        {
            var has = dgv.SelectedRows.Count > 0;
            btnEditar.Enabled = has;
            btnExcluir.Enabled = has;
        }

        private void MostrarEditor(ClienteModel? cliente)
        {
            bool editando = cliente != null;

            // Layout constants (ajustados para nunca sobrepor/cortar labels e campos)
            const int dialogWidth = 560;
            const int leftX = 24;
            const int rightX = 300;
            const int fieldWidthLeft = 220;
            const int fieldWidthRight = 200;
            const int labelH = 20;
            const int gapLabelField = 6;
            const int fieldH = 34;
            const int rowSpacing = 20;
            const int rowUnit = labelH + gapLabelField + fieldH + rowSpacing;
            const int contentTop = 60;
            const int btnPanelHeight = 64;
            const int gapBeforeButtons = 24;
            const int bottomPadding = 16;
            const int rightEdge = rightX + fieldWidthRight;

            using var frm = new Form
            {
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Text = editando ? "Editar Cliente" : "Novo Cliente"
            };

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Theme.AppTheme.Surface };
            var lblTitle = new Guna2HtmlLabel { Text = editando ? "EDITAR CLIENTE" : "NOVO CLIENTE", Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold) };

            int row = 0;

            // ID (somente leitura) - somente ao editar
            Guna2HtmlLabel? lblId = null;
            Guna2TextBox? txtId = null;
            if (cliente != null)
            {
                var idY = contentTop + row * rowUnit;
                lblId = new Guna2HtmlLabel { Text = "ID do Cliente", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, idY), AutoSize = true };
                txtId = new Guna2TextBox { Text = cliente.Id.ToString(), ReadOnly = true, Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, idY + labelH + gapLabelField) };
                row++;
            }

            // Nome (esquerda) / CPF-CNPJ (direita)
            var nomeY = contentTop + row * rowUnit;
            var lblNome = new Guna2HtmlLabel { Text = "Nome", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, nomeY), AutoSize = true };
            var txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, nomeY + labelH + gapLabelField) };

            var lblCpf = new Guna2HtmlLabel { Text = "CPF/CNPJ", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(rightX, nomeY), AutoSize = true };
            var txtCpf = new Guna2TextBox { PlaceholderText = "CPF/CNPJ", Width = fieldWidthRight, Height = fieldH, Location = new Point(rightX, nomeY + labelH + gapLabelField) };
            row++;

            // E-mail (esquerda) / Telefone (direita)
            var emailY = contentTop + row * rowUnit;
            var lblEmail = new Guna2HtmlLabel { Text = "E-mail", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, emailY), AutoSize = true };
            var txtEmail = new Guna2TextBox { PlaceholderText = "E-mail", Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, emailY + labelH + gapLabelField) };

            var lblTelefone = new Guna2HtmlLabel { Text = "Telefone", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(rightX, emailY), AutoSize = true };
            var txtTelefone = new Guna2TextBox { PlaceholderText = "Telefone", Width = fieldWidthRight, Height = fieldH, Location = new Point(rightX, emailY + labelH + gapLabelField) };
            row++;

            // CEP (estreito) / Endereço (largo, ocupa o restante da linha)
            var cepY = contentTop + row * rowUnit;
            const int cepWidth = 110;
            const int enderecoX = leftX + 130;
            var lblCep = new Guna2HtmlLabel { Text = "CEP", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, cepY), AutoSize = true };
            var txtCep = new Guna2TextBox { PlaceholderText = "CEP", Width = cepWidth, Height = fieldH, Location = new Point(leftX, cepY + labelH + gapLabelField) };

            var lblEndereco = new Guna2HtmlLabel { Text = "Endereço", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(enderecoX, cepY), AutoSize = true };
            var txtEndereco = new Guna2TextBox { PlaceholderText = "Endereço", Width = rightEdge - enderecoX, Height = fieldH, Location = new Point(enderecoX, cepY + labelH + gapLabelField) };
            row++;

            // Número (estreito) / Cidade / Estado
            var numeroY = contentTop + row * rowUnit;
            const int numeroWidth = 110;
            const int cidadeX = leftX + 130;
            const int cidadeWidth = 150;
            const int estadoX = leftX + 300;
            var lblNumero = new Guna2HtmlLabel { Text = "Número", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, numeroY), AutoSize = true };
            var txtNumero = new Guna2TextBox { PlaceholderText = "Número", Width = numeroWidth, Height = fieldH, Location = new Point(leftX, numeroY + labelH + gapLabelField) };

            var lblCidade = new Guna2HtmlLabel { Text = "Cidade", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(cidadeX, numeroY), AutoSize = true };
            var txtCidade = new Guna2TextBox { PlaceholderText = "Cidade", Width = cidadeWidth, Height = fieldH, Location = new Point(cidadeX, numeroY + labelH + gapLabelField) };

            var lblEstado = new Guna2HtmlLabel { Text = "Estado", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(estadoX, numeroY), AutoSize = true };
            var txtEstado = new Guna2TextBox { PlaceholderText = "Estado", Width = rightEdge - estadoX, Height = fieldH, Location = new Point(estadoX, numeroY + labelH + gapLabelField) };
            row++;

            // Altura do diálogo calculada dinamicamente para nunca cortar campos/botões
            var lastFieldBottom = numeroY + labelH + gapLabelField + fieldH;
            var clientHeight = lastFieldBottom + gapBeforeButtons + btnPanelHeight + bottomPadding;
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

            if (cliente != null)
            {
                txtNome.Text = cliente.Nome;
                txtCpf.Text = cliente.Cpf;
                txtEmail.Text = cliente.Email;
                txtTelefone.Text = cliente.Telefone;
                txtCep.Text = cliente.Cep;
                txtEndereco.Text = cliente.Endereco;
                txtNumero.Text = cliente.Numero;
                txtCidade.Text = cliente.Cidade;
                txtEstado.Text = cliente.Estado;
            }

            main.Controls.Add(lblTitle);
            if (cliente != null)
            {
                main.Controls.Add(lblId!);
                main.Controls.Add(txtId!);
            }
            main.Controls.Add(lblNome);
            main.Controls.Add(txtNome);
            main.Controls.Add(lblCpf);
            main.Controls.Add(txtCpf);
            main.Controls.Add(lblEmail);
            main.Controls.Add(txtEmail);
            main.Controls.Add(lblTelefone);
            main.Controls.Add(txtTelefone);
            main.Controls.Add(lblCep);
            main.Controls.Add(txtCep);
            main.Controls.Add(lblEndereco);
            main.Controls.Add(txtEndereco);
            main.Controls.Add(lblNumero);
            main.Controls.Add(txtNumero);
            main.Controls.Add(lblCidade);
            main.Controls.Add(txtCidade);
            main.Controls.Add(lblEstado);
            main.Controls.Add(txtEstado);
            main.Controls.Add(btnPanel);

            frm.Controls.Add(main);

            // Estilos: sem borda branca, fundo #222222, texto branco, cantos arredondados (8)
            foreach (Control c in new Control[] { txtNome, txtCpf, txtEmail, txtTelefone, txtCep, txtEndereco, txtNumero, txtCidade, txtEstado, txtId! })
            {
                if (c is Guna2TextBox tb)
                {
                    UIHelpers.StyleTextBoxFlat(tb, 10);
                }
            }

            // Restrições de digitação (não alteram a lógica de gravação existente)
            Validacoes.SomenteLetras(txtNome);
            Validacoes.SomenteNumeros(txtCpf);
            Validacoes.SomenteNumeros(txtTelefone);
            Validacoes.SomenteNumeros(txtCep);
            Validacoes.SomenteNumeros(txtNumero);
            Validacoes.SomenteLetras(txtCidade);
            Validacoes.SomenteLetras(txtEstado);

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

                    if (!Validacoes.ObrigatorioPreenchido(txtCpf.Text) || !Validacoes.SomenteNumerosTexto(txtCpf.Text))
                    {
                        Validacoes.MostrarErro("Informe um CPF/CNPJ válido (somente números).");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtEmail.Text) || !Validacoes.EmailValido(txtEmail.Text))
                    {
                        Validacoes.MostrarErro("Informe um E-mail em um formato válido.");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtTelefone.Text) || !Validacoes.SomenteNumerosTexto(txtTelefone.Text))
                    {
                        Validacoes.MostrarErro("Informe um Telefone válido (somente números).");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtCep.Text) || !Validacoes.SomenteNumerosTexto(txtCep.Text))
                    {
                        Validacoes.MostrarErro("Informe um CEP válido (somente números).");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtEndereco.Text))
                    {
                        Validacoes.MostrarErro("O campo Endereço é obrigatório.");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtNumero.Text) || !Validacoes.SomenteNumerosTexto(txtNumero.Text))
                    {
                        Validacoes.MostrarErro("Informe um Número válido (somente números).");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtCidade.Text) || !Validacoes.SomenteLetrasTexto(txtCidade.Text))
                    {
                        Validacoes.MostrarErro("Informe uma Cidade válida (somente letras e espaços).");
                        return;
                    }

                    if (!Validacoes.ObrigatorioPreenchido(txtEstado.Text) || !Validacoes.SomenteLetrasTexto(txtEstado.Text))
                    {
                        Validacoes.MostrarErro("Informe um Estado válido (somente letras e espaços).");
                        return;
                    }

                    if (cliente == null)
                    {
                        var criar = new CriarClienteModel
                        {
                            Nome = txtNome.Text.Trim(),
                            Cpf = txtCpf.Text.Trim(),
                            Email = txtEmail.Text.Trim(),
                            Telefone = txtTelefone.Text.Trim(),
                            Cep = txtCep.Text.Trim(),
                            Endereco = txtEndereco.Text.Trim(),
                            Numero = txtNumero.Text.Trim(),
                            Cidade = txtCidade.Text.Trim(),
                            Estado = txtEstado.Text.Trim()
                        };

                        var novo = new ClienteModel
                        {
                            Id = LocalStore.NextClienteId(),
                            Nome = criar.Nome,
                            Cpf = criar.Cpf,
                            Email = criar.Email,
                            Telefone = criar.Telefone,
                            Cep = criar.Cep,
                            Endereco = criar.Endereco,
                            Numero = criar.Numero,
                            Cidade = criar.Cidade,
                            Estado = criar.Estado
                        };

                        LocalStore.Clientes.Add(novo);
                    }
                    else
                    {
                        cliente.Nome = txtNome.Text.Trim();
                        cliente.Cpf = txtCpf.Text.Trim();
                        cliente.Email = txtEmail.Text.Trim();
                        cliente.Telefone = txtTelefone.Text.Trim();
                        cliente.Cep = txtCep.Text.Trim();
                        cliente.Endereco = txtEndereco.Text.Trim();
                        cliente.Numero = txtNumero.Text.Trim();
                        cliente.Cidade = txtCidade.Text.Trim();
                        cliente.Estado = txtEstado.Text.Trim();

                        var idx = LocalStore.Clientes.FindIndex(c => c.Id == cliente.Id);
                        if (idx >= 0) LocalStore.Clientes[idx] = cliente;
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
