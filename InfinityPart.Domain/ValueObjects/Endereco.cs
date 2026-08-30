namespace InfinityPart.Domain.ValueObjects
{
    public class Endereco
    {
        // Propriedades do endereço
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }

        // Construtor: recebe e guarda os dados do endereço
        public Endereco(string logradouro, string numero, string bairro, string cidade, string estado, string cep)
        {
            Logradouro = logradouro;
            Numero = numero;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
        }
    }
}