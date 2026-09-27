document.addEventListener('DOMContentLoaded', function () {

    const accountLink = document.getElementById('header-account');
    const accountText = document.getElementById('header-account-text');
    const accountIcon = document.querySelector('#header-account i');
    const topAccountLink = document.getElementById('top-account-link');

    if (!accountLink || !accountText) {
        return;
    }

    function atualizarHeaderConta() {

        let usuario = null;

        try {

            const dados = localStorage.getItem('ip_user');

            if (dados) {
                usuario = JSON.parse(dados);
            }

        } catch (e) {

            usuario = null;

        }


        /*
         * USUÁRIO LOGADO
         */

        if (usuario && usuario.id) {

            accountLink.setAttribute(
                'href',
                'minha-conta.html'
            );

            accountText.textContent =
                'Minha Conta';


            if (accountIcon) {

                accountIcon.className =
                    'bi bi-person-check';

            }


            let logoutLink =
                document.getElementById('header-logout');


            if (!logoutLink) {

                logoutLink =
                    document.createElement('a');

                logoutLink.setAttribute(
                    'href',
                    '#'
                );

                logoutLink.id =
                    'header-logout';

                logoutLink.className =
                    'header-action';

                logoutLink.innerHTML = `
                    <i class="bi bi-box-arrow-right"></i>
                    <span>Sair</span>
                `;


                logoutLink.addEventListener(
                    'click',
                    function (e) {

                        e.preventDefault();

                        localStorage.removeItem(
                            'ip_user'
                        );

                        atualizarHeaderConta();

                        window.location.href =
                            'index.html';

                    }
                );


                if (accountLink.parentElement) {

                    accountLink.parentElement.insertBefore(
                        logoutLink,
                        accountLink.nextSibling
                    );

                }

            }


            if (topAccountLink) {

                topAccountLink.setAttribute(
                    'href',
                    'minha-conta.html'
                );

                topAccountLink.textContent =
                    'Minha Conta';

            }

        }


        /*
         * USUÁRIO DESLOGADO
         */

        else {

            accountLink.setAttribute(
                'href',
                'login.html'
            );

            accountText.textContent =
                'Conta';


            if (accountIcon) {

                accountIcon.className =
                    'bi bi-person';

            }


            const logoutLink =
                document.getElementById(
                    'header-logout'
                );


            if (logoutLink) {

                logoutLink.remove();

            }


            if (topAccountLink) {

                topAccountLink.setAttribute(
                    'href',
                    'login.html'
                );

                topAccountLink.textContent =
                    'Minha Conta';

            }

        }

    }


    atualizarHeaderConta();


    window.addEventListener(
        'storage',
        function (event) {

            if (event.key === 'ip_user') {

                atualizarHeaderConta();

            }

        }
    );

});