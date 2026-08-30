using InfinityPart.Domain.Entidades;

namespace InfinityPart.Entidades;

public class Pedido
{
    public int Id { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Pendente"; // Pendente, Concluído, Cancelado

    public int ApplicationUserId { get; set; }
    public ApplicationUser? Usuario { get; set; }
}