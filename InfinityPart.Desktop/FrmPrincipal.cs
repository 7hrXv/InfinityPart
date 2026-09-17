using InfinityPart.Desktop.Controls;
using InfinityPart.Desktop.Models;
using InfinityPart.Desktop.Services;
using InfinityPart.Desktop.Theme;

namespace InfinityPart.Desktop
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();

            btnNavDashboard.Click += (_, _) => IrParaDashboard();
            btnNavProdutos.Click += (_, _) => IrParaModuloEmConstrucao(btnNavProdutos, "Produtos");
            btnNavClientes.Click += (_, _) => IrParaModuloEmConstrucao(btnNavClientes, "Clientes");
            btnNavPedidos.Click += (_, _) => IrParaModuloEmConstrucao(btnNavPedidos, "Pedidos");
            btnNavMarcas.Click += (_, _) => IrParaModuloEmConstrucao(btnNavMarcas, "Marcas");
            btnNavSair.Click += (_, _) => Sair();

            Load += FrmPrincipal_Load;
        }

        private async void FrmPrincipal_Load(object? sender, EventArgs e)
        {
            lblUser.Text = $"Olá, {SessaoAtual.NomeExibicao}";
            PositionHeaderUser();

            await CarregarDashboardAsync();
        }

        // ---------- Navegação ----------

        private SidebarButton[] TodosBotoesNav => new[]
        {
            btnNavDashboard, btnNavProdutos, btnNavClientes, btnNavPedidos, btnNavMarcas
        };

        private void MarcarAtivo(SidebarButton botaoAtivo)
        {
            foreach (var botao in TodosBotoesNav)
                botao.IsActive = botao == botaoAtivo;
        }

        private void IrParaDashboard()
        {
            MarcarAtivo(btnNavDashboard);
            lblSectionTitle.Text = "Dashboard";
            pnlPlaceholder.Visible = false;
            pnlDashboard.Visible = true;
        }

        private void IrParaModuloEmConstrucao(SidebarButton botao, string nomeModulo)
        {
            MarcarAtivo(botao);
            lblSectionTitle.Text = nomeModulo;
            pnlDashboard.Visible = false;
            lblPlaceholder.Text = $"O módulo de {nomeModulo} será implementado na próxima etapa,\n" +
                                   "reaproveitando os endpoints já existentes na API.";
            pnlPlaceholder.Visible = true;
        }

        private void Sair()
        {
            var confirmar = MessageBox.Show(
                "Deseja realmente sair do sistema?",
                "Confirmar saída",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                SessaoAtual.Encerrar();
                Application.Exit();
            }
        }

        // ---------- Dashboard ----------

        private async Task CarregarDashboardAsync()
        {
            try
            {
                lblStatusApi.Text = "Carregando dados da API...";
                lblStatusApi.ForeColor = AppTheme.TextMuted;

                var produtosTask = ApiClient.GetListAsync<ProdutoModel>("produto");
                var clientesTask = ApiClient.GetListAsync<ClienteModel>("cliente");
                var pedidosTask = ApiClient.GetListAsync<PedidoModel>("pedido");
                var marcasTask = ApiClient.GetListAsync<MarcaModel>("marca");
                var statusTask = ApiClient.GetListAsync<StatusPedidoModel>("statuspedido");

                await Task.WhenAll(produtosTask, clientesTask, pedidosTask, marcasTask, statusTask);

                var produtos = produtosTask.Result;
                var clientes = clientesTask.Result;
                var pedidos = pedidosTask.Result;
                var marcas = marcasTask.Result;
                var statusPedidos = statusTask.Result;

                cardProdutos.Value = produtos.Count.ToString();
                cardClientes.Value = clientes.Count.ToString();
                cardPedidos.Value = pedidos.Count.ToString();
                cardMarcas.Value = marcas.Count.ToString();

                PreencherGridPedidos(pedidos, clientes, statusPedidos);

                lblStatusApi.Text = $"Dados atualizados em {DateTime.Now:dd/MM/yyyy HH:mm}  •  {ApiClient.BaseUrl}";
                lblStatusApi.ForeColor = AppTheme.TextMuted;
            }
            catch (Exception ex)
            {
                cardProdutos.Value = "--";
                cardClientes.Value = "--";
                cardPedidos.Value = "--";
                cardMarcas.Value = "--";

                lblStatusApi.Text = $"Não foi possível conectar à API em {ApiClient.BaseUrl}. Verifique se o InfinityPart.API está em execução.";
                lblStatusApi.ForeColor = AppTheme.PrimaryRedHover;

                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void PreencherGridPedidos(List<PedidoModel> pedidos, List<ClienteModel> clientes, List<StatusPedidoModel> statusPedidos)
        {
            var clientePorId = clientes.ToDictionary(c => c.Id, c => c.Nome);
            var statusPorId = statusPedidos.ToDictionary(s => s.Id, s => s.Nome);

            dgvPedidosRecentes.DataSource = null;
            dgvPedidosRecentes.Columns.Clear();

            dgvPedidosRecentes.Columns.Add("Id", "Nº");
            dgvPedidosRecentes.Columns.Add("Data", "Data");
            dgvPedidosRecentes.Columns.Add("Cliente", "Cliente");
            dgvPedidosRecentes.Columns.Add("Valor", "Valor total");
            dgvPedidosRecentes.Columns.Add("Status", "Status");

            dgvPedidosRecentes.Columns["Id"].FillWeight = 40;
            dgvPedidosRecentes.Columns["Data"].FillWeight = 70;
            dgvPedidosRecentes.Columns["Cliente"].FillWeight = 140;
            dgvPedidosRecentes.Columns["Valor"].FillWeight = 70;
            dgvPedidosRecentes.Columns["Status"].FillWeight = 70;

            foreach (var pedido in pedidos.OrderByDescending(p => p.DataPedido).Take(50))
            {
                var nomeCliente = clientePorId.TryGetValue(pedido.ClienteId, out var nome) ? nome : $"#{pedido.ClienteId}";
                var nomeStatus = statusPorId.TryGetValue(pedido.StatusPedidoId, out var status) ? status : $"#{pedido.StatusPedidoId}";

                dgvPedidosRecentes.Rows.Add(
                    pedido.Id,
                    pedido.DataPedido.ToString("dd/MM/yyyy"),
                    nomeCliente,
                    pedido.ValorTotal.ToString("C2"),
                    nomeStatus);
            }

            if (pedidos.Count == 0)
            {
                dgvPedidosRecentes.Rows.Add("", "Nenhum pedido cadastrado ainda.", "", "", "");
            }
        }
    }
}
