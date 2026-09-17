using System.Drawing;

namespace InfinityPart.Desktop.Theme
{
    /// <summary>
    /// Paleta central do sistema. Qualquer ajuste de cor/fonte do app inteiro
    /// deve ser feito aqui, nunca espalhado pelos formulários.
    /// </summary>
    public static class AppTheme
    {
        // Fundo
        public static readonly Color Background = ColorTranslator.FromHtml("#0B0B0B");
        public static readonly Color Surface = ColorTranslator.FromHtml("#161616");
        public static readonly Color SurfaceAlt = ColorTranslator.FromHtml("#1E1E1E");
        public static readonly Color Sidebar = ColorTranslator.FromHtml("#101010");

        // Vermelho escuro (cor principal da marca)
        public static readonly Color PrimaryRed = ColorTranslator.FromHtml("#8B0000");
        public static readonly Color PrimaryRedHover = ColorTranslator.FromHtml("#A61111");
        public static readonly Color PrimaryRedDark = ColorTranslator.FromHtml("#5C0000");

        // Textos
        public static readonly Color TextPrimary = ColorTranslator.FromHtml("#F5F5F5");
        public static readonly Color TextSecondary = ColorTranslator.FromHtml("#B5B5B5");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#7A7A7A");

        // Bordas / linhas
        public static readonly Color Border = ColorTranslator.FromHtml("#2A2A2A");

        // Estados
        public static readonly Color Success = ColorTranslator.FromHtml("#2E7D32");
        public static readonly Color Warning = ColorTranslator.FromHtml("#B8860B");
        public static readonly Color Danger = PrimaryRed;

        // Fontes
        public static readonly FontFamily FontFamily = new("Segoe UI");

        public static Font FontTitle => new(FontFamily, 20f, FontStyle.Bold);
        public static Font FontSubtitle => new(FontFamily, 12f, FontStyle.Regular);
        public static Font FontHeading => new(FontFamily, 14f, FontStyle.Bold);
        public static Font FontBody => new(FontFamily, 10f, FontStyle.Regular);
        public static Font FontBodyBold => new(FontFamily, 10f, FontStyle.Bold);
        public static Font FontSmall => new(FontFamily, 9f, FontStyle.Regular);
        public static Font FontCardNumber => new(FontFamily, 24f, FontStyle.Bold);
        public static Font FontNav => new(FontFamily, 10.5f, FontStyle.Regular);
    }
}
