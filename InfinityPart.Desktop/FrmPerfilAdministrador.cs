using System.Globalization;
using InfinityPart.Desktop.Models;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    public class FrmPerfilAdministrador : Form
    {
        private Guna2TextBox txtNome;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtSenha;
        private Guna2TextBox txtCpf;
        private Guna2TextBox txtTelefone;
        private Guna2TextBox txtDataCadastro;
        private Guna2Button btnSalvar;
        private Guna2Button btnSairConta;

        public FrmPerfilAdministrador()
        {
            Text = "Perfil do Administrador";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            BackColor = Theme.AppTheme.Background;
            Font = Theme.AppTheme.FontBody;

            // Layout constants (ajustados para nunca sobrepor/cortar labels e campos)
            const int dialogWidth = 520;
            const int outerPad = 18; // Padding do painel "main"

            const int leftX = 24;
            const int rightX = 264;
            const int fieldWidthLeft = 190;
            const int fieldWidthRight = 190;
            const int labelH = 20;
            const int gapLabelField = 8;
            const int fieldH = 34;
            const int rowSpacing = 26;
            const int rowUnit = labelH + gapLabelField + fieldH + rowSpacing;
            const int contentTop = 60;
            const int btnPanelHeight = 72;
            const int gapBeforeButtons = 24;

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(outerPad), BackColor = Color.Transparent };
            var card = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(20), BackColor = Theme.AppTheme.Surface };

            var lblTitle = new Guna2HtmlLabel { Text = "PERFIL DO ADMINISTRADOR", Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 14f, FontStyle.Bold) };

            // Linha 0: Nome (esquerda) / CPF (direita)
            var row0Y = contentTop;
            var lblNome = new Guna2HtmlLabel { Text = "Nome", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, row0Y), AutoSize = true };
            txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, row0Y + labelH + gapLabelField), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };

            var lblCpf = new Guna2HtmlLabel { Text = "CPF", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(rightX, row0Y), AutoSize = true };
            txtCpf = new Guna2TextBox { PlaceholderText = "CPF", Width = fieldWidthRight, Height = fieldH, Location = new Point(rightX, row0Y + labelH + gapLabelField), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };

            // Linha 1: E-mail (esquerda) / Telefone (direita)
            var row1Y = contentTop + rowUnit;
            var lblEmail = new Guna2HtmlLabel { Text = "E-mail", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, row1Y), AutoSize = true };
            txtEmail = new Guna2TextBox { PlaceholderText = "E-mail", Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, row1Y + labelH + gapLabelField), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };

            var lblTelefone = new Guna2HtmlLabel { Text = "Telefone", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(rightX, row1Y), AutoSize = true };
            txtTelefone = new Guna2TextBox { PlaceholderText = "Telefone", Width = fieldWidthRight, Height = fieldH, Location = new Point(rightX, row1Y + labelH + gapLabelField), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };

            // Linha 2: Senha (esquerda) / Data de Cadastro (direita, somente leitura)
            var row2Y = contentTop + rowUnit * 2;
            var lblSenha = new Guna2HtmlLabel { Text = "Senha", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(leftX, row2Y), AutoSize = true };
            txtSenha = new Guna2TextBox { PlaceholderText = "Senha", Width = fieldWidthLeft, Height = fieldH, Location = new Point(leftX, row2Y + labelH + gapLabelField), UseSystemPasswordChar = true, BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };

            var lblDataCadastro = new Guna2HtmlLabel { Text = "Data de Cadastro", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(rightX, row2Y), AutoSize = true };
            txtDataCadastro = new Guna2TextBox { ReadOnly = true, Width = fieldWidthRight, Height = fieldH, Location = new Point(rightX, row2Y + labelH + gapLabelField), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };

            // Altura do diálogo calculada dinamicamente para nunca cortar campos/botões
            var lastFieldBottom = row2Y + labelH + gapLabelField + fieldH;
            var cardContentHeight = lastFieldBottom + gapBeforeButtons + btnPanelHeight + 20; // 20 = Padding inferior do card
            var clientHeight = cardContentHeight + outerPad * 2;
            ClientSize = new Size(dialogWidth, clientHeight);

            // Botões alinhados via FlowLayoutPanel: garante que "Sair da conta" fique sempre
            // totalmente visível e corretamente alinhado, independente dos paddings internos
            // do card (o cálculo manual de posição por pixel cortava o botão antes).
            var btnPanel = new Guna2Panel { Dock = DockStyle.Bottom, Height = btnPanelHeight, BackColor = Color.Transparent };
            btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Height = 36 };
            btnSairConta = new Guna2Button { Text = "Sair da conta", Width = 150, Height = 36, AutoSize = false };

            var flBtnPerfil = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Padding = new Padding(0, (btnPanelHeight - 36) / 2, 24, 0)
            };
            btnSairConta.Margin = new Padding(0, 0, 12, 0);
            btnSalvar.Margin = new Padding(0);
            flBtnPerfil.Controls.Add(btnSairConta);
            flBtnPerfil.Controls.Add(btnSalvar);
            btnPanel.Controls.Add(flBtnPerfil);

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblNome);
            card.Controls.Add(txtNome);
            card.Controls.Add(lblEmail);
            card.Controls.Add(txtEmail);
            card.Controls.Add(lblSenha);
            card.Controls.Add(txtSenha);

            card.Controls.Add(lblCpf);
            card.Controls.Add(txtCpf);
            card.Controls.Add(lblTelefone);
            card.Controls.Add(txtTelefone);
            card.Controls.Add(lblDataCadastro);
            card.Controls.Add(txtDataCadastro);

            card.Controls.Add(btnPanel);

            main.Controls.Add(card);
            Controls.Add(main);

            Load += FrmPerfilAdministrador_Load;
            btnSalvar.Click += async (_, _) => await SalvarAsync();
            btnSairConta.Click += (_, _) => SairConta();

            // Estilos: sem borda branca, fundo #222222, texto branco, cantos arredondados (8)
            foreach (var tb in new[] { txtNome, txtEmail, txtSenha, txtCpf, txtTelefone, txtDataCadastro })
            {
                UIHelpers.StyleTextBoxFlat(tb, 10);
            }

            // Restrições de digitação (não alteram a lógica de gravação existente)
            Validacoes.SomenteLetras(txtNome);
            Validacoes.SomenteNumeros(txtCpf);
            Validacoes.SomenteNumeros(txtTelefone);

            UIHelpers.StyleButton(btnSalvar, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnSairConta, AppTheme.SurfaceAlt, AppTheme.Surface);
            UIHelpers.RoundControl(btnSalvar, 10);
            UIHelpers.RoundControl(btnSairConta, 10);
            UIHelpers.RoundControl(card, 10);
        }

        private void FrmPerfilAdministrador_Load(object? sender, EventArgs e)
        {
            var adm = SessaoAtual.Administrador;
            if (adm == null)
                return;

            // Garante que todos os dados do administrador sejam carregados no formulário
            txtNome.Text = adm.Nome;
            txtEmail.Text = adm.Email;
            txtCpf.Text = adm.Cpf;
            txtTelefone.Text = adm.Telefone;
            txtDataCadastro.Text = adm.DataCadastro == default
                ? string.Empty
                : adm.DataCadastro.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        private async Task SalvarAsync()
        {
            // Validações obrigatórias antes de salvar
            if (!Validacoes.ObrigatorioPreenchido(txtNome.Text) || !Validacoes.SomenteLetrasTexto(txtNome.Text))
            {
                Validacoes.MostrarErro("Informe um Nome válido (somente letras e espaços).");
                return;
            }

            if (!Validacoes.ObrigatorioPreenchido(txtEmail.Text) || !Validacoes.EmailValido(txtEmail.Text))
            {
                Validacoes.MostrarErro("Informe um E-mail em um formato válido.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtCpf.Text) && !Validacoes.SomenteNumerosTexto(txtCpf.Text))
            {
                Validacoes.MostrarErro("O campo CPF/CNPJ deve conter somente números.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtTelefone.Text) && !Validacoes.SomenteNumerosTexto(txtTelefone.Text))
            {
                Validacoes.MostrarErro("O campo Telefone deve conter somente números.");
                return;
            }

            var adm = SessaoAtual.Administrador;
            if (adm == null)
                return;

            adm.Nome = txtNome.Text.Trim();
            adm.Email = txtEmail.Text.Trim();
            adm.Cpf = txtCpf.Text.Trim();
            adm.Telefone = txtTelefone.Text.Trim();
            // Data de Cadastro é somente leitura e não é alterada pelo formulário.
            // Em modo offline a senha é apenas informativa; não chamamos API
            // Se desejar, poderia armazenar hash local temporário.

            SessaoAtual.Administrador = adm;
            MessageBox.Show("Dados atualizados.", "Perfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void SairConta()
        {
            var confirma = MessageBox.Show("Deseja sair da conta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma != DialogResult.Yes)
                return;

            SessaoAtual.Encerrar();

            // Volta ao login existente
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
