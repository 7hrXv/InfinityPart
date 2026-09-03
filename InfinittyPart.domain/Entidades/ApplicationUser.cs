using InfinityPart.Entidades;
using Microsoft.AspNetCore.Identity;

namespace InfinityPart.Domain.Entidades; 
public class ApplicationUser : IdentityUser
{
    // Campos adicionais do seu usuário além de E-mail e Senha
    public string NomeCompleto { get; set; } = string.Empty;
    public string? CPF { get; set; }
    public string? Telefone { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    // Relacionamento: Um usuário pode ter vários pedidos
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}