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
        private Guna2Button btnSalvar;
        private Guna2Button btnSairConta;

        public FrmPerfilAdministrador()
        {
            Text = "Perfil do Administrador";
            Width = 520;
            Height = 420;
            StartPosition = FormStartPosition.CenterParent;

            BackColor = Theme.AppTheme.Background;
            Font = Theme.AppTheme.FontBody;

            var main = new Guna2Panel { Dock = DockStyle.Fill, Padding = new Padding(18), BackColor = Color.Transparent };
            var card = new Guna2Panel { Width = 440, Height = 300, BackColor = Theme.AppTheme.Surface, Location = new System.Drawing.Point((Width - 440) / 2, 20) };

            var lblTitle = new Guna2HtmlLabel { Text = "PERFIL DO ADMINISTRADOR", Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.White, Font = new Font("Segoe UI", 14f, FontStyle.Bold) };

            txtNome = new Guna2TextBox { PlaceholderText = "Nome", Width = 400, Location = new System.Drawing.Point(20, 60), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };
            txtEmail = new Guna2TextBox { PlaceholderText = "E-mail", Width = 400, Location = new System.Drawing.Point(20, 110), BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };
            txtSenha = new Guna2TextBox { PlaceholderText = "Senha", Width = 400, Location = new System.Drawing.Point(20, 160), UseSystemPasswordChar = true, BackColor = Theme.AppTheme.SurfaceAlt, ForeColor = Color.White };

            btnSalvar = new Guna2Button { Text = "Salvar", Width = 140, Location = new System.Drawing.Point(20, 210) };
            btnSairConta = new Guna2Button { Text = "Sair da conta", Width = 140, Location = new System.Drawing.Point(200, 210) };

            card.Controls.Add(lblTitle);
            card.Controls.Add(txtNome);
            card.Controls.Add(txtEmail);
            card.Controls.Add(txtSenha);
            card.Controls.Add(btnSalvar);
            card.Controls.Add(btnSairConta);

            main.Controls.Add(card);
            Controls.Add(main);

            Load += FrmPerfilAdministrador_Load;
            btnSalvar.Click += async (_, _) => await SalvarAsync();
            btnSairConta.Click += (_, _) => SairConta();

            UIHelpers.StyleTextBox(txtNome);
            UIHelpers.StyleTextBox(txtEmail);
            UIHelpers.StyleTextBox(txtSenha);
            UIHelpers.StyleButton(btnSalvar, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
            UIHelpers.StyleButton(btnSairConta, AppTheme.SurfaceAlt, AppTheme.Surface);
        }

        private void FrmPerfilAdministrador_Load(object? sender, EventArgs e)
        {
            var adm = SessaoAtual.Administrador;
            if (adm == null)
                return;

            txtNome.Text = adm.Nome;
            txtEmail.Text = adm.Email;
        }

        private async Task SalvarAsync()
        {
            var adm = SessaoAtual.Administrador;
            if (adm == null)
                return;

            adm.Nome = txtNome.Text.Trim();
            adm.Email = txtEmail.Text.Trim();
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
