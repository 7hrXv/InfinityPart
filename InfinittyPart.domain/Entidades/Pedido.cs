using InfinityPart.Domain.Entidades;

namespace InfinityPart.Entidades;

public class Pedido
{
    public int Id { get; set; }

    public DateTime DataPedido { get; set; } = DateTime.UtcNow;

    public decimal ValorTotal { get; set; }

    public string Status { get; set; } = "Pendente";

    // Relacionamento com o usuário do ASP.NET Identity
    public string ApplicationUserId { get; set; } = string.Empty;

    public ApplicationUser? Usuario { get; set; }
}