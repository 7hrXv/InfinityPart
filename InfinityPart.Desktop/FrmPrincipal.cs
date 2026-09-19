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
            btnNavProdutos.Click += (_, _) => IrParaModuloProdutos();
            btnNavClientes.Click += (_, _) => IrParaModuloClientes();
            btnNavPedidos.Click += (_, _) => IrParaModuloPedidos();
            btnNavMarcas.Click += (_, _) => IrParaModuloMarcas();
            btnNavSair.Click += (_, _) => Sair();

            // Cabeçalho: abrir perfil ao clicar no nome do usuário
            lblUser.Cursor = Cursors.Hand;
            lblUser.Click += (_, _) => MostrarPerfil();

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

            if (confirmar != DialogResult.Yes)
                return;

            // Encerrar sessão e voltar para a tela de login
            SessaoAtual.Encerrar();

            Hide();
            using var login = new FrmLogin();
            if (login.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }

            // Novo login realizado: atualizar cabeçalho e recarregar dashboard
            lblUser.Text = $"Olá, {SessaoAtual.NomeExibicao}";
            PositionHeaderUser();
            _ = CarregarDashboardAsync();
            Show();
        }

        private void MostrarPerfil()
        {
            using var frm = new FrmPerfilAdministrador();
            frm.ShowDialog();
            // Depois de fechar, atualizar exibição do nome
            lblUser.Text = $"Olá, {SessaoAtual.NomeExibicao}";
            PositionHeaderUser();
        }

        // Navegação para módulos: carregam forms no painel host
        private void AbrirNoHost(Form form)
        {
            pnlHost.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlHost.Controls.Add(form);
            pnlHost.Visible = true;
            pnlDashboard.Visible = false;
            pnlPlaceholder.Visible = false;
            form.Show();
        }

        private void IrParaModuloProdutos()
        {
            MarcarAtivo(btnNavProdutos);
            lblSectionTitle.Text = "Produtos";
            AbrirNoHost(new FrmProdutos());
        }

        private void IrParaModuloClientes()
        {
            MarcarAtivo(btnNavClientes);
            lblSectionTitle.Text = "Clientes";
            AbrirNoHost(new FrmClientes());
        }

        private void IrParaModuloPedidos()
        {
            MarcarAtivo(btnNavPedidos);
            lblSectionTitle.Text = "Pedidos";
            AbrirNoHost(new FrmPedidos());
        }

        private void IrParaModuloMarcas()
        {
            MarcarAtivo(btnNavMarcas);
            lblSectionTitle.Text = "Marcas";
            AbrirNoHost(new FrmMarcas());
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
