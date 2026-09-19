using InfinityPart.Desktop.Models;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    public class FrmPedidos : Form
    {
        private Guna2DataGridView dgv;
        private Guna2TextBox txtSearch;
        private Guna2Button btnNovo;
        private Guna2Button btnEditar;
        private Guna2Button btnExcluir;
        private Guna2Button btnAtualizar;

        private List<PedidoModel> _pedidos = new();
        private List<ClienteModel> _clientes = new();
        private List<StatusPedidoModel> _status = new();

        public FrmPedidos()
        {
            Text = "Pedidos";
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

            var lblTitle = new Guna2HtmlLabel { Text = "GERENCIAR PEDIDOS", Dock = DockStyle.Top, Height = 42, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Theme.AppTheme.TextPrimary, Font = new Font("Segoe UI", 14f, FontStyle.Bold) };
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

            // Rounded visuals
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
            _pedidos = LocalStore.Pedidos.ToList();
            _clientes = LocalStore.Clientes.ToList();
            _status = LocalStore.StatusPedidos.ToList();

            AtualizarGrid(_pedidos);
            return Task.CompletedTask;
        }

        private void AtualizarGrid(List<PedidoModel> list)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();

            var clientePorId = _clientes.ToDictionary(c => c.Id, c => c.Nome);
            var statusPorId = _status.ToDictionary(s => s.Id, s => s.Nome);

            dgv.DataSource = list.Select(p => new
            {
                p.Id,
                Data = p.DataPedido.ToString("dd/MM/yyyy"),
                Cliente = clientePorId.TryGetValue(p.ClienteId, out var n) ? n : $"#{p.ClienteId}",
                Valor = p.ValorTotal,
                Status = statusPorId.TryGetValue(p.StatusPedidoId, out var s) ? s : $"#{p.StatusPedidoId}"
            }).ToList();

            try
            {
                if (dgv.Columns.Contains("Data")) dgv.Columns["Data"].FillWeight = 100;
                if (dgv.Columns.Contains("Cliente")) dgv.Columns["Cliente"].FillWeight = 220;
                if (dgv.Columns.Contains("Valor")) dgv.Columns["Valor"].FillWeight = 100;
                if (dgv.Columns.Contains("Status")) dgv.Columns["Status"].FillWeight = 120;
            }
            catch { }
        }

        private void Filtrar()
        {
            var q = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q))
                AtualizarGrid(_pedidos);
            else
                AtualizarGrid(_pedidos.Where(p => p.Id.ToString().Contains(q) || p.ValorTotal.ToString().Contains(q)).ToList());
        }

        private PedidoModel? Selecionado()
        {
            if (dgv.SelectedRows.Count == 0)
                return null;

            var idObj = dgv.SelectedRows[0].Cells[0].Value;
            if (idObj == null)
                return null;

            if (!int.TryParse(idObj.ToString(), out var id))
                return null;

            return _pedidos.FirstOrDefault(p => p.Id == id);
        }

        private void EditarSelecionado()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione um pedido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MostrarEditor(selecionado);
        }

        private async Task ExcluirSelecionadoAsync()
        {
            var selecionado = Selecionado();
            if (selecionado == null)
            {
                MessageBox.Show("Selecione um pedido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show($"Excluir o pedido #{selecionado.Id}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes)
                return;

            // Exclusão local
            LocalStore.Pedidos.RemoveAll(p => p.Id == selecionado.Id);
            await CarregarAsync();
        }

        private void OnSelectionChanged()
        {
            var has = dgv.SelectedRows.Count > 0;
            btnEditar.Enabled = has;
            btnExcluir.Enabled = has;
        }

        private void MostrarEditor(PedidoModel? pedido)
        {
            using var frm = new Form { Width = 520, Height = 420, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, Text = pedido == null ? "Novo Pedido" : "Editar Pedido" };
            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Theme.AppTheme.Surface };
            var lblTitle = new Guna2HtmlLabel { Text = pedido == null ? "NOVO PEDIDO" : "EDITAR PEDIDO", Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold) };

            var dpData = new DateTimePicker { Width = 240, Location = new System.Drawing.Point(20, 50) };
            var txtValor = new Guna2TextBox { PlaceholderText = "Valor total", Width = 240, Location = new System.Drawing.Point(280, 50) };
            var cbCliente = new ComboBox { Width = 240, Location = new System.Drawing.Point(20, 110), DropDownStyle = ComboBoxStyle.DropDownList };
            var cbStatus = new ComboBox { Width = 240, Location = new System.Drawing.Point(280, 110), DropDownStyle = ComboBoxStyle.DropDownList };

            var btnCancelar = new Guna2Button { Text = "Cancelar", Width = 140, Location = new System.Drawing.Point(220, 180) };
            var btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Location = new System.Drawing.Point(380, 180) };

            foreach (var c in _clientes)
                cbCliente.Items.Add(new ComboBoxItem { Value = c.Id, Text = c.Nome });

            foreach (var s in _status)
                cbStatus.Items.Add(new ComboBoxItem { Value = s.Id, Text = s.Nome });

            if (pedido != null)
            {
                dpData.Value = pedido.DataPedido;
                txtValor.Text = pedido.ValorTotal.ToString();
                cbCliente.SelectedIndex = _clientes.FindIndex(c => c.Id == pedido.ClienteId);
                cbStatus.SelectedIndex = _status.FindIndex(s => s.Id == pedido.StatusPedidoId);
            }

            main.Controls.Add(lblTitle);
            main.Controls.Add(dpData);
            main.Controls.Add(txtValor);
            main.Controls.Add(cbCliente);
            main.Controls.Add(cbStatus);
            main.Controls.Add(btnCancelar);
            main.Controls.Add(btnSalvar);

            frm.Controls.Add(main);

            UIHelpers.StyleTextBox(txtValor);
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
                    var clienteId = cbCliente.SelectedIndex >= 0 ? _clientes[cbCliente.SelectedIndex].Id : 0;
                    var statusId = cbStatus.SelectedIndex >= 0 ? _status[cbStatus.SelectedIndex].Id : 0;
                    var valor = decimal.TryParse(txtValor.Text, out var v) ? v : 0;

                    if (pedido == null)
                    {
                        var novo = new PedidoModel { DataPedido = dpData.Value, ValorTotal = valor, ClienteId = clienteId, StatusPedidoId = statusId };
                        novo.Id = LocalStore.NextPedidoId();
                        LocalStore.Pedidos.Add(novo);
                    }
                    else
                    {
                        pedido.DataPedido = dpData.Value;
                        pedido.ValorTotal = valor;
                        pedido.ClienteId = clienteId;
                        pedido.StatusPedidoId = statusId;
                        var idx = LocalStore.Pedidos.FindIndex(p => p.Id == pedido.Id);
                        if (idx >= 0) LocalStore.Pedidos[idx] = pedido;
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

        // Helper para representar itens nas combo boxes com id
        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; } = string.Empty;
            public override string ToString() => Text;
        }
    }
}
