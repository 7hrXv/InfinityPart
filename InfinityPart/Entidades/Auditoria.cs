namespace InfinityPart.Domain.Entities
{
    public class Auditoria
    {
        public int Id { get; set; }

        public string? UsuarioId { get; set; }

        public string? NomeUsuario { get; set; }

        public required string Acao { get; set; }

        public required string TabelaAfetada { get; set; }

        public string? Detalhes { get; set; }

        public DateTime DataHora { get; set; } = DateTime.UtcNow;
    }
}