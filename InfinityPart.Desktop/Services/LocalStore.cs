using InfinityPart.Desktop.Models;

namespace InfinityPart.Desktop.Services
{
    /// <summary>
    /// Armazenamento local temporário para o modo sem API.
    /// Dados mantidos em memória apenas para permitir a interface funcionar offline.
    /// </summary>
    public static class LocalStore
    {
        public static List<ProdutoModel> Produtos { get; } = new();
        public static List<ClienteModel> Clientes { get; } = new();
        public static List<MarcaModel> Marcas { get; } = new();
        public static List<PedidoModel> Pedidos { get; } = new();
        public static List<StatusPedidoModel> StatusPedidos { get; } = new();

        static LocalStore()
        {
            // Seeds mínimos para permitir visualização
            Marcas.Add(new MarcaModel { Id = 1, Nome = "Marca A", Cnpj = "00.000.000/0001-00" });
            Marcas.Add(new MarcaModel { Id = 2, Nome = "Marca B", Cnpj = "11.111.111/0001-11" });

            Produtos.Add(new ProdutoModel { Id = 1, Nome = "Parafuso M4", Codigo = "P-M4", Preco = 1.5m, QuantidadeEstoque = 120, MarcaId = 1 });
            Produtos.Add(new ProdutoModel { Id = 2, Nome = "Porca M4", Codigo = "PN-M4", Preco = 0.5m, QuantidadeEstoque = 300, MarcaId = 1 });

            Clientes.Add(new ClienteModel { Id = 1, Nome = "ACME Ltda.", Cpf = "", Email = "contato@acme.com", Telefone = "(11) 99999-0000" });

            StatusPedidos.Add(new StatusPedidoModel { Id = 1, Nome = "Aguardando" });
            StatusPedidos.Add(new StatusPedidoModel { Id = 2, Nome = "Concluído" });

            Pedidos.Add(new PedidoModel { Id = 1, DataPedido = DateTime.Now.AddDays(-2), ValorTotal = 150.75m, ClienteId = 1, StatusPedidoId = 2 });
            Pedidos.Add(new PedidoModel { Id = 2, DataPedido = DateTime.Now.AddDays(-1), ValorTotal = 45.00m, ClienteId = 1, StatusPedidoId = 1 });
        }

        // Helpers para gerar novos ids
        public static int NextProdutoId() => Produtos.Count == 0 ? 1 : Produtos.Max(p => p.Id) + 1;
        public static int NextClienteId() => Clientes.Count == 0 ? 1 : Clientes.Max(c => c.Id) + 1;
        public static int NextMarcaId() => Marcas.Count == 0 ? 1 : Marcas.Max(m => m.Id) + 1;
        public static int NextPedidoId() => Pedidos.Count == 0 ? 1 : Pedidos.Max(p => p.Id) + 1;
    }
}
