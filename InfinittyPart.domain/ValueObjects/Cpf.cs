using Infinitypart.Domain.Excecoes;
using InfinityPart.Domain.Excecoes;


namespace InfinityPart.Domain.ValueObjects
{
    public class Cpf
    {
        public string Numero { get; private set; }

        public Cpf(string numero)
        {
            // Remove os pontos e traços para deixar só os números
            var cpfLimpo = numero?.Replace(".", "").Replace("-", "").Trim();

            // Regra: CPF não pode ser vazio e precisa ter exatamente 11 dígitos
            if (string.IsNullOrEmpty(cpfLimpo) || cpfLimpo.Length != 11)
            {
                throw new RegraNegocioException("O CPF informado é inválido. Deve conter 11 dígitos.");
            }

            Numero = cpfLimpo;
        }
    }
}