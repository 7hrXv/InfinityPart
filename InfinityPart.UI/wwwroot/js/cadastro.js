const API_URL = "http://localhost:5022/api";

document.addEventListener("DOMContentLoaded", function() {

    // =========================================================
    // ELEMENTOS
    // =========================================================

    const formulario = document.getElementById("register-form");

    const etapa1 = document.querySelector(
        '[data-register-step="1"]'
    );

    const etapa2 = document.querySelector(
        '[data-register-step="2"]'
    );

    const indicador1 = document.querySelector(
        '.step-indicator[data-step="1"]'
    );

    const indicador2 = document.querySelector(
        '.step-indicator[data-step="2"]'
    );

    const btnProximo = document.getElementById(
        "btn-register-next"
    );

    const btnVoltar = document.getElementById(
        "btn-register-back"
    );

    const btnFinalizar = document.getElementById(
        "btn-register-finish"
    );

    const cpf = document.getElementById("reg-cpf");
    const telefone = document.getElementById("reg-telefone");
    const nascimento = document.getElementById("reg-nascimento");
    const cep = document.getElementById("reg-cep");
    const estado = document.getElementById("reg-estado");
    const cepStatus = document.getElementById("cep-status");


    // =========================================================
    // NOTIFICAÇÕES
    // =========================================================

    function criarNotificacao() {

        let container = document.getElementById(
            "infinity-notifications"
        );

        if (container) {
            return container;
        }

        container = document.createElement("div");
        container.id = "infinity-notifications";

        document.body.appendChild(container);

        const style = document.createElement("style");
        style.id = "infinity-notifications-style";

        style.textContent = `
            #infinity-notifications {
                position: fixed;
                right: 24px;
                bottom: 24px;
                z-index: 999999;
                display: flex;
                flex-direction: column;
                gap: 12px;
                width: min(380px, calc(100vw - 32px));
                pointer-events: none;
            }

            .infinity-notification {
                position: relative;
                display: flex;
                align-items: flex-start;
                gap: 13px;
                padding: 16px 18px;
                background: #0D0D0D;
                border: 1px solid #262626;
                border-radius: 10px;
                box-shadow: 0 12px 35px rgba(0, 0, 0, 0.45);
                color: #FFFFFF;
                font-family: Inter, Arial, sans-serif;
                overflow: hidden;
                animation: infinityNotificationIn 0.25s ease forwards;
                pointer-events: auto;
            }

            .infinity-notification::before {
                content: "";
                position: absolute;
                left: 0;
                top: 0;
                bottom: 0;
                width: 4px;
            }

            .infinity-notification.error::before {
                background: #E10600;
            }

            .infinity-notification.success::before {
                background: #22C55E;
            }

            .infinity-notification-icon {
                width: 28px;
                height: 28px;
                min-width: 28px;
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                font-size: 15px;
                font-weight: 700;
                margin-top: 1px;
            }

            .infinity-notification.error
            .infinity-notification-icon {
                background: rgba(225, 6, 0, 0.14);
                color: #E10600;
            }

            .infinity-notification.success
            .infinity-notification-icon {
                background: rgba(34, 197, 94, 0.14);
                color: #22C55E;
            }

            .infinity-notification-content {
                flex: 1;
                min-width: 0;
            }

            .infinity-notification-title {
                font-size: 14px;
                font-weight: 700;
                margin-bottom: 4px;
            }

            .infinity-notification-message {
                color: #A0A0A0;
                font-size: 13px;
                line-height: 1.45;
            }

            .infinity-notification-close {
                border: 0;
                background: transparent;
                color: #6E6E6E;
                cursor: pointer;
                font-size: 18px;
                line-height: 1;
                padding: 0;
            }

            .infinity-notification-close:hover {
                color: #FFFFFF;
            }

            .infinity-notification.hide {
                animation: infinityNotificationOut 0.25s ease forwards;
            }

            @keyframes infinityNotificationIn {
                from {
                    opacity: 0;
                    transform: translateX(30px);
                }

                to {
                    opacity: 1;
                    transform: translateX(0);
                }
            }

            @keyframes infinityNotificationOut {
                from {
                    opacity: 1;
                    transform: translateX(0);
                }

                to {
                    opacity: 0;
                    transform: translateX(30px);
                }
            }

            @media (max-width: 600px) {
                #infinity-notifications {
                    right: 16px;
                    bottom: 16px;
                    width: calc(100vw - 32px);
                }
            }
        `;

        document.head.appendChild(style);

        return container;
    }


    function mostrarNotificacao(
        tipo,
        mensagem,
        titulo = null
    ) {

        const container = criarNotificacao();

        const notificacao = document.createElement("div");

        notificacao.className =
            `infinity-notification ${tipo}`;

        const tituloFinal =
            titulo ||
            (
                tipo === "success"
                    ? "Tudo certo!"
                    : "Não foi possível continuar"
            );

        const icone =
            tipo === "success"
                ? "✓"
                : "!";

        notificacao.innerHTML = `
            <div class="infinity-notification-icon">
                ${icone}
            </div>

            <div class="infinity-notification-content">

                <div class="infinity-notification-title">
                    ${tituloFinal}
                </div>

                <div class="infinity-notification-message">
                    ${mensagem}
                </div>

            </div>

            <button
                type="button"
                class="infinity-notification-close"
                aria-label="Fechar"
            >
                ×
            </button>
        `;

        container.appendChild(notificacao);

        const fechar = () => {

            if (
                !notificacao ||
                notificacao.classList.contains("hide")
            ) {
                return;
            }

            notificacao.classList.add("hide");

            setTimeout(() => {
                notificacao.remove();
            }, 250);
        };

        notificacao
            .querySelector(".infinity-notification-close")
            .addEventListener("click", fechar);

        setTimeout(
            fechar,
            tipo === "success"
                ? 4500
                : 5500
        );
    }


    function erro(mensagem) {

        mostrarNotificacao(
            "error",
            mensagem,
            "Confira os dados"
        );
    }


    function sucesso(mensagem) {

        mostrarNotificacao(
            "success",
            mensagem,
            "Cadastro realizado"
        );
    }


    // =========================================================
    // SOMENTE NÚMEROS
    // =========================================================

    function somenteNumeros(valor) {

        return String(valor || "")
            .replace(/\D/g, "");
    }


    // =========================================================
    // MÁSCARA CPF
    // =========================================================

    function aplicarMascaraCpf(valor) {

        let numeros = somenteNumeros(valor);

        numeros = numeros.substring(0, 11);

        if (numeros.length <= 3) {
            return numeros;
        }

        if (numeros.length <= 6) {

            return numeros.replace(
                /(\d{3})(\d+)/,
                "$1.$2"
            );
        }

        if (numeros.length <= 9) {

            return numeros.replace(
                /(\d{3})(\d{3})(\d+)/,
                "$1.$2.$3"
            );
        }

        return numeros.replace(
            /(\d{3})(\d{3})(\d{3})(\d{1,2})/,
            "$1.$2.$3-$4"
        );
    }


    // =========================================================
    // MÁSCARA TELEFONE
    // =========================================================

    function aplicarMascaraTelefone(valor) {

        let numeros = somenteNumeros(valor);

        numeros = numeros.substring(0, 11);

        if (numeros.length <= 2) {
            return numeros;
        }

        if (numeros.length <= 6) {

            return numeros.replace(
                /(\d{2})(\d+)/,
                "($1) $2"
            );
        }

        if (numeros.length <= 10) {

            return numeros.replace(
                /(\d{2})(\d{4})(\d+)/,
                "($1) $2-$3"
            );
        }

        return numeros.replace(
            /(\d{2})(\d{5})(\d{1,4})/,
            "($1) $2-$3"
        );
    }


    // =========================================================
    // MÁSCARA DATA
    // =========================================================

    function aplicarMascaraData(valor) {

        let numeros = somenteNumeros(valor);

        numeros = numeros.substring(0, 8);

        if (numeros.length <= 2) {
            return numeros;
        }

        if (numeros.length <= 4) {

            return numeros.replace(
                /(\d{2})(\d+)/,
                "$1/$2"
            );
        }

        return numeros.replace(
            /(\d{2})(\d{2})(\d{1,4})/,
            "$1/$2/$3"
        );
    }


    // =========================================================
    // MÁSCARA CEP
    // =========================================================

    function aplicarMascaraCep(valor) {

        let numeros = somenteNumeros(valor);

        numeros = numeros.substring(0, 8);

        if (numeros.length <= 5) {
            return numeros;
        }

        return numeros.replace(
            /(\d{5})(\d{1,3})/,
            "$1-$2"
        );
    }


    // =========================================================
    // EVENTOS DAS MÁSCARAS
    // =========================================================

    if (cpf) {

        cpf.addEventListener("input", function() {

            this.value = aplicarMascaraCpf(
                this.value
            );
        });
    }


    if (telefone) {

        telefone.addEventListener("input", function() {

            this.value = aplicarMascaraTelefone(
                this.value
            );
        });
    }


    if (nascimento) {

        nascimento.addEventListener("input", function() {

            this.value = aplicarMascaraData(
                this.value
            );
        });
    }


    if (cep) {

        cep.addEventListener("input", function() {

            this.value = aplicarMascaraCep(
                this.value
            );

            const numeros =
                somenteNumeros(this.value);

            if (numeros.length === 8) {
                buscarCep(numeros);
            }
        });
    }


    if (estado) {

        estado.addEventListener("input", function() {

            this.value = this.value
                .replace(/[^a-zA-Z]/g, "")
                .substring(0, 2)
                .toUpperCase();
        });
    }


    // =========================================================
    // VIA CEP
    // =========================================================

    async function buscarCep(cepNumerico) {

        if (!cepStatus) {
            return;
        }

        cepStatus.textContent =
            "Consultando CEP...";

        try {

            const resposta = await fetch(
                `https://viacep.com.br/ws/${cepNumerico}/json/`
            );

            if (!resposta.ok) {

                throw new Error(
                    "Não foi possível consultar o CEP."
                );
            }

            const dados = await resposta.json();

            if (dados.erro) {

                cepStatus.textContent =
                    "CEP não encontrado.";

                return;
            }

            const rua =
                document.getElementById("reg-rua");

            const bairro =
                document.getElementById("reg-bairro");

            const cidade =
                document.getElementById("reg-cidade");

            const estadoCampo =
                document.getElementById("reg-estado");


            if (rua) {
                rua.value =
                    dados.logradouro || "";
            }

            if (bairro) {
                bairro.value =
                    dados.bairro || "";
            }

            if (cidade) {
                cidade.value =
                    dados.localidade || "";
            }

            if (estadoCampo) {
                estadoCampo.value =
                    dados.uf || "";
            }

            cepStatus.textContent =
                "CEP encontrado.";

        } catch (erro) {

            console.error(
                "[Infinity Parts] Erro ao consultar CEP:",
                erro
            );

            cepStatus.textContent =
                "Não foi possível consultar o CEP.";
        }
    }


    // =========================================================
    // CPF VÁLIDO
    // =========================================================

    function cpfValido(valor) {

        const numeros =
            somenteNumeros(valor);

        if (numeros.length !== 11) {
            return false;
        }

        if (
            numeros
                .split("")
                .every(
                    numero =>
                        numero === numeros[0]
                )
        ) {
            return false;
        }

        let soma = 0;

        for (let i = 0; i < 9; i++) {

            soma +=
                Number(numeros[i]) *
                (10 - i);
        }

        let resto =
            soma % 11;

        let primeiroDigito =
            resto < 2
                ? 0
                : 11 - resto;

        if (
            primeiroDigito !==
            Number(numeros[9])
        ) {
            return false;
        }

        soma = 0;

        for (let i = 0; i < 10; i++) {

            soma +=
                Number(numeros[i]) *
                (11 - i);
        }

        resto =
            soma % 11;

        const segundoDigito =
            resto < 2
                ? 0
                : 11 - resto;

        return (
            segundoDigito ===
            Number(numeros[10])
        );
    }


    // =========================================================
    // DATA PARA API
    // =========================================================

    function converterDataParaApi(valor) {

        const numeros =
            somenteNumeros(valor);

        if (numeros.length !== 8) {
            return null;
        }

        const dia =
            numeros.substring(0, 2);

        const mes =
            numeros.substring(2, 4);

        const ano =
            numeros.substring(4, 8);

        const data = new Date(
            Number(ano),
            Number(mes) - 1,
            Number(dia)
        );

        if (
            data.getFullYear() !== Number(ano) ||
            data.getMonth() !== Number(mes) - 1 ||
            data.getDate() !== Number(dia)
        ) {
            return null;
        }

        return `${ano}-${mes}-${dia}`;
    }


    // =========================================================
    // VALIDAR ETAPA 1
    // =========================================================

    function validarEtapa1() {

        const nome =
            document.getElementById("reg-nome");

        const email =
            document.getElementById("reg-email");

        const senha =
            document.getElementById("reg-senha");

        const confirmar =
            document.getElementById("reg-confirmar");


        if (
            !nome ||
            !nome.value.trim()
        ) {

            erro(
                "Informe seu nome completo."
            );

            nome?.focus();

            return false;
        }


        if (
            !cpf ||
            !cpfValido(cpf.value)
        ) {

            erro(
                "Informe um CPF válido."
            );

            cpf?.focus();

            return false;
        }


        const telefoneNumerico =
            somenteNumeros(
                telefone?.value
            );

        if (
            telefoneNumerico.length !== 10 &&
            telefoneNumerico.length !== 11
        ) {

            erro(
                "Informe um telefone válido."
            );

            telefone?.focus();

            return false;
        }


        const dataApi =
            converterDataParaApi(
                nascimento?.value
            );

        if (!dataApi) {

            erro(
                "Informe uma data de nascimento válida."
            );

            nascimento?.focus();

            return false;
        }


        if (
            !email ||
            !email.value.trim()
        ) {

            erro(
                "Informe seu e-mail."
            );

            email?.focus();

            return false;
        }


        if (
            !senha ||
            senha.value.length < 8
        ) {

            erro(
                "A senha deve ter no mínimo 8 caracteres."
            );

            senha?.focus();

            return false;
        }


        if (!/[a-zA-Z]/.test(senha.value)) {

            erro(
                "A senha deve conter pelo menos uma letra."
            );

            senha.focus();

            return false;
        }


        if (!/\d/.test(senha.value)) {

            erro(
                "A senha deve conter pelo menos um número."
            );

            senha.focus();

            return false;
        }


        if (
            !confirmar ||
            confirmar.value !== senha.value
        ) {

            erro(
                "As senhas não coincidem."
            );

            confirmar?.focus();

            return false;
        }


        return true;
    }


    // =========================================================
    // PRÓXIMO
    // =========================================================

    function irParaEtapa2() {

        if (!validarEtapa1()) {
            return;
        }

        etapa1?.classList.remove("active");
        etapa2?.classList.add("active");

        indicador1?.classList.remove("active");
        indicador1?.classList.add("completed");

        indicador2?.classList.add("active");

        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
    }


    // =========================================================
    // VOLTAR
    // =========================================================

    function voltarEtapa1() {

        etapa2?.classList.remove("active");
        etapa1?.classList.add("active");

        indicador2?.classList.remove("active");

        indicador1?.classList.add("active");
        indicador1?.classList.remove("completed");

        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
    }


    btnProximo?.addEventListener(
        "click",
        irParaEtapa2
    );

    btnVoltar?.addEventListener(
        "click",
        voltarEtapa1
    );


    // =========================================================
    // FINALIZAR CADASTRO
    // =========================================================

    formulario?.addEventListener(
        "submit",
        async function(event) {

            event.preventDefault();


            const nome =
                document.getElementById("reg-nome");

            const email =
                document.getElementById("reg-email");

            const senha =
                document.getElementById("reg-senha");

            const rua =
                document.getElementById("reg-rua");

            const numero =
                document.getElementById("reg-numero");

            const complemento =
                document.getElementById("reg-complemento");

            const bairro =
                document.getElementById("reg-bairro");

            const cidade =
                document.getElementById("reg-cidade");

            const estadoCampo =
                document.getElementById("reg-estado");


            const cepNumerico =
                somenteNumeros(
                    cep?.value
                );


            if (cepNumerico.length !== 8) {

                erro(
                    "Informe um CEP válido."
                );

                cep?.focus();

                return;
            }


            if (!rua?.value.trim()) {

                erro(
                    "Informe o endereço."
                );

                rua?.focus();

                return;
            }


            if (!numero?.value.trim()) {

                erro(
                    "Informe o número."
                );

                numero?.focus();

                return;
            }


            if (!bairro?.value.trim()) {

                erro(
                    "Informe o bairro."
                );

                bairro?.focus();

                return;
            }


            if (!cidade?.value.trim()) {

                erro(
                    "Informe a cidade."
                );

                cidade?.focus();

                return;
            }


            if (!estadoCampo?.value.trim()) {

                erro(
                    "Informe o estado."
                );

                estadoCampo?.focus();

                return;
            }


            if (btnFinalizar) {

                btnFinalizar.disabled = true;

                btnFinalizar.textContent =
                    "CADASTRANDO...";
            }


            const dataNascimento =
                converterDataParaApi(
                    nascimento.value
                );


            const dados = {

                nome:
                    nome.value.trim(),

                cpf:
                    somenteNumeros(
                        cpf.value
                    ),

                email:
                    email.value.trim(),

                telefone:
                    somenteNumeros(
                        telefone.value
                    ),

                senha:
                    senha.value,

                dataNascimento:
                    dataNascimento,

                cep:
                    cepNumerico,

                endereco:
                    rua.value.trim(),

                numero:
                    numero.value.trim(),

                complemento:
                    complemento?.value.trim() || "",

                bairro:
                    bairro.value.trim(),

                cidade:
                    cidade.value.trim(),

                estado:
                    estadoCampo.value
                        .trim()
                        .toUpperCase()
            };


            try {

                const resposta =
                    await fetch(
                        `${API_URL}/Cliente`,
                        {
                            method: "POST",

                            headers: {
                                "Content-Type":
                                    "application/json"
                            },

                            body:
                                JSON.stringify(dados)
                        }
                    );


                const texto =
                    await resposta.text();

                let resultado = null;


                try {

                    resultado =
                        texto
                            ? JSON.parse(texto)
                            : null;

                } catch {

                    resultado = null;
                }


                if (!resposta.ok) {

                    let mensagem =
                        "Não foi possível criar a conta.";

                    if (resultado?.message) {
                        mensagem =
                            resultado.message;
                    }

                    if (resultado?.title) {
                        mensagem =
                            resultado.title;
                    }

                    throw new Error(
                        mensagem
                    );
                }


                sucesso(
                    "Sua conta foi criada com sucesso!"
                );


                setTimeout(
                    function() {

                        window.location.href =
                            "login.html";

                    },
                    1800
                );


            } catch (e) {

                console.error(
                    "[Infinity Parts] Erro no cadastro:",
                    e
                );


                erro(
                    e.message ||
                    "Não foi possível criar sua conta."
                );


                if (btnFinalizar) {

                    btnFinalizar.disabled =
                        false;

                    btnFinalizar.textContent =
                        "FINALIZAR CADASTRO";
                }
            }
        }
    );


    // =========================================================
    // ANO
    // =========================================================

    const ano =
        document.querySelector(
            ".current-year"
        );

    if (ano) {

        ano.textContent =
            new Date().getFullYear();
    }


    console.log(
        "[Infinity Parts] cadastro.js carregado corretamente."
    );

});