/* ============================================================
   INFINITY PARTS — account.js
   Minha Conta
   ============================================================ */

const ACCOUNT_API_URL = 'http://localhost:5022/api';
const ACCOUNT_REQUEST_TIMEOUT = 8000;


/* ============================================================
   USUÁRIO LOGADO
   ============================================================ */

function getAccountUser() {

    try {

        return JSON.parse(
            localStorage.getItem('ip_user') || 'null'
        );

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

    const id =
        user.id ??
        user.clienteId ??
        user.Id ??
        user.ClienteId;

    const clientId = Number(id);

    return Number.isFinite(clientId) && clientId > 0
        ? clientId
        : null;
}


/* ============================================================
   REQUISIÇÃO COM TIMEOUT
   ============================================================ */

async function accountFetch(
    url,
    options = {}
) {

    const controller =
        new AbortController();

    const timeout =
        setTimeout(
            () => controller.abort(),
            ACCOUNT_REQUEST_TIMEOUT
        );

    try {

        const response =
            await fetch(
                url,
                {
                    ...options,
                    signal: controller.signal,
                    headers: {
                        'Accept': 'application/json',
                        ...(options.headers || {})
                    }
                }
            );

        return response;

    } finally {

        clearTimeout(timeout);

    }
}


/* ============================================================
   PROTEÇÃO DA PÁGINA
   ============================================================ */

function protectAccountPage() {

    const user =
        getAccountUser();

    if (!user) {

        window.location.replace(
            'login.html'
        );

        return false;
    }

    return true;
}


/* ============================================================
   SAUDAÇÃO
   ============================================================ */

function renderAccountGreeting() {

    const user =
        getAccountUser();

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


    ordersList.innerHTML = `
        <div class="loading-state">
            <i class="bi bi-arrow-repeat spin"></i>
            Carregando pedidos...
        </div>
    `;


    try {

        /*
         * Busca diretamente os pedidos
         * do cliente logado.
         *
         * GET /api/Pedido/cliente/{id}
         */

        const response =
            await accountFetch(
                `${ACCOUNT_API_URL}/Pedido/cliente/${clientId}`
            );


        if (!response.ok) {

            throw new Error(
                `Erro HTTP ${response.status}`
            );
        }


        const orders =
            await response.json();


        renderCustomerOrders(
            Array.isArray(orders)
                ? orders
                : []
        );


    } catch (error) {

        console.error(
            'Erro ao carregar pedidos:',
            error
        );


        const mensagem =
            error.name === 'AbortError'
                ? 'A API demorou demais para responder.'
                : 'Não foi possível carregar seus pedidos.';


        ordersList.innerHTML = `
            <div class="order-card">

                <div>

                    <span class="order-number">
                        Não foi possível carregar seus pedidos
                    </span>

                    <span class="order-date">
                        ${mensagem}
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
            1
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

    const statusName =
        order.statusPedido?.nome ??
        order.statusPedido?.Nome ??
        order.status ??
        order.Status ??
        '';


    const normalized =
        String(statusName)
            .toLowerCase()
            .trim();


    if (
        normalized.includes('entreg')
        ||
        statusId === 4
    ) {

        return {
            label: 'ENTREGUE',
            className: 'status-entregue'
        };
    }


    if (
        normalized.includes('cancel')
        ||
        statusId === 5
    ) {

        return {
            label: 'CANCELADO',
            className: 'status-cancelado'
        };
    }


    if (
        normalized.includes('enviado')
        ||
        normalized.includes('transporte')
        ||
        statusId === 3
    ) {

        return {
            label: 'A CAMINHO',
            className: 'status-enviado'
        };
    }


    if (
        normalized.includes('process')
        ||
        normalized.includes('prepar')
        ||
        statusId === 2
    ) {

        return {
            label: 'EM PROCESSAMENTO',
            className: 'status-preparacao'
        };
    }


    return {
        label: 'PENDENTE',
        className: 'status-pendente'
    };
}


/* ============================================================
   DATA
   ============================================================ */

function formatOrderDate(
    value
) {

    if (!value) {
        return 'Data não informada';
    }


    const date =
        new Date(value);


    if (
        Number.isNaN(
            date.getTime()
        )
    ) {

        return 'Data não informada';
    }


    return date.toLocaleDateString(
        'pt-BR'
    );
}


/* ============================================================
   MOEDA
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


    container.innerHTML = `
        <div class="loading-state">

            <i class="bi bi-arrow-repeat spin"></i>

            Carregando endereço...

        </div>
    `;


    try {

        /*
         * Busca o cliente diretamente.
         *
         * GET /api/Cliente/{id}
         */

        const response =
            await accountFetch(
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


        const mensagem =
            error.name === 'AbortError'
                ? 'A API demorou demais para responder.'
                : 'Não foi possível carregar o endereço.';


        container.innerHTML = `
            <div class="order-card">

                <div>

                    <span class="order-number">
                        Endereço
                    </span>

                    <span class="order-date">
                        ${mensagem}
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


    const complemento =
        client.complemento ??
        client.Complemento ??
        '';


    const bairro =
        client.bairro ??
        client.Bairro ??
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


    let linhaEndereco =
        escapeHtml(endereco);


    if (numero) {

        linhaEndereco +=
            `, ${escapeHtml(numero)}`;
    }


    if (complemento) {

        linhaEndereco +=
            ` — ${escapeHtml(complemento)}`;
    }


    if (bairro) {

        linhaEndereco +=
            ` — ${escapeHtml(bairro)}`;
    }


    if (cidade) {

        linhaEndereco +=
            ` — ${escapeHtml(cidade)}`;
    }


    if (estado) {

        linhaEndereco +=
            `/${escapeHtml(estado)}`;
    }


    if (cep) {

        linhaEndereco +=
            ` — CEP ${escapeHtml(cep)}`;
    }


    container.innerHTML = `
        <div class="order-card">

            <div>

                <span class="order-number">
                    Endereço principal
                </span>

                <span class="order-date">
                    ${linhaEndereco}
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


            if (nameInput) {

                user.name =
                    nameInput.value.trim();
            }


            if (emailInput) {

                user.email =
                    emailInput.value.trim();
            }


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
    () => {

        if (!protectAccountPage()) {
            return;
        }


        renderAccountGreeting();

        loadCustomerData();

        initAccountLogout();

        initSaveAccountData();


        /*
         * Carregamos os dois independentemente.
         * Um não fica esperando o outro.
         */

        loadCustomerOrders();

        loadCustomerAddress();

    }
);