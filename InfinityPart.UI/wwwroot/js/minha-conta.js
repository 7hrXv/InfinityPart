// @ts-nocheck

const ACCOUNT_API_URL = 'http://localhost:5022/api';

document.addEventListener(
    'DOMContentLoaded',
    function () {

        inicializarMinhaConta();

    }
);


// =========================================================
// INICIALIZAÇÃO
// =========================================================

async function inicializarMinhaConta() {

    const usuario = obterUsuario();

    if (!usuario) {

        window.location.href = 'login.html';

        return;
    }

    configurarMenuConta();

    configurarLogout();

    configurarSalvarDados();

    configurarAtualizarSenha();

    carregarDadosUsuario();

    carregarEndereco();

    await carregarPedidos();
}


// =========================================================
// USUÁRIO
// =========================================================

function obterUsuario() {

    try {

        const dados =
            localStorage.getItem('ip_user');

        if (!dados) {

            return null;
        }

        return JSON.parse(dados);

    } catch (error) {

        console.error(
            'Erro ao ler ip_user:',
            error
        );

        return null;
    }
}


// =========================================================
// DADOS DO USUÁRIO
// =========================================================

function carregarDadosUsuario() {

    const usuario =
        obterUsuario();

    if (!usuario) {

        window.location.href =
            'login.html';

        return;
    }


    const nome =
        usuario.nome ||
        usuario.name ||
        'Cliente';


    const email =
        usuario.email ||
        '';


    const greeting =
        document.getElementById(
            'account-greeting'
        );


    if (greeting) {

        const primeiroNome =
            String(nome)
                .trim()
                .split(' ')[0];


        greeting.textContent =
            'Olá, ' +
            primeiroNome +
            '!';
    }


    const nameInput =
        document.getElementById(
            'account-name'
        );


    if (
        nameInput instanceof
        HTMLInputElement
    ) {

        nameInput.value =
            String(nome);
    }


    const emailInput =
        document.getElementById(
            'account-email'
        );


    if (
        emailInput instanceof
        HTMLInputElement
    ) {

        emailInput.value =
            String(email);
    }
}


// =========================================================
// ENDEREÇO
// =========================================================

function carregarEndereco() {

    const container =
        document.getElementById(
            'address-container'
        );


    if (!container) {

        return;
    }


    const usuario =
        obterUsuario();


    if (!usuario) {

        container.innerHTML = `

            <div class="loading-state">

                <i class="bi bi-exclamation-circle"></i>

                Faça login para visualizar seu endereço.

            </div>

        `;

        return;
    }


    const endereco =
        usuario.endereco ||
        usuario.Endereco ||
        '';


    const numero =
        usuario.numero ||
        usuario.Numero ||
        '';


    const cidade =
        usuario.cidade ||
        usuario.Cidade ||
        '';


    const estado =
        usuario.estado ||
        usuario.Estado ||
        '';


    const cep =
        usuario.cep ||
        usuario.Cep ||
        '';


    if (
        !endereco &&
        !numero &&
        !cidade &&
        !estado &&
        !cep
    ) {

        container.innerHTML = `

            <div class="loading-state">

                <i class="bi bi-geo-alt"></i>

                Nenhum endereço cadastrado.

            </div>

        `;

        return;
    }


    let enderecoHtml = '';


    if (endereco) {

        enderecoHtml +=
            escapeHtml(endereco);
    }


    if (numero) {

        enderecoHtml +=
            ', ' +
            escapeHtml(numero);
    }


    let cidadeHtml =
        escapeHtml(cidade);


    if (estado) {

        cidadeHtml +=
            ' - ' +
            escapeHtml(estado);
    }


    let cepHtml = '';


    if (cep) {

        cepHtml = `

            <p style="margin:6px 0;">

                CEP:
                ${escapeHtml(cep)}

            </p>

        `;
    }


    container.innerHTML = `

        <div class="account-address-card">

            <div style="
                display:flex;
                align-items:center;
                gap:12px;
                margin-bottom:14px;
            ">

                <i class="bi bi-house-door"
                   style="font-size:24px;">
                </i>

                <strong>
                    Endereço principal
                </strong>

            </div>


            <p style="margin:6px 0;">

                ${enderecoHtml}

            </p>


            <p style="margin:6px 0;">

                ${cidadeHtml}

            </p>


            ${cepHtml}

        </div>

    `;
}


// =========================================================
// PEDIDOS
// =========================================================

async function carregarPedidos() {

    const container =
        document.getElementById(
            'orders-list'
        );


    if (!container) {

        return;
    }


    const usuario =
        obterUsuario();


    if (
        !usuario ||
        !usuario.id
    ) {

        container.innerHTML = `

            <div class="loading-state">

                <i class="bi bi-exclamation-circle"></i>

                Faça login para visualizar seus pedidos.

            </div>

        `;

        return;
    }


    const clienteId =
        Number(usuario.id);


    if (!clienteId) {

        container.innerHTML = `

            <div class="loading-state">

                <i class="bi bi-exclamation-circle"></i>

                Não foi possível identificar o cliente.

            </div>

        `;

        return;
    }


    try {

        const controller =
            new AbortController();


        const timeout =
            setTimeout(
                function () {

                    controller.abort();

                },
                10000
            );


        const response =
            await fetch(

                ACCOUNT_API_URL +
                '/Pedido/cliente/' +
                clienteId,

                {
                    method: 'GET',

                    headers: {
                        'Accept':
                            'application/json'
                    },

                    signal:
                        controller.signal
                }
            );


        clearTimeout(timeout);


        if (!response.ok) {

            throw new Error(
                'HTTP ' +
                response.status
            );
        }


        const pedidos =
            await response.json();


        if (
            !Array.isArray(pedidos) ||
            pedidos.length === 0
        ) {

            container.innerHTML = `

                <div class="loading-state">

                    <i class="bi bi-box-seam"></i>

                    Você ainda não possui pedidos.

                </div>

            `;

            return;
        }


        let htmlPedidos = '';


        for (
            let i = 0;
            i < pedidos.length;
            i++
        ) {

            htmlPedidos +=
                renderPedido(
                    pedidos[i]
                );
        }


        container.innerHTML =
            htmlPedidos;

    } catch (error) {

        console.error(
            'Erro ao carregar pedidos:',
            error
        );


        if (
            error &&
            error.name === 'AbortError'
        ) {

            container.innerHTML = `

                <div class="loading-state">

                    <i class="bi bi-clock-history"></i>

                    A API demorou muito para responder.

                    <br>

                    <small>
                        Tente novamente em alguns instantes.
                    </small>

                </div>

            `;

            return;
        }


        container.innerHTML = `

            <div class="loading-state">

                <i class="bi bi-exclamation-triangle"></i>

                Não foi possível carregar seus pedidos.

                <br>

                <small>
                    Verifique se a API está funcionando.
                </small>

            </div>

        `;
    }
}


// =========================================================
// RENDERIZA PEDIDO
// =========================================================

function renderPedido(pedido) {

    const id =
        pedido.id ??
        pedido.Id;


    const data =
        pedido.dataPedido ??
        pedido.DataPedido;


    const valor =
        pedido.valorTotal ??
        pedido.ValorTotal ??
        0;


    const statusId =
        pedido.statusPedidoId ??
        pedido.StatusPedidoId;


    const status =
        obterNomeStatus(
            statusId
        );


    let dataFormatada =
        'Data não informada';


    if (data) {

        const dataObj =
            new Date(data);


        if (
            !Number.isNaN(
                dataObj.getTime()
            )
        ) {

            dataFormatada =
                dataObj.toLocaleDateString(
                    'pt-BR'
                );
        }
    }


    const valorFormatado =
        Number(valor)
            .toLocaleString(
                'pt-BR',
                {
                    style: 'currency',
                    currency: 'BRL'
                }
            );


    return `

        <div class="account-order-card"
             style="
                border:1px solid rgba(255,255,255,.10);
                border-radius:10px;
                padding:18px;
                margin-bottom:12px;
             ">


            <div style="
                display:flex;
                justify-content:space-between;
                align-items:center;
                gap:15px;
                flex-wrap:wrap;
            ">


                <div>

                    <strong>

                        Pedido #${escapeHtml(id)}

                    </strong>


                    <div style="
                        margin-top:5px;
                        opacity:.75;
                        font-size:14px;
                    ">

                        ${escapeHtml(dataFormatada)}

                    </div>

                </div>


                <span style="
                    font-weight:600;
                ">

                    ${escapeHtml(status)}

                </span>


            </div>


            <div style="
                margin-top:15px;
                font-size:16px;
                font-weight:600;
            ">

                ${escapeHtml(valorFormatado)}

            </div>


        </div>

    `;
}


// =========================================================
// STATUS DO PEDIDO
// =========================================================

function obterNomeStatus(statusId) {

    const id =
        Number(statusId);


    switch (id) {

        case 1:

            return 'Pendente';


        case 2:

            return 'Processando';


        case 3:

            return 'Enviado';


        case 4:

            return 'Entregue';


        case 5:

            return 'Cancelado';


        default:

            return 'Status desconhecido';
    }
}


// =========================================================
// MENU DA CONTA
// =========================================================

function configurarMenuConta() {

    const menuLinks =
        document.querySelectorAll(
            '.account-menu a[data-panel]'
        );


    const panels =
        document.querySelectorAll(
            '.account-panel'
        );


    for (
        let i = 0;
        i < menuLinks.length;
        i++
    ) {

        const link =
            menuLinks[i];


        link.addEventListener(
            'click',
            function (event) {

                event.preventDefault();


                const panelId =
                    link.getAttribute(
                        'data-panel'
                    );


                if (!panelId) {

                    return;
                }


                for (
                    let j = 0;
                    j < panels.length;
                    j++
                ) {

                    panels[j].classList.remove(
                        'active'
                    );
                }


                for (
                    let j = 0;
                    j < menuLinks.length;
                    j++
                ) {

                    menuLinks[j].classList.remove(
                        'active'
                    );
                }


                const selectedPanel =
                    document.getElementById(
                        panelId
                    );


                if (selectedPanel) {

                    selectedPanel.classList.add(
                        'active'
                    );
                }


                link.classList.add(
                    'active'
                );

            }
        );
    }
}


// =========================================================
// LOGOUT
// =========================================================

function configurarLogout() {

    const logoutBtn =
        document.getElementById(
            'logout-btn'
        );


    if (!logoutBtn) {

        return;
    }


    logoutBtn.addEventListener(
        'click',
        function (event) {

            event.preventDefault();


            localStorage.removeItem(
                'ip_user'
            );


            window.location.href =
                'login.html';

        }
    );
}


// =========================================================
// SALVAR DADOS
// =========================================================

function configurarSalvarDados() {

    const button =
        document.getElementById(
            'save-account-data'
        );


    if (!button) {

        return;
    }


    button.addEventListener(
        'click',
        function () {

            const usuario =
                obterUsuario();


            if (!usuario) {

                window.location.href =
                    'login.html';

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


            const novoNome =
                nameInput
                    ? nameInput.value.trim()
                    : '';


            const novoEmail =
                emailInput
                    ? emailInput.value.trim()
                    : '';


            if (!novoNome) {

                alert(
                    'Digite seu nome completo.'
                );

                return;
            }


            if (!novoEmail) {

                alert(
                    'Digite seu e-mail.'
                );

                return;
            }


            usuario.nome =
                novoNome;


            usuario.name =
                novoNome;


            usuario.email =
                novoEmail;


            localStorage.setItem(
                'ip_user',
                JSON.stringify(usuario)
            );


            carregarDadosUsuario();


            alert(
                'Dados atualizados com sucesso!'
            );

        }
    );
}


// =========================================================
// ATUALIZAR SENHA
// =========================================================

function configurarAtualizarSenha() {

    const button =
        document.getElementById(
            'update-password'
        );


    if (!button) {

        return;
    }


    button.addEventListener(
        'click',
        function () {

            const currentPassword =
                document.getElementById(
                    'current-password'
                );


            const newPassword =
                document.getElementById(
                    'new-password'
                );


            const atual =
                currentPassword
                    ? currentPassword.value
                    : '';


            const nova =
                newPassword
                    ? newPassword.value
                    : '';


            if (!atual) {

                alert(
                    'Digite sua senha atual.'
                );

                return;
            }


            if (!nova) {

                alert(
                    'Digite a nova senha.'
                );

                return;
            }


            if (nova.length < 6) {

                alert(
                    'A nova senha deve ter pelo menos 6 caracteres.'
                );

                return;
            }


            alert(
                'A alteração de senha ainda será integrada à API.'
            );

        }
    );
}


// =========================================================
// SEGURANÇA HTML
// =========================================================

function escapeHtml(value) {

    return String(value ?? '')
        .replace(
            /&/g,
            '&amp;'
        )
        .replace(
            /</g,
            '&lt;'
        )
        .replace(
            />/g,
            '&gt;'
        )
        .replace(
            /"/g,
            '&quot;'
        )
        .replace(
            /'/g,
            '&#039;'
        );
}