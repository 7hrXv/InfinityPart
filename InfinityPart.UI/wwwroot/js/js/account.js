/* ============================================================
   INFINITY PARTS — account.js
   Minha Conta totalmente dinâmica
   ============================================================ */


/* ============================================================
   CONFIGURAÇÃO DA API
   ============================================================ */

const ACCOUNT_API_URL = 'http://localhost:5022/api';


/* ============================================================
   USUÁRIO LOGADO
   ============================================================ */

function getAccountUser() {

    try {

        const user =
            JSON.parse(
                localStorage.getItem('ip_user') || 'null'
            );

        return user;

    } catch (error) {

        console.error(
            'Erro ao carregar usuário:',
            error
        );

        return null;
    }
}


/* ============================================================
   ID DO CLIENTE LOGADO
   ============================================================ */

function getLoggedClientId() {

    const user = getAccountUser();

    if (!user) {
        return null;
    }

    return Number(
        user.id ??
        user.clienteId ??
        user.Id ??
        user.ClienteId
    );
}


/* ============================================================
   PROTEÇÃO DA PÁGINA
   ============================================================ */

function protectAccountPage() {

    const user = getAccountUser();

    if (!user) {

        window.location.replace(
            'login.html'
        );

        return false;
    }

    return true;
}


/* ============================================================
   CABEÇALHO DA CONTA
   ============================================================ */

function renderAccountGreeting() {

    const user = getAccountUser();

    const greeting =
        document.getElementById(
            'account-greeting'
        );

    if (!greeting || !user) {
        return;
    }

    const name =
        user.name ??
        user.nome ??
        user.Nome ??
        'Cliente';

    const firstName =
        String(name)
            .trim()
            .split(' ')[0];

    greeting.textContent =
        `Olá, ${firstName}!`;
}


/* ============================================================
   PEDIDOS
   ============================================================ */

async function loadCustomerOrders() {

    const ordersList =
        document.getElementById(
            'orders-list'
        );

    if (!ordersList) {
        return;
    }

    const clientId =
        getLoggedClientId();

    if (!clientId) {

        ordersList.innerHTML = `
            <div class="order-card">
                <div>
                    <span class="order-number">
                        Sessão inválida
                    </span>

                    <span class="order-date">
                        Faça login novamente para visualizar seus pedidos.
                    </span>
                </div>
            </div>
        `;

        return;
    }


    try {

        ordersList.innerHTML = `
            <div class="loading-state">
                <i class="bi bi-arrow-repeat spin"></i>
                Carregando pedidos...
            </div>
        `;


        const response =
            await fetch(
                `${ACCOUNT_API_URL}/Pedido`
            );


        if (!response.ok) {

            throw new Error(
                `Erro HTTP ${response.status}`
            );
        }


        const orders =
            await response.json();


        /*
         * IMPORTANTE:
         *
         * Aqui filtramos somente os pedidos
         * pertencentes ao cliente logado.
         */

        const customerOrders =
            Array.isArray(orders)
                ? orders.filter(
                    order =>
                        Number(
                            order.clienteId ??
                            order.ClienteId
                        ) === clientId
                )
                : [];


        renderCustomerOrders(
            customerOrders
        );

    } catch (error) {

        console.error(
            'Erro ao carregar pedidos:',
            error
        );


        ordersList.innerHTML = `
            <div class="order-card">

                <div>

                    <span class="order-number">
                        Não foi possível carregar seus pedidos
                    </span>

                    <span class="order-date">
                        Verifique se a API está funcionando.
                    </span>

                </div>

            </div>
        `;
    }
}


/* ============================================================
   RENDER DOS PEDIDOS
   ============================================================ */

function renderCustomerOrders(
    orders
) {

    const ordersList =
        document.getElementById(
            'orders-list'
        );

    if (!ordersList) {
        return;
    }


    /*
     * CLIENTE NOVO:
     * nenhum pedido cadastrado.
     */

    if (
        !Array.isArray(orders) ||
        orders.length === 0
    ) {

        ordersList.innerHTML = `
            <div class="order-card">

                <div>

                    <span class="order-number">
                        Você ainda não possui pedidos
                    </span>

                    <span class="order-date">
                        Quando você realizar uma compra,
                        seus pedidos aparecerão aqui.
                    </span>

                </div>

                <a
                    href="produtos.html"
                    class="btn btn-outline btn-sm"
                >
                    VER PRODUTOS
                </a>

            </div>
        `;

        return;
    }


    /*
     * Mais recentes primeiro.
     */

    const sortedOrders =
        [...orders].sort(
            (a, b) =>
                new Date(
                    b.dataPedido ??
                    b.DataPedido
                ) -
                new Date(
                    a.dataPedido ??
                    a.DataPedido
                )
        );


    ordersList.innerHTML =
        sortedOrders
            .map(
                order =>
                    renderOrderCard(order)
            )
            .join('');
}


/* ============================================================
   CARD DO PEDIDO
   ============================================================ */

function renderOrderCard(
    order
) {

    const id =
        order.id ??
        order.Id;


    const date =
        order.dataPedido ??
        order.DataPedido;


    const total =
        Number(
            order.valorTotal ??
            order.ValorTotal ??
            0
        );


    const statusId =
        Number(
            order.statusPedidoId ??
            order.StatusPedidoId ??
            0
        );


    const status =
        getOrderStatus(
            statusId,
            order
        );


    return `
        <div class="order-card">

            <div>

                <span class="order-number">
                    Pedido #${id}
                </span>

                <span class="order-date">
                    ${formatOrderDate(date)}
                </span>

            </div>


            <span
                class="order-status ${status.className}"
            >
                ${status.label}
            </span>


            <strong>
                ${formatCurrency(total)}
            </strong>

        </div>
    `;
}


/* ============================================================
   STATUS DO PEDIDO
   ============================================================ */

function getOrderStatus(
    statusId,
    order
) {

    /*
     * Caso a API futuramente envie o objeto
     * StatusPedido junto com o pedido.
     */

    const statusName =
        order.statusPedido?.nome ??
        order.statusPedido?.Nome ??
        order.status ??
        order.Status ??
        'Pendente';


    const normalized =
        String(statusName)
            .toLowerCase()
            .trim();


    if (
        normalized.includes('entreg')
    ) {

        return {
            label: 'ENTREGUE',
            className: 'status-entregue'
        };
    }


    if (
        normalized.includes('cancel')
    ) {

        return {
            label: 'CANCELADO',
            className: 'status-cancelado'
        };
    }


    if (
        normalized.includes('enviado') ||
        normalized.includes('transporte')
    ) {

        return {
            label: 'A CAMINHO',
            className: 'status-enviado'
        };
    }


    if (
        normalized.includes('prepar')
    ) {

        return {
            label: 'EM PREPARAÇÃO',
            className: 'status-preparacao'
        };
    }


    /*
     * Enquanto a API retornar somente
     * statusPedidoId, o ID 2 que usamos
     * atualmente representa Pendente.
     */

    if (statusId === 2) {

        return {
            label: 'PENDENTE',
            className: 'status-pendente'
        };
    }


    return {
        label: String(statusName).toUpperCase(),
        className: 'status-pendente'
    };
}


/* ============================================================
   FORMATAÇÃO DE DATA
   ============================================================ */

function formatOrderDate(
    value
) {

    if (!value) {
        return 'Data não informada';
    }


    const date =
        new Date(value);


    if (Number.isNaN(date.getTime())) {

        return 'Data não informada';
    }


    return date.toLocaleDateString(
        'pt-BR'
    );
}


/* ============================================================
   FORMATAÇÃO DE VALOR
   ============================================================ */

function formatCurrency(
    value
) {

    return Number(value || 0)
        .toLocaleString(
            'pt-BR',
            {
                style: 'currency',
                currency: 'BRL'
            }
        );
}


/* ============================================================
   DADOS PESSOAIS
   ============================================================ */

function loadCustomerData() {

    const user =
        getAccountUser();

    if (!user) {
        return;
    }


    const nameInput =
        document.getElementById(
            'account-name'
        );


    const emailInput =
        document.getElementById(
            'account-email'
        );


    if (nameInput) {

        nameInput.value =
            user.name ??
            user.nome ??
            user.Nome ??
            '';
    }


    if (emailInput) {

        emailInput.value =
            user.email ??
            user.Email ??
            '';
    }
}


/* ============================================================
   ENDEREÇO
   ============================================================ */

async function loadCustomerAddress() {

    const container =
        document.getElementById(
            'address-container'
        );

    if (!container) {
        return;
    }


    const clientId =
        getLoggedClientId();


    if (!clientId) {

        container.innerHTML = `
            <div class="order-card">

                <span class="order-date">
                    Nenhum endereço disponível.
                </span>

            </div>
        `;

        return;
    }


    try {

        const response =
            await fetch(
                `${ACCOUNT_API_URL}/Cliente/${clientId}`
            );


        if (!response.ok) {

            throw new Error(
                `Erro HTTP ${response.status}`
            );
        }


        const client =
            await response.json();


        renderCustomerAddress(
            client
        );

    } catch (error) {

        console.error(
            'Erro ao carregar endereço:',
            error
        );


        container.innerHTML = `
            <div class="order-card">

                <div>

                    <span class="order-number">
                        Endereço
                    </span>

                    <span class="order-date">
                        Não foi possível carregar o endereço.
                    </span>

                </div>

            </div>
        `;
    }
}


/* ============================================================
   RENDER ENDEREÇO
   ============================================================ */

function renderCustomerAddress(
    client
) {

    const container =
        document.getElementById(
            'address-container'
        );

    if (!container) {
        return;
    }


    const endereco =
        client.endereco ??
        client.Endereco ??
        '';


    const numero =
        client.numero ??
        client.Numero ??
        '';


    const cidade =
        client.cidade ??
        client.Cidade ??
        '';


    const estado =
        client.estado ??
        client.Estado ??
        '';


    const cep =
        client.cep ??
        client.Cep ??
        '';


    if (
        !endereco &&
        !numero &&
        !cidade &&
        !estado &&
        !cep
    ) {

        container.innerHTML = `
            <div class="order-card">

                <div>

                    <span class="order-number">
                        Nenhum endereço cadastrado
                    </span>

                    <span class="order-date">
                        Cadastre seu endereço para facilitar suas compras.
                    </span>

                </div>

            </div>
        `;

        return;
    }


    container.innerHTML = `
        <div class="order-card">

            <div>

                <span class="order-number">
                    Endereço principal
                </span>

                <span class="order-date">

                    ${escapeHtml(endereco)}

                    ${numero
                        ? `, ${escapeHtml(numero)}`
                        : ''
                    }

                    ${
                        cidade
                            ? ` — ${escapeHtml(cidade)}`
                            : ''
                    }

                    ${
                        estado
                            ? `/${escapeHtml(estado)}`
                            : ''
                    }

                    ${
                        cep
                            ? ` — CEP ${escapeHtml(cep)}`
                            : ''
                    }

                </span>

            </div>

            <span class="order-status status-entregue">
                CADASTRADO
            </span>

        </div>
    `;
}


/* ============================================================
   ESCAPE HTML
   ============================================================ */

function escapeHtml(
    value
) {

    return String(value ?? '')
        .replaceAll('&', '&amp;')
        .replaceAll('<', '&lt;')
        .replaceAll('>', '&gt;')
        .replaceAll('"', '&quot;')
        .replaceAll("'", '&#039;');
}


/* ============================================================
   MENU DA CONTA
   ============================================================ */

function initAccountPanels() {

    const links =
        document.querySelectorAll(
            '.account-menu a[data-panel]'
        );


    const panels =
        document.querySelectorAll(
            '.account-panel'
        );


    links.forEach(
        link => {

            link.addEventListener(
                'click',
                event => {

                    event.preventDefault();


                    const targetId =
                        link.dataset.panel;


                    links.forEach(
                        item =>
                            item.classList.remove(
                                'active'
                            )
                    );


                    panels.forEach(
                        panel =>
                            panel.classList.remove(
                                'active'
                            )
                    );


                    link.classList.add(
                        'active'
                    );


                    const target =
                        document.getElementById(
                            targetId
                        );


                    if (target) {

                        target.classList.add(
                            'active'
                        );
                    }

                }
            );
        }
    );
}


/* ============================================================
   LOGOUT
   ============================================================ */

function initAccountLogout() {

    const button =
        document.getElementById(
            'logout-btn'
        );


    if (!button) {
        return;
    }


    button.addEventListener(
        'click',
        event => {

            event.preventDefault();


            localStorage.removeItem(
                'ip_user'
            );


            if (
                typeof showToast ===
                'function'
            ) {

                showToast(
                    'Você saiu da sua conta.'
                );

            }


            setTimeout(
                () => {

                    window.location.href =
                        'login.html';

                },
                500
            );

        }
    );
}


/* ============================================================
   SALVAR DADOS
   ============================================================ */

function initSaveAccountData() {

    const button =
        document.getElementById(
            'save-account-data'
        );


    if (!button) {
        return;
    }


    button.addEventListener(
        'click',
        () => {

            const nameInput =
                document.getElementById(
                    'account-name'
                );


            const emailInput =
                document.getElementById(
                    'account-email'
                );


            const user =
                getAccountUser();


            if (!user) {
                return;
            }


            user.name =
                nameInput?.value.trim() ||
                user.name;


            user.email =
                emailInput?.value.trim() ||
                user.email;


            localStorage.setItem(
                'ip_user',
                JSON.stringify(user)
            );


            renderAccountGreeting();


            if (
                typeof updateHeaderAccount ===
                'function'
            ) {

                updateHeaderAccount();
            }


            if (
                typeof showToast ===
                'function'
            ) {

                showToast(
                    'Dados atualizados com sucesso.'
                );

            }

        }
    );
}


/* ============================================================
   INICIALIZAÇÃO
   ============================================================ */

document.addEventListener(
    'DOMContentLoaded',
    async () => {

        /*
         * Se não estiver logado,
         * volta para o login.
         */

        if (!protectAccountPage()) {
            return;
        }


        renderAccountGreeting();

        loadCustomerData();

        initAccountPanels();

        initAccountLogout();

        initSaveAccountData();


        /*
         * Carrega somente os dados
         * pertencentes ao cliente logado.
         */

        await Promise.all([
            loadCustomerOrders(),
            loadCustomerAddress()
        ]);

    }
);