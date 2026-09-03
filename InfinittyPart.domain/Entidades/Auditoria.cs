namespace InfinityPart.Entidades; // Mantenha o mesmo namespace das outras classes

public class Auditoria
{
    public int Id { get; set; }

    // Identifica o usuário que realizou a ação
    public string UsuarioId { get; set; } = string.Empty;
    public string UsuarioNome { get; set; } = string.Empty;

    // Ação realizada (ex: "Criar", "Atualizar", "Deletar")
    public string Acao { get; set; } = string.Empty;

    // Nome da tabela ou entidade afetada (ex: "Peca", "Pedido")
    public string Tabela { get; set; } = string.Empty;

    // Data e hora exatas do evento
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    // Armazena como os dados estavam antes e como ficaram depois (geralmente em formato JSON)
    public string? ValoresAntigos { get; set; }
    public string? ValoresNovos { get; set; }
}