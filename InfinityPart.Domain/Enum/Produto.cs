using InfinityPart.Domain.Enums;

namespace InfinityPart.Domain.Entidades
{
    // Representa uma peça de PC na sua loja
    public class Produto
    {
        // Propriedades básicas da peça
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        // Aqui usamos o Enum em vez de um texto comum!
        public CondicaoProduto Condicao { get; set; }
        public int QuantidadeEstoque { get; internal set; }
    }
}