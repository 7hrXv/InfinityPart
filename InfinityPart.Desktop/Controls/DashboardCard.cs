using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop.Controls
{
    /// <summary>
    /// Card de indicador usado no dashboard (ex.: "128 Produtos cadastrados").
    /// </summary>
    public class DashboardCard : Panel
    {
        private readonly Label _lblValue;
        private readonly Label _lblCaption;
        private readonly Panel _accentBar;

        public string Value
        {
            get => _lblValue.Text;
            set => _lblValue.Text = value;
        }

        public string Caption
        {
            get => _lblCaption.Text;
            set => _lblCaption.Text = value;
        }

        public Color AccentColor
        {
            get => _accentBar.BackColor;
            set => _accentBar.BackColor = value;
        }

        public DashboardCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = AppTheme.Surface;
            Size = new Size(230, 110);
            Padding = new Padding(18, 14, 14, 14);

            // Posicionamento manual (em vez de empilhar via Dock) para não depender
            // da ordem de inserção no Controls, evitando ambiguidade de layout.
            _accentBar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(5, Height),
                BackColor = AppTheme.PrimaryRed,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            _lblCaption = new Label
            {
                AutoSize = false,
                Location = new Point(22, 16),
                Size = new Size(Width - 36, 20),
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextSecondary,
                TextAlign = ContentAlignment.TopLeft,
                Text = "Indicador",
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };

            _lblValue = new Label
            {
                AutoSize = false,
                Location = new Point(20, 40),
                Size = new Size(Width - 36, 50),
                Font = AppTheme.FontCardNumber,
                ForeColor = AppTheme.TextPrimary,
                TextAlign = ContentAlignment.TopLeft,
                Text = "0",
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };

            Controls.Add(_accentBar);
            Controls.Add(_lblCaption);
            Controls.Add(_lblValue);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(AppTheme.Border, 1);
            e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }
    }
}
