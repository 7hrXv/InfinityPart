using System.Globalization;
using System.Text.RegularExpressions;

namespace InfinityPart.Desktop
{
    /// <summary>
    /// Utilitários de validação de entrada usados pelos formulários do Desktop.
    /// Atua somente na camada de UI (restrição de digitação e checagem antes de salvar),
    /// sem alterar a lógica de negócio/gravação já existente nos formulários.
    /// </summary>
    public static class Validacoes
    {
        private static readonly Regex RegexEmail =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        // ---------- Restrições de digitação (KeyPress) ----------

        /// <summary>Permite apenas dígitos (0-9). Uso: ID, CPF/CNPJ, Telefone, CEP, Número, Quantidade, MarcaId.</summary>
        public static void SomenteNumeros(TextBox txt)
        {
            txt.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };
        }

        /// <summary>Permite apenas letras e espaços (com acentuação). Uso: Nome.</summary>
        public static void SomenteLetras(TextBox txt)
        {
            txt.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                    e.Handled = true;
            };
        }

        /// <summary>Permite dígitos e um único separador decimal (, ou .). Uso: Preço/Valor.</summary>
        public static void SomenteDecimal(TextBox txt)
        {
            txt.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                    return;

                if ((e.KeyChar == ',' || e.KeyChar == '.') &&
                    !txt.Text.Contains(',') && !txt.Text.Contains('.'))
                    return;

                e.Handled = true;
            };
        }

        // ---------- Validações antes de salvar ----------

        public static bool ObrigatorioPreenchido(string? valor)
            => !string.IsNullOrWhiteSpace(valor);

        public static bool SomenteLetrasTexto(string? valor)
            => !string.IsNullOrWhiteSpace(valor) && valor.Trim().All(c => char.IsLetter(c) || c == ' ');

        public static bool SomenteNumerosTexto(string? valor)
            => !string.IsNullOrWhiteSpace(valor) && valor.Trim().All(char.IsDigit);

        public static bool EmailValido(string? email)
            => !string.IsNullOrWhiteSpace(email) && RegexEmail.IsMatch(email.Trim());

        public static bool DataValida(string? valor, out DateTime data)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                data = default;
                return false;
            }
            return DateTime.TryParse(valor.Trim(), CultureInfo.CurrentCulture, DateTimeStyles.None, out data)
                || DateTime.TryParse(valor.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out data);
        }

        public static bool DecimalValido(string? valor, out decimal resultado)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                resultado = 0;
                return false;
            }

            valor = valor.Trim();
            return decimal.TryParse(valor, NumberStyles.Number, CultureInfo.CurrentCulture, out resultado)
                || decimal.TryParse(valor.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out resultado);
        }

        public static bool InteiroValido(string? valor, out int resultado)
            => int.TryParse((valor ?? string.Empty).Trim(), out resultado);

        public static void MostrarErro(string mensagem)
            => MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
