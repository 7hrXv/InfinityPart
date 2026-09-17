using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Controls
{
    /// <summary>
    /// Item de navegação do menu lateral. Mostra uma barra vermelha à esquerda
    /// e fundo destacado quando está selecionado (tela atual).
    /// </summary>
    public class SidebarButton : Button
    {
        private const int IndicatorWidth = 4;
        private bool _isActive;
        private bool _isHover;

        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; Invalidate(); }
        }

        public SidebarButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Size = new Size(230, 46);
            TextAlign = ContentAlignment.MiddleLeft;
            Padding = new Padding(28, 0, 0, 0);
            Font = AppTheme.FontNav;
            ForeColor = AppTheme.TextSecondary;
            BackColor = AppTheme.Sidebar;

            MouseEnter += (_, _) => { _isHover = true; Invalidate(); };
            MouseLeave += (_, _) => { _isHover = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;

            Color back = _isActive ? AppTheme.SurfaceAlt : (_isHover ? AppTheme.Surface : AppTheme.Sidebar);
            using (var brush = new SolidBrush(back))
                g.FillRectangle(brush, ClientRectangle);

            if (_isActive)
            {
                using var indicator = new SolidBrush(AppTheme.PrimaryRed);
                g.FillRectangle(indicator, 0, 0, IndicatorWidth, Height);
            }

            ForeColor = _isActive ? AppTheme.TextPrimary : AppTheme.TextSecondary;

            var textRect = new Rectangle(Padding.Left, 0, Width - Padding.Left, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }
}
