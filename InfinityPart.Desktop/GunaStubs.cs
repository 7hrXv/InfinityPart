using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// Stubs no namespace global para evitar dependência da biblioteca Guna no ambiente de build
// Estes tipos reproduzem a API mínima usada pelo projeto.
public class Guna2DataGridView : DataGridView { }
public class Guna2TextBox : TextBox
{
    private const int EM_SETCUEBANNER = 0x1501;

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

    private string _placeholderText = string.Empty;

    // Mantido para compatibilidade com o código existente (UIHelpers usa reflexão para
    // tentar definir esta propriedade). O banner nativo do Windows já renderiza o texto
    // de apoio em cinza por padrão; a propriedade fica disponível para futura customização.
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Color PlaceholderForeColor { get; set; } = Color.Gray;

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string PlaceholderText
    {
        get => _placeholderText;
        set
        {
            _placeholderText = value ?? string.Empty;
            AtualizarPlaceholder();
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        AtualizarPlaceholder();
    }

    private void AtualizarPlaceholder()
    {
        if (IsHandleCreated)
            SendMessage(Handle, EM_SETCUEBANNER, IntPtr.Zero, _placeholderText);
    }
}
public class Guna2Button : Button { }
public class Guna2Panel : Panel { }
public class Guna2HtmlLabel : Label { }
public class Guna2PictureBox : PictureBox { }
// Minimal stubs for additional Guna controls used by the UI
public class Guna2ComboBox : ComboBox
{
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int BorderRadius { get; set; }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int BorderThickness { get; set; }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Color BorderColor { get; set; }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Color FillColor { get; set; }
    public Guna2ComboBox()
    {
        // enable custom painting
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        DrawMode = DrawMode.OwnerDrawFixed;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        try
        {
            var g = e.Graphics;
            var rect = ClientRectangle;
            // background
            using var bg = new SolidBrush(FillColor.IsEmpty ? ColorTranslator.FromHtml("#222222") : FillColor);
            g.FillRectangle(bg, rect);

            // no border (BorderThickness handled elsewhere); draw rounded region if possible
            // draw text
            var text = Text ?? string.Empty;
            var textRect = new Rectangle(rect.X + 6, rect.Y, rect.Width - 28, rect.Height);
            TextRenderer.DrawText(g, text, Font, textRect, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            // draw arrow
            var arrowRect = new Rectangle(rect.Right - 18, rect.Y + (rect.Height - 8) / 2, 12, 8);
            Point[] triangle = new Point[] {
                new Point(arrowRect.Left + 2, arrowRect.Top + 2),
                new Point(arrowRect.Right - 2, arrowRect.Top + 2),
                new Point(arrowRect.Left + (arrowRect.Width / 2), arrowRect.Bottom - 2)
            };
            using var arrowBrush = new SolidBrush(ForeColor);
            g.FillPolygon(arrowBrush, triangle);
        }
        catch { base.OnPaint(e); }
    }
}

public class Guna2DateTimePicker : DateTimePicker
{
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int BorderRadius { get; set; }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int BorderThickness { get; set; }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Color BorderColor { get; set; }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Color FillColor { get; set; }
    public Guna2DateTimePicker()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        try
        {
            var g = e.Graphics;
            var rect = ClientRectangle;
            using var bg = new SolidBrush(FillColor.IsEmpty ? ColorTranslator.FromHtml("#222222") : FillColor);
            g.FillRectangle(bg, rect);

            // draw text (date)
            var text = Value.ToString("dd/MM/yyyy");
            var textRect = new Rectangle(rect.X + 6, rect.Y, rect.Width - 28, rect.Height);
            TextRenderer.DrawText(g, text, Font, textRect, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            // draw arrow
            var arrowRect = new Rectangle(rect.Right - 18, rect.Y + (rect.Height - 8) / 2, 12, 8);
            Point[] triangle = new Point[] {
                new Point(arrowRect.Left + 2, arrowRect.Top + 2),
                new Point(arrowRect.Right - 2, arrowRect.Top + 2),
                new Point(arrowRect.Left + (arrowRect.Width / 2), arrowRect.Bottom - 2)
            };
            using var arrowBrush = new SolidBrush(ForeColor);
            g.FillPolygon(arrowBrush, triangle);
        }
        catch { base.OnPaint(e); }
    }
}
