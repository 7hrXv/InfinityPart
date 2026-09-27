/* ============================================================
   INFINITY PARTS — main.js
   Funções globais:
   favoritos, toasts, header, busca, menu mobile,
   WhatsApp, sessão do cliente, login, tema e Home.
   ============================================================ */


/* ============================================================
   CONFIGURAÇÃO
   ============================================================ */

const LOGIN_API_URL = 'http://localhost:5022/api';


/* ============================================================
   CAMINHO BASE DO SITE
   ============================================================ */

function getSiteBasePath() {

    try {

        const scripts =
            document.querySelectorAll('script[src]');

        for (const script of scripts) {

            const src =
                script.getAttribute('src');

            if (
                src &&
                (
                    src.includes('/js/main.js') ||
                    src.endsWith('js/main.js')
                )
            ) {

                const url =
                    new URL(
                        src,
                        window.location.href
                    );

                const jsPath =
                    url.pathname;

                const jsFolder =
                    jsPath.substring(
                        0,
                        jsPath.lastIndexOf('/js/')
                    );

                return jsFolder + '/';
            }
        }

    } catch (e) {

        console.warn(
            'Não foi possível detectar o caminho base.',
            e
        );
    }

    const path =
        window.location.pathname;

    return path.substring(
        0,
        path.lastIndexOf('/') + 1
    );
}


function getSiteUrl(page) {

    const basePath =
        getSiteBasePath();

    return `${window.location.origin}${basePath}${page}`;
}


/* ============================================================
   SESSÃO DO CLIENTE
   ============================================================ */

function getLoggedUser() {

    try {

        return JSON.parse(
            localStorage.getItem(
                'ip_user'
            ) || 'null'
        );

    } catch (e) {

        console.error(
            'Erro ao ler usuário logado:',
            e
        );

        return null;
    }
}


function isUserLoggedIn() {

    return !!getLoggedUser();
}


function logoutUserFromHeader(event) {

    if (event) {
        event.preventDefault();
    }

    localStorage.removeItem(
        'ip_user'
    );

    showToast(
        'Você saiu da sua conta.'
    );

    setTimeout(() => {

        window.location.href =
            getSiteUrl(
                'login.html'
            );

    }, 500);
}


/* ============================================================
   LOGIN DO CLIENTE
   ============================================================ */

function initLoginForm() {

    const loginForm =
        document.getElementById(
            'login-form'
        );

    if (!loginForm) {
        return;
    }

    loginForm.addEventListener(
        'submit',
        async (event) => {

            event.preventDefault();

            const emailInput =
                document.getElementById(
                    'login-email'
                );

            const passwordInput =
                document.getElementById(
                    'login-password'
                );

            if (
                !emailInput ||
                !passwordInput
            ) {
                return;
            }

            const identificador =
                emailInput.value.trim();

            const senha =
                passwordInput.value;

            if (
                !identificador ||
                !senha
            ) {

                showToast(
                    'Informe o e-mail e a senha.',
                    'error'
                );

                return;
            }

            const button =
                loginForm.querySelector(
                    'button[type="submit"]'
                );

            if (button) {

                button.disabled = true;
                button.textContent =
                    'ENTRANDO...';
            }

            try {

                const response =
                    await fetch(
                        `${LOGIN_API_URL}/Autenticacao/login-cliente`,
                        {
                            method: 'POST',

                            headers: {
                                'Content-Type':
                                    'application/json'
                            },

                            body:
                                JSON.stringify({
                                    identificador:
                                        identificador,
                                    senha:
                                        senha
                                })
                        }
                    );

                let data = null;

                try {

                    data =
                        await response.json();

                } catch (e) {

                    data = null;
                }

                if (!response.ok) {

                    showToast(
                        data?.mensagem ||
                        data?.Mensagem ||
                        'E-mail ou senha inválidos.',
                        'error'
                    );

                    return;
                }

                if (
                    !data ||
                    data.autenticado !== true
                ) {

                    showToast(
                        data?.mensagem ||
                        data?.Mensagem ||
                        'E-mail ou senha inválidos.',
                        'error'
                    );

                    return;
                }

                const cliente =
                    data.cliente ||
                    data.Cliente;

                if (!cliente) {

                    console.error(
                        'Cliente não retornado:',
                        data
                    );

                    showToast(
                        'Não foi possível carregar os dados da conta.',
                        'error'
                    );

                    return;
                }

                const usuarioLogado = {

                    id:
                        cliente.id ??
                        cliente.Id,

                    nome:
                        cliente.nome ??
                        cliente.Nome ??
                        '',

                    name:
                        cliente.nome ??
                        cliente.Nome ??
                        '',

                    cpf:
                        cliente.cpf ??
                        cliente.Cpf ??
                        '',

                    email:
                        cliente.email ??
                        cliente.Email ??
                        '',

                    telefone:
                        cliente.telefone ??
                        cliente.Telefone ??
                        '',

                    cep:
                        cliente.cep ??
                        cliente.Cep ??
                        '',

                    endereco:
                        cliente.endereco ??
                        cliente.Endereco ??
                        '',

                    numero:
                        cliente.numero ??
                        cliente.Numero ??
                        '',

                    cidade:
                        cliente.cidade ??
                        cliente.Cidade ??
                        '',

                    estado:
                        cliente.estado ??
                        cliente.Estado ??
                        ''
                };

                localStorage.setItem(
                    'ip_user',
                    JSON.stringify(
                        usuarioLogado
                    )
                );

                showToast(
                    'Login realizado com sucesso!'
                );

                setTimeout(() => {

                    window.location.href =
                        getSiteUrl(
                            'index.html'
                        );

                }, 500);

            } catch (error) {

                console.error(
                    'Erro ao realizar login:',
                    error
                );

                showToast(
                    'Não foi possível conectar à API. Verifique se o backend está rodando.',
                    'error'
                );

            } finally {

                if (button) {

                    button.disabled = false;
                    button.textContent =
                        'ENTRAR';

                }
            }
        }
    );
}


/* ============================================================
   HEADER — CONTA DO USUÁRIO
   ============================================================ */

function updateHeaderAccount() {

    const headerActions =
        document.querySelector(
            '.header-actions'
        );

    if (!headerActions) {
        return;
    }

    const user =
        getLoggedUser();

    let accountAction =
        document.getElementById(
            'header-account-action'
        );


    /* ----------------------------------------------------------
       REMOVE CONTA ANTIGA
       ---------------------------------------------------------- */

    const oldAccount =
        headerActions.querySelector(
            ':scope > a[href="login.html"]'
        );

    if (oldAccount) {
        oldAccount.remove();
    }


    /* ----------------------------------------------------------
       CRIA O BLOCO DA CONTA
       ---------------------------------------------------------- */

    if (!accountAction) {

        accountAction =
            document.createElement(
                'div'
            );

        accountAction.id =
            'header-account-action';

        accountAction.className =
            'header-account-wrapper';

        headerActions.insertBefore(
            accountAction,
            headerActions.firstChild
        );
    }


    /* ----------------------------------------------------------
       FORÇA O BLOCO A FICAR HORIZONTAL
       ---------------------------------------------------------- */

    accountAction.style.display =
        'flex';

    accountAction.style.flexDirection =
        'row';

    accountAction.style.alignItems =
        'center';

    accountAction.style.justifyContent =
        'center';

    accountAction.style.gap =
        '0px';

    accountAction.style.height =
        '100%';


    /* ==========================================================
       USUÁRIO LOGADO
       ========================================================== */

    if (user) {

        const name =
            user.name ||
            user.nome ||
            'Minha conta';

        const firstName =
            String(name)
                .trim()
                .split(' ')[0];


        accountAction.innerHTML = `

            <a
                href="${getSiteUrl(
                    'minha-conta.html'
                )}"
                class="header-action header-account-item"
                title="Minha conta"
            >

                <i class="bi bi-person-circle"></i>

                <span>
                    ${firstName}
                </span>

            </a>


            <a
                href="#"
                class="header-action header-logout header-account-item"
                id="header-logout-btn"
                title="Sair da conta"
            >

                <i class="bi bi-box-arrow-right"></i>

                <span>
                    Sair
                </span>

            </a>

        `;


        /* ------------------------------------------------------
           FORÇA OS DOIS ITENS A TEREM O MESMO TAMANHO
           ------------------------------------------------------ */

        const accountItems =
            accountAction.querySelectorAll(
                '.header-account-item'
            );

        accountItems.forEach(item => {

            item.style.display =
                'flex';

            item.style.flexDirection =
                'column';

            item.style.alignItems =
                'center';

            item.style.justifyContent =
                'center';

            item.style.width =
                '58px';

            item.style.minWidth =
                '58px';

            item.style.height =
                '64px';

            item.style.padding =
                '0';

            item.style.margin =
                '0';

            item.style.boxSizing =
                'border-box';

            item.style.background =
                'transparent';

            item.style.border =
                'none';

            item.style.textDecoration =
                'none';

            item.style.color =
                'inherit';

            item.style.cursor =
                'pointer';

        });


        const logoutBtn =
            document.getElementById(
                'header-logout-btn'
            );

        if (logoutBtn) {

            logoutBtn.addEventListener(
                'click',
                logoutUserFromHeader
            );

        }

    }


    /* ==========================================================
       USUÁRIO NÃO LOGADO
       ========================================================== */

    else {

        accountAction.innerHTML = `

            <a
                href="${getSiteUrl(
                    'login.html'
                )}"
                class="header-action"
                title="Entrar na sua conta"
            >

                <i class="bi bi-person"></i>

                <span>
                    Conta
                </span>

            </a>

        `;
    }
}


/* ============================================================
   PROTEÇÃO DA PÁGINA DE LOGIN
   ============================================================ */

function preventLoggedUserFromLoginPage() {

    const currentPage =
        window.location.pathname
            .split('/')
            .pop()
            .toLowerCase();

    if (
        currentPage !==
        'login.html'
    ) {
        return;
    }

    if (isUserLoggedIn()) {

        window.location.replace(
            getSiteUrl(
                'minha-conta.html'
            )
        );
    }
}


/* ============================================================
   FAVORITOS
   ============================================================ */

function getFavorites() {

    try {

        return JSON.parse(
            localStorage.getItem(
                'ip_favorites'
            ) || '[]'
        );

    } catch (e) {

        return [];
    }
}


function isFavorite(id) {

    return getFavorites()
        .includes(
            Number(id)
        );
}


function toggleFavorite(id) {

    id = Number(id);

    let favs =
        getFavorites();

    if (
        favs.includes(id)
    ) {

        favs =
            favs.filter(
                f => f !== id
            );

        showToast(
            'Produto removido dos favoritos.'
        );

    } else {

        favs.push(id);

        showToast(
            'Produto adicionado aos favoritos.'
        );
    }

    localStorage.setItem(
        'ip_favorites',
        JSON.stringify(favs)
    );

    updateFavCount();

    return favs.includes(id);
}


function updateFavCount() {

    const el =
        document.getElementById(
            'fav-count'
        );

    if (el) {

        el.textContent =
            getFavorites().length;
    }
}


/* ============================================================
   TOASTS
   ============================================================ */

function showToast(
    message,
    type = 'success'
) {

    let container =
        document.getElementById(
            'toast-container'
        );

    if (!container) {

        container =
            document.createElement(
                'div'
            );

        container.id =
            'toast-container';

        document.body.appendChild(
            container
        );
    }

    const toast =
        document.createElement(
            'div'
        );

    toast.className =
        `toast-ip toast-${type}`;

    toast.innerHTML = `

        <i class="bi ${
            type === 'success'
                ? 'bi-check-circle-fill'
                : 'bi-exclamation-circle-fill'
        }"></i>

        <span>
            ${message}
        </span>

    `;

    container.appendChild(
        toast
    );

    requestAnimationFrame(() => {

        toast.classList.add(
            'show'
        );

    });

    setTimeout(() => {

        toast.classList.remove(
            'show'
        );

        setTimeout(() => {

            toast.remove();

        }, 300);

    }, 2600);
}


/* ============================================================
   HEADER — SCROLL
   ============================================================ */

function initHeaderScroll() {

    const header =
        document.querySelector(
            '.site-header'
        );

    if (!header) {
        return;
    }

    window.addEventListener(
        'scroll',
        () => {

            header.classList.toggle(
                'scrolled',
                window.scrollY > 10
            );

        }
    );
}


/* ============================================================
   MENU MOBILE
   ============================================================ */

function initMobileMenu() {

    const toggle =
        document.getElementById(
            'menu-toggle'
        );

    const nav =
        document.getElementById(
            'main-nav'
        );

    if (
        !toggle ||
        !nav
    ) {
        return;
    }

    toggle.addEventListener(
        'click',
        () => {

            nav.classList.toggle(
                'open'
            );

            toggle.classList.toggle(
                'active'
            );

            document.body.classList.toggle(
                'no-scroll'
            );

        }
    );

    nav.querySelectorAll('a')
        .forEach(a => {

            a.addEventListener(
                'click',
                () => {

                    nav.classList.remove(
                        'open'
                    );

                    toggle.classList.remove(
                        'active'
                    );

                    document.body.classList.remove(
                        'no-scroll'
                    );

                }
            );

        });
}


/* ============================================================
   SUBMENU MOBILE
   ============================================================ */

function initMobileDropdowns() {

    document
        .querySelectorAll(
            '.has-dropdown > a'
        )
        .forEach(link => {

            link.addEventListener(
                'click',
                (e) => {

                    if (
                        window.innerWidth >
                        768
                    ) {
                        return;
                    }

                    e.preventDefault();

                    link.parentElement
                        .classList.toggle(
                            'open'
                        );

                }
            );

        });
}


/* ============================================================
   BUSCA
   ============================================================ */

function initSearch() {

    const input =
        document.getElementById(
            'search-input'
        );

    const box =
        document.getElementById(
            'search-suggestions'
        );

    const form =
        document.getElementById(
            'search-form'
        );

    if (
        !input ||
        !box
    ) {
        return;
    }

    input.addEventListener(
        'input',
        () => {

            const term =
                input.value.trim();

            if (
                term.length < 2
            ) {

                box.classList.remove(
                    'active'
                );

                box.innerHTML =
                    '';

                return;
            }

            const results =
                searchProducts(term)
                    .slice(0, 6);

            if (
                results.length === 0
            ) {

                box.innerHTML = `
                    <div class="suggestion-empty">
                        Nenhum produto encontrado.
                    </div>
                `;

            } else {

                box.innerHTML =
                    results
                        .map(
                            p => `

                                <a
                                    class="suggestion-item"
                                    href="${getSiteUrl(
                                        `produto.html?id=${p.id}`
                                    )}"
                                >

                                    ${
                                        p.image
                                            ? `
                                                <img
                                                    src="${resolveImageUrl(
                                                        p.image
                                                    )}"
                                                    alt="${p.name}"
                                                >
                                            `
                                            : `
                                                <div class="no-image-placeholder compact">
                                                    <i class="bi bi-image"></i>
                                                </div>
                                            `
                                    }

                                    <div>

                                        <span class="s-name">
                                            ${p.name}
                                        </span>

                                        <span class="s-category">
                                            ${p.category} • ${p.brand}
                                        </span>

                                    </div>

                                    <span class="s-price">
                                        ${formatBRL(
                                            p.salePrice
                                        )}
                                    </span>

                                </a>

                            `
                        )
                        .join('');

            }

            box.classList.add(
                'active'
            );

        }
    );

    document.addEventListener(
        'click',
        (e) => {

            if (
                !box.contains(e.target) &&
                e.target !== input
            ) {

                box.classList.remove(
                    'active'
                );
            }

        }
    );

    if (form) {

        form.addEventListener(
            'submit',
            (e) => {

                e.preventDefault();

                const term =
                    input.value.trim();

                window.location.href =
                    getSiteUrl(
                        `produtos.html?busca=${encodeURIComponent(
                            term
                        )}`
                    );

            }
        );
    }
}


/* ============================================================
   WHATSAPP
   ============================================================ */

function initWhatsapp() {

    const btn =
        document.getElementById(
            'whatsapp-btn'
        );

    if (!btn) {
        return;
    }

    btn.addEventListener(
        'click',
        () => {

            const msg =
                encodeURIComponent(
                    'Olá! Vim pelo site da Infinity Parts e gostaria de ajuda com um produto.'
                );

            window.open(
                `https://wa.me/5511999999999?text=${msg}`,
                '_blank'
            );

        }
    );
}


/* ============================================================
   TEMA
   ============================================================ */

function initThemeToggle() {

    const toggle =
        document.getElementById(
            'theme-toggle'
        );

    if (!toggle) {
        return;
    }

    function isLight() {

        return (
            document.documentElement
                .getAttribute(
                    'data-theme'
                ) === 'light'
        );
    }

    function syncAria() {

        toggle.setAttribute(
            'aria-pressed',
            isLight()
                ? 'true'
                : 'false'
        );

        toggle.title =
            isLight()
                ? 'Mudar para tema escuro'
                : 'Mudar para tema claro';
    }

    syncAria();

    toggle.addEventListener(
        'click',
        () => {

            if (isLight()) {

                document.documentElement
                    .removeAttribute(
                        'data-theme'
                    );

                localStorage.setItem(
                    'ip_theme',
                    'dark'
                );

            } else {

                document.documentElement
                    .setAttribute(
                        'data-theme',
                        'light'
                    );

                localStorage.setItem(
                    'ip_theme',
                    'light'
                );
            }

            syncAria();
        }
    );
}


/* ============================================================
   FAVORITAR
   ============================================================ */

function initFavButtons() {

    document.body.addEventListener(
        'click',
        (e) => {

            const btn =
                e.target.closest(
                    '.btn-fav'
                );

            if (!btn) {
                return;
            }

            const id =
                btn.dataset.id;

            const active =
                toggleFavorite(id);

            btn.classList.toggle(
                'active',
                active
            );

            const icon =
                btn.querySelector('i');

            if (icon) {

                icon.className =
                    `bi ${
                        active
                            ? 'bi-heart-fill'
                            : 'bi-heart'
                    }`;
            }
        }
    );
}


/* ============================================================
   COMPRAR
   ============================================================ */

function initBuyButtons() {

    document.body.addEventListener(
        'click',
        (e) => {

            const btn =
                e.target.closest(
                    '.btn-buy'
                );

            if (
                !btn ||
                btn.disabled
            ) {
                return;
            }

            if (
                btn.classList.contains(
                    'notify-btn'
                )
            ) {

                showToast(
                    'Você será avisado quando o produto chegar!'
                );

                return;
            }

            addToCart(
                Number(btn.dataset.id),
                1
            );
        }
    );
}


/* ============================================================
   HOME — DESTAQUES
   ============================================================ */

function renderFeatured() {

    const grid =
        document.getElementById(
            'featured-grid'
        );

    if (!grid) {
        return;
    }

    const featured =
        PRODUCTS
            .filter(
                p =>
                    p.stock > 0 &&
                    p.category !== 'PC Montado'
            )
            .sort(
                (a, b) =>
                    b.rating - a.rating
            )
            .slice(0, 8);

    grid.innerHTML =
        featured
            .map(
                renderProductCard
            )
            .join('');
}


/* ============================================================
   CARROSSEL DE PCS
   ============================================================ */

function renderPcCarousel() {

    const track =
        document.getElementById(
            'pc-carousel'
        );

    if (!track) {
        return;
    }

    const builds =
        PRODUCTS.filter(
            p =>
                p.category ===
                'PC Montado'
        );

    track.innerHTML =
        builds
            .map(
                renderPcCard
            )
            .join('');

    const prevBtn =
        document.getElementById(
            'pc-carousel-prev'
        );

    const nextBtn =
        document.getElementById(
            'pc-carousel-next'
        );

    const scrollAmount = 290;

    if (prevBtn) {

        prevBtn.addEventListener(
            'click',
            () => {

                track.scrollBy({
                    left:
                        -scrollAmount * 2,
                    behavior:
                        'smooth'
                });

            }
        );
    }

    if (nextBtn) {

        nextBtn.addEventListener(
            'click',
            () => {

                track.scrollBy({
                    left:
                        scrollAmount * 2,
                    behavior:
                        'smooth'
                });

            }
        );
    }
}


/* ============================================================
   SCROLL REVEAL
   ============================================================ */

function initReveal() {

    const items =
        document.querySelectorAll(
            '.reveal'
        );

    if (!items.length) {
        return;
    }

    const observer =
        new IntersectionObserver(
            (entries) => {

                entries.forEach(
                    entry => {

                        if (
                            entry.isIntersecting
                        ) {

                            entry.target
                                .classList.add(
                                    'revealed'
                                );

                            observer.unobserve(
                                entry.target
                            );
                        }
                    }
                );

            },
            {
                threshold: 0.1
            }
        );

    items.forEach(
        item =>
            observer.observe(item)
    );
}


/* ============================================================
   CARROSSEL DE BANNERS
   ============================================================ */

function initHeroCarousel() {

    const track =
        document.getElementById(
            'hero-carousel-track'
        );

    if (!track) {
        return;
    }

    const slides =
        track.querySelectorAll(
            '.hero-slide'
        );

    const dotsWrap =
        document.getElementById(
            'hero-dots'
        );

    const prevBtn =
        document.getElementById(
            'hero-prev'
        );

    const nextBtn =
        document.getElementById(
            'hero-next'
        );

    if (
        !slides.length ||
        !dotsWrap
    ) {
        return;
    }

    let index = 0;
    let timer = null;

    dotsWrap.innerHTML =
        Array.from(slides)
            .map(
                (_, i) =>
                    `
                    <button
                        aria-label="Ir para banner ${i + 1}"
                        class="${
                            i === 0
                                ? 'active'
                                : ''
                        }"
                        data-index="${i}"
                    ></button>
                    `
            )
            .join('');

    const dots =
        dotsWrap.querySelectorAll(
            'button'
        );

    function goTo(i) {

        index =
            (i + slides.length) %
            slides.length;

        track.style.transform =
            `translateX(-${
                index * 100
            }%)`;

        dots.forEach(
            (d, di) =>
                d.classList.toggle(
                    'active',
                    di === index
                )
        );
    }

    function next() {

        goTo(
            index + 1
        );
    }

    function prev() {

        goTo(
            index - 1
        );
    }

    function startAuto() {

        stopAuto();

        timer =
            setInterval(
                next,
                5500
            );
    }

    function stopAuto() {

        if (timer) {

            clearInterval(
                timer
            );

            timer = null;
        }
    }

    if (prevBtn) {

        prevBtn.addEventListener(
            'click',
            () => {

                prev();
                startAuto();

            }
        );
    }

    if (nextBtn) {

        nextBtn.addEventListener(
            'click',
            () => {

                next();
                startAuto();

            }
        );
    }

    dots.forEach(
        d =>
            d.addEventListener(
                'click',
                () => {

                    goTo(
                        Number(
                            d.dataset.index
                        )
                    );

                    startAuto();

                }
            )
    );

    const section =
        document.getElementById(
            'hero-carousel'
        );

    if (section) {

        section.addEventListener(
            'mouseenter',
            stopAuto
        );

        section.addEventListener(
            'mouseleave',
            startAuto
        );
    }

    goTo(0);

    startAuto();
}


/* ============================================================
   ANO DO FOOTER
   ============================================================ */

function initFooterYear() {

    document
        .querySelectorAll(
            '.current-year'
        )
        .forEach(
            el =>
                el.textContent =
                    new Date()
                        .getFullYear()
        );
}


/* ============================================================
   LOADING DOS PRODUTOS
   ============================================================ */

function showProductsLoading() {

    document
        .querySelectorAll(
            '#featured-grid, #pc-carousel, #catalog-grid, #related-grid, #favorites-grid'
        )
        .forEach(
            el => {

                el.innerHTML =
                    '<div class="loading-state">' +
                    '<i class="bi bi-arrow-repeat spin"></i> ' +
                    'Carregando produtos...' +
                    '</div>';

            }
        );
}


/* ============================================================
   INICIALIZAÇÃO GERAL
   ============================================================ */

document.addEventListener(
    'DOMContentLoaded',
    async () => {

        initLoginForm();

        preventLoggedUserFromLoginPage();

        updateHeaderAccount();

        initHeaderScroll();

        initMobileMenu();

        initMobileDropdowns();

        initWhatsapp();

        initThemeToggle();

        initHeroCarousel();

        initFavButtons();

        initBuyButtons();

        initFooterYear();

        updateFavCount();

        if (
            typeof updateCartCount ===
            'function'
        ) {

            updateCartCount();
        }

        showProductsLoading();

        if (
            window.productsReadyPromise
        ) {

            await window.productsReadyPromise;
        }

        if (
            typeof usedFallbackProducts !==
                'undefined' &&
            usedFallbackProducts &&
            typeof API_CONFIG !==
                'undefined' &&
            API_CONFIG.useApi
        ) {

            showToast(
                'Não foi possível conectar à API — exibindo catálogo de demonstração.',
                'error'
            );
        }

        initSearch();

        renderFeatured();

        renderPcCarousel();

        initReveal();

    }
);