namespace InfinityPart.Domain.Entities
{
    public class PerfilUsuario : IdentityUser
    {
        public required string Nome { get; set; }
        public int Cpf { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string? FotoPerfil { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataExclusao { get; set; }






    }
}
