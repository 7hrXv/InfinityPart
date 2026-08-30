namespace InfinityPart.Domain.Enums
{
    // Um Enum é apenas uma lista de opções fixas com nomes amigáveis
    public enum CondicaoProduto
    {
        Novo = 1,           // Produto novo, lacrado na caixa
        Seminovo = 2,       // Produto usado, mas em perfeito estado
        OpenBox = 3         // Produto com caixa aberta/reembalado
    }
}