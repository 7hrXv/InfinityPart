using System.Drawing.Drawing2D;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Controls
{
    public enum AppButtonStyle
    {
        Primary,   // vermelho escuro sólido
        Secondary, // contorno, fundo transparente
        Ghost      // texto simples, sem fundo/borda (ações discretas)
    }

    /// <summary>
    /// Botão padrão reutilizável em todo o sistema, com cantos arredondados
    /// e efeito de hover, seguindo a identidade visual (preto/vermelho/branco).
    /// </summary>
    public class AppButton : Button
    {
        private const int CornerRadius = 8;
        private bool _isHover;

        private AppButtonStyle _style = AppButtonStyle.Primary;
        public AppButtonStyle Style
        {
            get => _style;
            set { _style = value; ApplyStyle(); Invalidate(); }
        }

        public AppButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Font = AppTheme.FontBodyBold;
            Height = 38;
            TextAlign = ContentAlignment.MiddleCenter;
            ApplyStyle();

            MouseEnter += (_, _) => { _isHover = true; Invalidate(); };
            MouseLeave += (_, _) => { _isHover = false; Invalidate(); };
        }

        private void ApplyStyle()
        {
            switch (_style)
            {
                case AppButtonStyle.Primary:
                    ForeColor = AppTheme.TextPrimary;
                    BackColor = AppTheme.PrimaryRed;
                    break;
                case AppButtonStyle.Secondary:
                    ForeColor = AppTheme.TextPrimary;
                    BackColor = AppTheme.SurfaceAlt;
                    break;
                case AppButtonStyle.Ghost:
                    ForeColor = AppTheme.TextSecondary;
                    BackColor = AppTheme.Background;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using var path = RoundedRect(rect, CornerRadius);

            Color fill = _style switch
            {
                AppButtonStyle.Primary => _isHover ? AppTheme.PrimaryRedHover : AppTheme.PrimaryRed,
                AppButtonStyle.Secondary => _isHover ? AppTheme.Border : AppTheme.SurfaceAlt,
                AppButtonStyle.Ghost => _isHover ? AppTheme.SurfaceAlt : AppTheme.Background,
                _ => BackColor
            };

            using (var brush = new SolidBrush(fill))
                g.FillPath(brush, path);

            if (_style == AppButtonStyle.Secondary)
            {
                using var pen = new Pen(AppTheme.Border, 1);
                g.DrawPath(pen, path);
            }

            TextRenderer.DrawText(g, Text, Font, rect, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
