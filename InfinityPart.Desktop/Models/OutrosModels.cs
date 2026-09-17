namespace InfinityPart.Desktop.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public int ClienteId { get; set; }
        public int StatusPedidoId { get; set; }
    }

    public class MarcaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Cnpj { get; set; }
    }

    public class CriarMarcaModel
    {
        public string Nome { get; set; } = string.Empty;
        public string? Cnpj { get; set; }
    }

    public class StatusPedidoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class AdministradorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime? UltimoAcesso { get; set; }
        public bool PossuiSenhaDefinida { get; set; }
    }
}
