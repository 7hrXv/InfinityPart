using System.Windows.Forms;

// Stubs no namespace global para evitar dependência da biblioteca Guna no ambiente de build
// Estes tipos reproduzem a API mínima usada pelo projeto.
public class Guna2DataGridView : DataGridView { }
public class Guna2TextBox : TextBox
{
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string PlaceholderText { get; set; }
}
public class Guna2Button : Button { }
public class Guna2Panel : Panel { }
public class Guna2HtmlLabel : Label { }
public class Guna2PictureBox : PictureBox { }
