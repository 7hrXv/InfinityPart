using System.Drawing;
using System.Windows.Forms;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    public static class UIHelpers
    {
        public static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = AppTheme.Surface;
            grid.GridColor = AppTheme.Border;
            grid.BorderStyle = BorderStyle.None;
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

            grid.ColumnHeadersDefaultCellStyle.BackColor = AppTheme.PrimaryRed;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontBodyBold;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AppTheme.PrimaryRedDark;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.DefaultCellStyle.BackColor = AppTheme.SurfaceAlt;
            grid.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = AppTheme.PrimaryRedDark;
            grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.Padding = new Padding(6, 0, 0, 0);

            grid.AlternatingRowsDefaultCellStyle.BackColor = AppTheme.Surface;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = AppTheme.PrimaryRedDark;
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
        }

        public static void StyleTextBox(TextBox txt)
        {
            txt.BackColor = AppTheme.SurfaceAlt;
            txt.ForeColor = AppTheme.TextPrimary;
            txt.Font = AppTheme.FontBody;
            // Try to set Guna-specific properties via reflection when available
            try
            {
                var t = txt.GetType();
                var propPlaceholder = t.GetProperty("PlaceholderForeColor");
                if (propPlaceholder != null && propPlaceholder.CanWrite)
                    propPlaceholder.SetValue(txt, AppTheme.TextMuted);

                var propBorder = t.GetProperty("BorderRadius");
                if (propBorder != null && propBorder.CanWrite)
                    propBorder.SetValue(txt, 8);
            }
            catch { }
        }

        public static Image CreateMagnifierIcon(Color color, int size = 16)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            using var pen = new Pen(color, 2);
            // draw circle
            var rect = new Rectangle(1, 1, size - 6, size - 6);
            g.DrawEllipse(pen, rect);
            // draw handle
            g.DrawLine(pen, size - 6, size - 6, size - 2, size - 2);
            return bmp;
        }

        // Cria um PictureBox padronizado para ícone de busca (lupa)
        public static PictureBox CreateSearchIcon(Color color, int iconSize = 16)
        {
            var pic = new PictureBox();
            pic.Width = 30;
            pic.Height = 30;
            pic.SizeMode = PictureBoxSizeMode.CenterImage;
            pic.BackColor = Color.Transparent;
            pic.Margin = new Padding(4, 2, 4, 2);
            pic.Image = CreateMagnifierIcon(color, iconSize);
            return pic;
        }

        public static void RoundControl(Control ctl, int radius)
        {
            try
            {
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                var rect = ctl.ClientRectangle;
                int r = Math.Max(0, radius);
                path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                path.CloseFigure();
                ctl.Region = new System.Drawing.Region(path);
            }
            catch { }
        }

        public static void StyleButton(Button btn)
        {
            StyleButton(btn, AppTheme.PrimaryRed, AppTheme.PrimaryRedHover);
        }

        public static void StyleButton(Button btn, Color baseColor, Color hoverColor)
        {
            btn.BackColor = baseColor;
            btn.ForeColor = AppTheme.TextPrimary;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            try
            {
                btn.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
                btn.FlatAppearance.BorderSize = 0;
            }
            catch { }

            // remove existing handlers to avoid duplicates
            btn.MouseEnter -= Btn_MouseEnter;
            btn.MouseLeave -= Btn_MouseLeave;

            // store colors in Tag for handlers
            btn.Tag = (baseColor, hoverColor);

            btn.MouseEnter += Btn_MouseEnter;
            btn.MouseLeave += Btn_MouseLeave;

            // Try to set Guna2Button specific visual properties if available
            try
            {
                var t = btn.GetType();
                var propFill = t.GetProperty("FillColor");
                if (propFill != null && propFill.CanWrite)
                    propFill.SetValue(btn, baseColor);

                var propRadius = t.GetProperty("BorderRadius");
                if (propRadius != null && propRadius.CanWrite)
                    propRadius.SetValue(btn, 6);

                // HoverState.FillColor
                var propHover = t.GetProperty("HoverState");
                if (propHover != null)
                {
                    var hoverState = propHover.GetValue(btn);
                    if (hoverState != null)
                    {
                        var hsType = hoverState.GetType();
                        var propHsFill = hsType.GetProperty("FillColor");
                        if (propHsFill != null && propHsFill.CanWrite)
                            propHsFill.SetValue(hoverState, hoverColor);
                    }
                }
            }
            catch { }
        }

        private static void Btn_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is ValueTuple<Color, Color> t)
                btn.BackColor = t.Item1;
        }

        private static void Btn_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is ValueTuple<Color, Color> t)
                btn.BackColor = t.Item2;
        }
    }
}
