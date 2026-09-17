using InfinityPart.Desktop.Controls;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer? components = null;

        // Sidebar
        private Panel pnlSidebar = null!;
        private Label lblLogo = null!;
        private Label lblLogoAccent = null!;
        private SidebarButton btnNavDashboard = null!;
        private SidebarButton btnNavProdutos = null!;
        private SidebarButton btnNavClientes = null!;
        private SidebarButton btnNavPedidos = null!;
        private SidebarButton btnNavMarcas = null!;
        private SidebarButton btnNavSair = null!;

        // Header
        private Panel pnlHeader = null!;
        private Label lblSectionTitle = null!;
        private Label lblUser = null!;
        private Panel pnlHeaderDivider = null!;

        // Content
        private Panel pnlContent = null!;

        // Dashboard section
        private Panel pnlDashboard = null!;
        private FlowLayoutPanel flpCards = null!;
        private DashboardCard cardProdutos = null!;
        private DashboardCard cardClientes = null!;
        private DashboardCard cardPedidos = null!;
        private DashboardCard cardMarcas = null!;
        private Label lblRecentTitle = null!;
        private DataGridView dgvPedidosRecentes = null!;
        private Label lblStatusApi = null!;

        // Placeholder para módulos ainda não implementados nesta etapa
        private Panel pnlPlaceholder = null!;
        private Label lblPlaceholder = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            SuspendLayout();

            // ===== Form =====
            AutoScaleMode = AutoScaleMode.Font;
            Text = "InfinityPart — Painel Principal";
            BackColor = AppTheme.Background;
            ForeColor = AppTheme.TextPrimary;
            Font = AppTheme.FontBody;
            MinimumSize = new Size(1100, 650);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            // ===== Sidebar =====
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = AppTheme.Sidebar
            };

            lblLogo = new Label
            {
                Text = "INFINITY",
                Font = new Font(AppTheme.FontFamily, 15f, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                AutoSize = true,
                Location = new Point(28, 28)
            };

            lblLogoAccent = new Label
            {
                Text = "PART",
                Font = new Font(AppTheme.FontFamily, 15f, FontStyle.Bold),
                ForeColor = AppTheme.PrimaryRed,
                AutoSize = true,
                Location = new Point(lblLogo.Location.X + 90, 28)
            };

            btnNavDashboard = new SidebarButton { Text = "Dashboard", Location = new Point(0, 90), IsActive = true };
            btnNavProdutos = new SidebarButton { Text = "Produtos", Location = new Point(0, 136) };
            btnNavClientes = new SidebarButton { Text = "Clientes", Location = new Point(0, 182) };
            btnNavPedidos = new SidebarButton { Text = "Pedidos", Location = new Point(0, 228) };
            btnNavMarcas = new SidebarButton { Text = "Marcas", Location = new Point(0, 274) };
            btnNavSair = new SidebarButton { Text = "Sair", Location = new Point(0, 338) };

            pnlSidebar.Controls.Add(lblLogo);
            pnlSidebar.Controls.Add(lblLogoAccent);
            pnlSidebar.Controls.Add(btnNavDashboard);
            pnlSidebar.Controls.Add(btnNavProdutos);
            pnlSidebar.Controls.Add(btnNavClientes);
            pnlSidebar.Controls.Add(btnNavPedidos);
            pnlSidebar.Controls.Add(btnNavMarcas);
            pnlSidebar.Controls.Add(btnNavSair);

            // ===== Header =====
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = AppTheme.Surface
            };

            lblSectionTitle = new Label
            {
                Text = "Dashboard",
                Font = AppTheme.FontHeading,
                ForeColor = AppTheme.TextPrimary,
                AutoSize = true,
                Location = new Point(30, 18)
            };

            lblUser = new Label
            {
                Text = "Olá, Administrador",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextSecondary,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            pnlHeaderDivider = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = AppTheme.Border
            };

            pnlHeader.Controls.Add(lblSectionTitle);
            pnlHeader.Controls.Add(lblUser);
            pnlHeader.Controls.Add(pnlHeaderDivider);
            pnlHeader.Resize += (_, _) => PositionHeaderUser();

            // ===== Content =====
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppTheme.Background,
                Padding = new Padding(30, 24, 30, 24)
            };

            // ----- Dashboard section -----
            pnlDashboard = new Panel { Dock = DockStyle.Fill, BackColor = AppTheme.Background };

            flpCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 130,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false,
                BackColor = AppTheme.Background
            };

            cardProdutos = new DashboardCard { Caption = "PRODUTOS CADASTRADOS", Margin = new Padding(0, 0, 18, 0) };
            cardClientes = new DashboardCard { Caption = "CLIENTES CADASTRADOS", Margin = new Padding(0, 0, 18, 0) };
            cardPedidos = new DashboardCard { Caption = "PEDIDOS REALIZADOS", Margin = new Padding(0, 0, 18, 0) };
            cardMarcas = new DashboardCard { Caption = "MARCAS CADASTRADAS", Margin = new Padding(0, 0, 0, 0) };

            flpCards.Controls.Add(cardProdutos);
            flpCards.Controls.Add(cardClientes);
            flpCards.Controls.Add(cardPedidos);
            flpCards.Controls.Add(cardMarcas);

            lblRecentTitle = new Label
            {
                Text = "PEDIDOS RECENTES",
                Dock = DockStyle.Top,
                Height = 34,
                Font = AppTheme.FontBodyBold,
                ForeColor = AppTheme.TextSecondary,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(2, 0, 0, 6),
                Margin = new Padding(0, 16, 0, 0)
            };

            dgvPedidosRecentes = new DataGridView { Dock = DockStyle.Fill };
            EstilizarGrid(dgvPedidosRecentes);

            lblStatusApi = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextMuted,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = string.Empty
            };

            var pnlGridWrapper = new Panel { Dock = DockStyle.Fill, BackColor = AppTheme.Background };
            pnlGridWrapper.Controls.Add(dgvPedidosRecentes);
            pnlGridWrapper.Controls.Add(lblStatusApi);
            pnlGridWrapper.Controls.Add(lblRecentTitle);

            pnlDashboard.Controls.Add(pnlGridWrapper);
            pnlDashboard.Controls.Add(flpCards);

            // ----- Placeholder (módulos das próximas etapas) -----
            pnlPlaceholder = new Panel { Dock = DockStyle.Fill, BackColor = AppTheme.Background, Visible = false };
            lblPlaceholder = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Este módulo será implementado em uma próxima etapa.",
                Font = AppTheme.FontSubtitle,
                ForeColor = AppTheme.TextMuted,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPlaceholder.Controls.Add(lblPlaceholder);

            pnlContent.Controls.Add(pnlPlaceholder);
            pnlContent.Controls.Add(pnlDashboard);

            // ===== Composição final =====
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);

            PositionHeaderUser();

            ResumeLayout(false);
        }

        private void PositionHeaderUser()
        {
            lblUser.Location = new Point(pnlHeader.Width - lblUser.Width - 30, 24);
        }

        private static void EstilizarGrid(DataGridView grid)
        {
            grid.BackgroundColor = AppTheme.Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = AppTheme.Border;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.ReadOnly = true;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.RowTemplate.Height = 34;
            grid.ColumnHeadersHeight = 38;
            grid.Font = AppTheme.FontBody;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            grid.ColumnHeadersDefaultCellStyle.BackColor = AppTheme.SurfaceAlt;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontBodyBold;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AppTheme.SurfaceAlt;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.DefaultCellStyle.BackColor = AppTheme.Surface;
            grid.DefaultCellStyle.ForeColor = AppTheme.TextSecondary;
            grid.DefaultCellStyle.SelectionBackColor = AppTheme.PrimaryRedDark;
            grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.Padding = new Padding(6, 0, 0, 0);

            grid.AlternatingRowsDefaultCellStyle.BackColor = AppTheme.SurfaceAlt;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = AppTheme.TextSecondary;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = AppTheme.PrimaryRedDark;
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
        }
    }
}
