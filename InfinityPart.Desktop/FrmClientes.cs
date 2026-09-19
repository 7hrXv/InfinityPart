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
            using var frm = new Form { Width = 560, Height = 520, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, Text = cliente == null ? "Novo Cliente" : "Editar Cliente" };
            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Theme.AppTheme.Surface };
            var lblTitle = new Guna2HtmlLabel { Text = cliente == null ? "NOVO CLIENTE" : "EDITAR CLIENTE", Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold) };

            var txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = 520, Location = new System.Drawing.Point(20, 50) };
            var txtCpf = new Guna2TextBox { PlaceholderText = "CPF", Width = 520, Location = new System.Drawing.Point(20, 100) };
            var txtEmail = new Guna2TextBox { PlaceholderText = "E-mail", Width = 520, Location = new System.Drawing.Point(20, 150) };
            var txtTelefone = new Guna2TextBox { PlaceholderText = "Telefone", Width = 260, Location = new System.Drawing.Point(20, 200) };
            var txtCep = new Guna2TextBox { PlaceholderText = "CEP", Width = 240, Location = new System.Drawing.Point(300, 200) };
            var txtEndereco = new Guna2TextBox { PlaceholderText = "Endereço", Width = 520, Location = new System.Drawing.Point(20, 250) };
            var txtNumero = new Guna2TextBox { PlaceholderText = "Número", Width = 200, Location = new System.Drawing.Point(20, 300) };
            var txtCidade = new Guna2TextBox { PlaceholderText = "Cidade", Width = 200, Location = new System.Drawing.Point(240, 300) };
            var txtEstado = new Guna2TextBox { PlaceholderText = "Estado", Width = 200, Location = new System.Drawing.Point(460, 300) };

            var btnCancelar = new Guna2Button { Text = "Cancelar", Width = 140, Location = new System.Drawing.Point(260, 360) };
            var btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Location = new System.Drawing.Point(420, 360) };

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
            main.Controls.Add(txtNome);
            main.Controls.Add(txtCpf);
            main.Controls.Add(txtEmail);
            main.Controls.Add(txtTelefone);
            main.Controls.Add(txtCep);
            main.Controls.Add(txtEndereco);
            main.Controls.Add(txtNumero);
            main.Controls.Add(txtCidade);
            main.Controls.Add(txtEstado);
            main.Controls.Add(btnCancelar);
            main.Controls.Add(btnSalvar);

            frm.Controls.Add(main);

            UIHelpers.StyleTextBox(txtNome);
            UIHelpers.StyleTextBox(txtCpf);
            UIHelpers.StyleTextBox(txtEmail);
            UIHelpers.StyleTextBox(txtTelefone);
            UIHelpers.StyleTextBox(txtCep);
            UIHelpers.StyleTextBox(txtEndereco);
            UIHelpers.StyleTextBox(txtNumero);
            UIHelpers.StyleTextBox(txtCidade);
            UIHelpers.StyleTextBox(txtEstado);
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
