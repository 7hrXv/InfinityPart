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
            // Alinha visualmente ao padrão do Editar Marca: layout compacto, espaçamento uniforme
            const int dialogWidth = 520;
            const int dialogHeight = 360;
            const int outerPad = 16;
            const int fieldH = 34;
            const int labelH = 20;
            const int gapLabelField = 6;
            const int verticalSpacing = 16;
            const int btnPanelHeight = 64;

            using var frm = new Form { StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, Text = pedido == null ? "Novo Pedido" : "Editar Pedido" };
            frm.ClientSize = new Size(dialogWidth, dialogHeight);

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(outerPad), BackColor = Theme.AppTheme.Surface };
            var lblTitle = new Guna2HtmlLabel { Text = pedido == null ? "NOVO PEDIDO" : "EDITAR PEDIDO", ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold), Height = 36, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Top };

            var fields = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = false, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            fields.Location = new Point(outerPad, lblTitle.Height + outerPad);
            fields.Size = new Size(frm.ClientSize.Width - outerPad * 2, frm.ClientSize.Height - lblTitle.Height - btnPanelHeight - outerPad * 3);

            // ID (opcional)
            Guna2TextBox txtId = null;
            if (pedido != null)
            {
                var lblId = new Guna2HtmlLabel { Text = "ID do Pedido", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true };
                txtId = new Guna2TextBox { Text = pedido.Id.ToString(), ReadOnly = true, Width = fields.Width, Height = fieldH, BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };
                lblId.Margin = new Padding(0, 0, 0, gapLabelField);
                txtId.Margin = new Padding(0, 0, 0, verticalSpacing);
                fields.Controls.Add(lblId);
                fields.Controls.Add(txtId);
            }

            // Row: Data (left) | Valor (right)
            var row1 = new Panel { Width = fields.Width, Height = labelH + gapLabelField + fieldH, Margin = new Padding(0, 0, 0, verticalSpacing) };
            int colGap = 20;
            int leftColW = (row1.Width - colGap) * 55 / 100; // data wider
            int rightColW = row1.Width - colGap - leftColW;

            var leftPanel1 = new Panel { Location = new Point(0, 0), Size = new Size(leftColW, row1.Height) };
            var lblData = new Guna2HtmlLabel { Text = "Data", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true, Location = new Point(0, 0) };
            var dpData = new Guna2DateTimePicker { Width = leftPanel1.Width, Location = new Point(0, labelH + gapLabelField), Format = DateTimePickerFormat.Short };
            // Visual: fundo escuro, texto branco, sem bordas visíveis
            try
            {
                dpData.Height = fieldH;
                dpData.BackColor = AppTheme.SurfaceAlt;
                dpData.ForeColor = AppTheme.TextPrimary;
                dpData.CalendarForeColor = AppTheme.TextPrimary;
                dpData.CalendarMonthBackground = AppTheme.SurfaceAlt;
                dpData.CalendarTitleBackColor = AppTheme.SurfaceAlt;
                dpData.CalendarTitleForeColor = AppTheme.TextPrimary;
                dpData.CalendarTrailingForeColor = AppTheme.TextSecondary;
                // Guna-like properties (stubs) to allow rounded visual where available
                try { dpData.GetType().GetProperty("FillColor")?.SetValue(dpData, AppTheme.SurfaceAlt); } catch { }
                try { dpData.GetType().GetProperty("BorderRadius")?.SetValue(dpData, 10); } catch { }
                try { dpData.GetType().GetProperty("BorderThickness")?.SetValue(dpData, 0); } catch { }
                try { dpData.GetType().GetProperty("BorderColor")?.SetValue(dpData, AppTheme.SurfaceAlt); } catch { }
            }
            catch { }
            leftPanel1.Controls.Add(lblData);
            leftPanel1.Controls.Add(dpData);

            var rightPanel1 = new Panel { Location = new Point(leftColW + colGap, 0), Size = new Size(rightColW, row1.Height) };
            var lblValor = new Guna2HtmlLabel { Text = "Valor", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true, Location = new Point(0, 0) };
            var txtValor = new Guna2TextBox { PlaceholderText = "Valor total", Width = rightPanel1.Width, Height = fieldH, Location = new Point(0, labelH + gapLabelField), ReadOnly = false, Enabled = true };
            rightPanel1.Controls.Add(lblValor);
            rightPanel1.Controls.Add(txtValor);

            row1.Controls.Add(leftPanel1);
            row1.Controls.Add(rightPanel1);
            fields.Controls.Add(row1);

            // Row: Cliente (left) | Status (right)
            var row2 = new Panel { Width = fields.Width, Height = labelH + gapLabelField + fieldH, Margin = new Padding(0, 0, 0, verticalSpacing) };
            var leftPanel2 = new Panel { Location = new Point(0, 0), Size = new Size(leftColW, row2.Height) };
            var lblCliente = new Guna2HtmlLabel { Text = "Cliente", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true, Location = new Point(0, 0) };
            var cbCliente = new Guna2ComboBox { Width = leftPanel2.Width, Location = new Point(0, labelH + gapLabelField), DropDownStyle = ComboBoxStyle.DropDownList };
            // Ensure dark theme and no white areas; use Guna stub properties when available
            try
            {
                cbCliente.Height = fieldH;
                cbCliente.BackColor = AppTheme.SurfaceAlt;
                cbCliente.ForeColor = AppTheme.TextPrimary;
                cbCliente.FlatStyle = FlatStyle.Flat;
                cbCliente.DrawMode = DrawMode.OwnerDrawFixed;
                cbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
                try { cbCliente.GetType().GetProperty("FillColor")?.SetValue(cbCliente, AppTheme.SurfaceAlt); } catch { }
                try { cbCliente.GetType().GetProperty("BorderRadius")?.SetValue(cbCliente, 10); } catch { }
                try { cbCliente.GetType().GetProperty("BorderThickness")?.SetValue(cbCliente, 0); } catch { }
                try { cbCliente.GetType().GetProperty("BorderColor")?.SetValue(cbCliente, AppTheme.SurfaceAlt); } catch { }
            }
            catch { }
            leftPanel2.Controls.Add(lblCliente);
            leftPanel2.Controls.Add(cbCliente);

            var rightPanel2 = new Panel { Location = new Point(leftColW + colGap, 0), Size = new Size(rightColW, row2.Height) };
            var lblStatus = new Guna2HtmlLabel { Text = "Status", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true, Location = new Point(0, 0) };
            var cbStatus = new Guna2ComboBox { Width = rightPanel2.Width, Location = new Point(0, labelH + gapLabelField), DropDownStyle = ComboBoxStyle.DropDownList };
            try
            {
                cbStatus.Height = fieldH;
                cbStatus.BackColor = AppTheme.SurfaceAlt;
                cbStatus.ForeColor = AppTheme.TextPrimary;
                cbStatus.FlatStyle = FlatStyle.Flat;
                cbStatus.DrawMode = DrawMode.OwnerDrawFixed;
                cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
                try { cbStatus.GetType().GetProperty("FillColor")?.SetValue(cbStatus, AppTheme.SurfaceAlt); } catch { }
                try { cbStatus.GetType().GetProperty("BorderRadius")?.SetValue(cbStatus, 10); } catch { }
                try { cbStatus.GetType().GetProperty("BorderThickness")?.SetValue(cbStatus, 0); } catch { }
                try { cbStatus.GetType().GetProperty("BorderColor")?.SetValue(cbStatus, AppTheme.SurfaceAlt); } catch { }
            }
            catch { }
            rightPanel2.Controls.Add(lblStatus);
            rightPanel2.Controls.Add(cbStatus);

            row2.Controls.Add(leftPanel2);
            row2.Controls.Add(rightPanel2);
            fields.Controls.Add(row2);

            // Populate combo boxes
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

            // Buttons bottom
            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = btnPanelHeight, BackColor = Color.Transparent };
            var flButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = false, Height = 36 };
            var btnCancelar = new Guna2Button { Text = "Cancelar", Width = 140, Height = 36 };
            var btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Height = 36 };
            flButtons.Controls.Add(btnCancelar);
            flButtons.Controls.Add(btnSalvar);
            btnPanel.Controls.Add(flButtons);
            flButtons.Width = btnCancelar.Width + btnSalvar.Width + 12;
            flButtons.Left = Math.Max(0, (btnPanel.ClientSize.Width - flButtons.Width) / 2);
            flButtons.Top = Math.Max(0, (btnPanel.Height - flButtons.Height) / 2);
            flButtons.Anchor = AnchorStyles.None;

            main.Controls.Add(lblTitle);
            main.Controls.Add(fields);
            main.Controls.Add(btnPanel);
            frm.Controls.Add(main);

            // Apply styling
            if (txtId != null) { UIHelpers.StyleTextBoxFlat(txtId, 10); UIHelpers.RoundControl(txtId, 10); }
            UIHelpers.StyleDateTimePicker(dpData, fieldH, 10);
            UIHelpers.RoundControl(dpData, 10);
            UIHelpers.StyleTextBoxFlat(txtValor, 10); UIHelpers.RoundControl(txtValor, 10);
            UIHelpers.StyleComboBox(cbCliente, fieldH, 10); UIHelpers.RoundControl(cbCliente, 10);
            UIHelpers.StyleComboBox(cbStatus, fieldH, 10); UIHelpers.RoundControl(cbStatus, 10);
            UIHelpers.RoundControl(main, 10);
            UIHelpers.StyleButton(btnSalvar, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnCancelar, AppTheme.SurfaceAlt, AppTheme.Surface);
            UIHelpers.RoundControl(btnSalvar, 10);
            UIHelpers.RoundControl(btnCancelar, 10);

            // Keep events and logic
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
