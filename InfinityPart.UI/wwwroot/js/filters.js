/* ============================================================
   INFINITY PARTS — filters.js
   Catálogo:
   - Filtro por categoria
   - Filtro por marca
   - Filtro por preço
   - Busca
   - Ordenação
   - PC Gamer
   - Ofertas
   - Painel mobile
   ============================================================ */

let currentFilters = {
    categories: [],
    brands: [],
    minPrice: null,
    maxPrice: null,
    search: '',
    sort: 'relevance',
    offersOnly: false
};


/* ============================================================
   QUERY STRING
============================================================ */

function getQueryParam(name) {
    return new URLSearchParams(window.location.search).get(name);
}


/* ============================================================
   NORMALIZAÇÃO
============================================================ */

function normalizeText(text) {
    return String(text || '')
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .trim()
        .toLowerCase();
}


/* ============================================================
   CATEGORIAS
============================================================ */

function categoryMatches(productCategory, requestedCategory) {

    const product = normalizeText(productCategory);
    const requested = normalizeText(requestedCategory);

    if (!product || !requested) {
        return false;
    }

    // Correspondência exata
    if (product === requested) {
        return true;
    }

    // Aliases das categorias usadas no menu
    const aliases = {

        'placa de video': [
            'placa de video',
            'placas de video',
            'gpu',
            'placa video'
        ],

        'processador': [
            'processador',
            'processadores',
            'cpu'
        ],

        'placa mae': [
            'placa mae',
            'placas mae',
            'placa-mae',
            'motherboard',
            'motherboard'
        ],

        'memoria ram': [
            'memoria ram',
            'memorias ram',
            'ram',
            'memoria'
        ],

        'teclado': [
            'teclado',
            'teclados'
        ],

        'mouse': [
            'mouse',
            'mouses'
        ],

        'headset': [
            'headset',
            'headsets'
        ],

        'monitor': [
            'monitor',
            'monitores'
        ],

        'cabos e adaptadores': [
            'cabos e adaptadores',
            'cabo e adaptador',
            'cabos',
            'adaptadores'
        ],

        'pc gamer': [
            'pc gamer',
            'pcgamer',
            'computador gamer',
            'computadores gamer'
        ]

    };

    const possibleAliases = aliases[requested];

    if (possibleAliases) {
        return possibleAliases.some(alias => {
            return product === normalizeText(alias);
        });
    }

    return false;
}


/* ============================================================
   PC GAMER
============================================================ */

function isProductPcGamer(product) {

    if (!product) {
        return false;
    }

    if (product.isPcGamer === true) {
        return true;
    }

    const category = normalizeText(product.category);
    const name = normalizeText(product.name);
    const description = normalizeText(product.desc);

    if (category === 'pc gamer') {
        return true;
    }

    return (
        name.includes('pc gamer') ||
        name.includes('computador gamer') ||
        description.includes('pc gamer') ||
        description.includes('computador gamer')
    );
}


/* ============================================================
   OFERTAS
============================================================ */

function isProductOffer(product) {

    if (!product) {
        return false;
    }

    if (product.isOffer === true) {
        return true;
    }

    if (Array.isArray(product.badges)) {
        return product.badges.some(
            badge => normalizeText(badge) === 'oferta'
        );
    }

    return false;
}


/* ============================================================
   APLICAÇÃO DOS FILTROS
============================================================ */

function applyFilters() {

    let result = [...PRODUCTS];


    /* ==========================================================
       BUSCA
    ========================================================== */

    if (currentFilters.search) {

        const t = normalizeText(currentFilters.search);

        result = result.filter(p => {

            return (
                normalizeText(p.name).includes(t) ||
                normalizeText(p.category).includes(t) ||
                normalizeText(p.brand).includes(t) ||
                normalizeText(p.desc).includes(t)
            );

        });
    }


    /* ==========================================================
       CATEGORIA
    ========================================================== */

    if (currentFilters.categories.length) {

        result = result.filter(product => {

            return currentFilters.categories.some(category => {

                return categoryMatches(
                    product.category,
                    category
                );

            });

        });
    }


    /* ==========================================================
       PC GAMER
    ========================================================== */

    if (currentFilters.pcGamerOnly) {

        result = result.filter(product => {

            return isProductPcGamer(product);

        });

    }


    /* ==========================================================
       OFERTAS
    ========================================================== */

    if (currentFilters.offersOnly) {

        result = result.filter(product => {

            return isProductOffer(product);

        });

    }


    /* ==========================================================
       MARCA
    ========================================================== */

    if (currentFilters.brands.length) {

        result = result.filter(product => {

            return currentFilters.brands.includes(product.brand);

        });

    }


    /* ==========================================================
       PREÇO MÍNIMO
    ========================================================== */

    if (currentFilters.minPrice !== null) {

        result = result.filter(product => {

            return product.salePrice >= currentFilters.minPrice;

        });

    }


    /* ==========================================================
       PREÇO MÁXIMO
    ========================================================== */

    if (currentFilters.maxPrice !== null) {

        result = result.filter(product => {

            return product.salePrice <= currentFilters.maxPrice;

        });

    }


    /* ==========================================================
       ORDENAÇÃO
    ========================================================== */

    switch (currentFilters.sort) {

        case 'price-asc':

            result.sort(
                (a, b) => a.salePrice - b.salePrice
            );

            break;


        case 'price-desc':

            result.sort(
                (a, b) => b.salePrice - a.salePrice
            );

            break;


        case 'best-selling':

            result.sort(
                (a, b) => b.reviews - a.reviews
            );

            break;


        case 'best-rated':

            result.sort(
                (a, b) => b.rating - a.rating
            );

            break;


        case 'newest':

            result.sort(
                (a, b) => b.id - a.id
            );

            break;


        default:

            // relevância = ordem original

            break;
    }


    renderCatalog(result);
}


/* ============================================================
   RENDERIZAÇÃO DO CATÁLOGO
============================================================ */

function renderCatalog(list) {

    const grid =
        document.getElementById('catalog-grid');

    const countEl =
        document.getElementById('results-count');


    if (!grid) {
        return;
    }


    if (countEl) {

        countEl.textContent =
            `${list.length} produto${list.length !== 1 ? 's' : ''} encontrado${list.length !== 1 ? 's' : ''}`;

    }


    if (list.length === 0) {

        grid.innerHTML = `
      <div class="empty-state">
        <i class="bi bi-search"></i>
        <p>Nenhum produto encontrado.</p>
      </div>
    `;

        return;
    }


    grid.innerHTML =
        list.map(renderProductCard).join('');
}


/* ============================================================
   SIDEBAR
============================================================ */

function buildFilterSidebar() {

    const categories =
        [...new Set(
            PRODUCTS
                .map(p => p.category)
                .filter(Boolean)
        )].sort();


    const brands =
        [...new Set(
            PRODUCTS
                .map(p => p.brand)
                .filter(Boolean)
        )].sort();


    const catBox =
        document.getElementById('category-filters');

    const brandBox =
        document.getElementById('brand-filters');


    if (catBox) {

        catBox.innerHTML =
            categories.map(category => `
        <label class="filter-check">

          <input
            type="checkbox"
            value="${category}"
            class="cat-check"
          >

          ${category}

        </label>
      `).join('');

    }


    if (brandBox) {

        brandBox.innerHTML =
            brands.map(brand => `
        <label class="filter-check">

          <input
            type="checkbox"
            value="${brand}"
            class="brand-check"
          >

          ${brand}

        </label>
      `).join('');

    }
}


/* ============================================================
   MARCAR CHECKBOX DA CATEGORIA
============================================================ */

function checkCategory(category) {

    const checkboxes =
        document.querySelectorAll('.cat-check');


    checkboxes.forEach(checkbox => {

        if (
            categoryMatches(
                checkbox.value,
                category
            )
        ) {

            checkbox.checked = true;

        }

    });
}


/* ============================================================
   EVENTOS
============================================================ */

function initFilterEvents() {

    document.body.addEventListener(
        'change',
        event => {


            /* ======================================================
               CATEGORIA
            ====================================================== */

            if (
                event.target.classList.contains('cat-check')
            ) {

                const checked =
                    [
                        ...document.querySelectorAll(
                            '.cat-check:checked'
                        )
                    ].map(
                        input => input.value
                    );


                currentFilters.categories =
                    checked;

                currentFilters.pcGamerOnly =
                    false;

                currentFilters.offersOnly =
                    false;


                applyFilters();

            }


            /* ======================================================
               MARCA
            ====================================================== */

            if (
                event.target.classList.contains('brand-check')
            ) {

                const checked =
                    [
                        ...document.querySelectorAll(
                            '.brand-check:checked'
                        )
                    ].map(
                        input => input.value
                    );


                currentFilters.brands =
                    checked;


                applyFilters();

            }


            /* ======================================================
               ORDENAÇÃO
            ====================================================== */

            if (
                event.target.id === 'sort-select'
            ) {

                currentFilters.sort =
                    event.target.value;


                applyFilters();

            }

        }
    );


    /* ==========================================================
       PREÇO
    ========================================================== */

    const applyPriceBtn =
        document.getElementById(
            'apply-price'
        );


    if (applyPriceBtn) {

        applyPriceBtn.addEventListener(
            'click',
            () => {

                const min =
                    document.getElementById(
                        'price-min'
                    ).value;


                const max =
                    document.getElementById(
                        'price-max'
                    ).value;


                currentFilters.minPrice =
                    min ? Number(min) : null;


                currentFilters.maxPrice =
                    max ? Number(max) : null;


                applyFilters();

            }
        );

    }


    /* ==========================================================
       LIMPAR FILTROS
    ========================================================== */

    const clearBtn =
        document.getElementById(
            'clear-filters'
        );


    if (clearBtn) {

        clearBtn.addEventListener(
            'click',
            () => {

                currentFilters = {

                    categories: [],

                    brands: [],

                    minPrice: null,

                    maxPrice: null,

                    search:
                        currentFilters.search,

                    sort: 'relevance',

                    pcGamerOnly: false,

                    offersOnly: false

                };


                document
                    .querySelectorAll(
                        '.cat-check, .brand-check'
                    )
                    .forEach(
                        input => input.checked = false
                    );


                const priceMin =
                    document.getElementById(
                        'price-min'
                    );


                const priceMax =
                    document.getElementById(
                        'price-max'
                    );


                if (priceMin) {
                    priceMin.value = '';
                }


                if (priceMax) {
                    priceMax.value = '';
                }


                const sortSelect =
                    document.getElementById(
                        'sort-select'
                    );


                if (sortSelect) {
                    sortSelect.value =
                        'relevance';
                }


                applyFilters();


                if (typeof showToast === 'function') {

                    showToast(
                        'Filtros limpos.'
                    );

                }

            }
        );

    }


    /* ==========================================================
       FILTROS MOBILE
    ========================================================== */

    const mobileToggle =
        document.getElementById(
            'filter-toggle-mobile'
        );


    const sidebar =
        document.getElementById(
            'filters-sidebar'
        );


    const overlay =
        document.getElementById(
            'filters-overlay'
        );


    if (
        mobileToggle &&
        sidebar
    ) {

        mobileToggle.addEventListener(
            'click',
            () => {

                sidebar.classList.add(
                    'open'
                );


                if (overlay) {

                    overlay.classList.add(
                        'active'
                    );

                }


                document.body.classList.add(
                    'no-scroll'
                );

            }
        );

    }


    const closeFilters =
        document.getElementById(
            'close-filters'
        );


    function closeSidebar() {

        if (sidebar) {

            sidebar.classList.remove(
                'open'
            );

        }


        if (overlay) {

            overlay.classList.remove(
                'active'
            );

        }


        document.body.classList.remove(
            'no-scroll'
        );

    }


    if (closeFilters) {

        closeFilters.addEventListener(
            'click',
            closeSidebar
        );

    }


    if (overlay) {

        overlay.addEventListener(
            'click',
            closeSidebar
        );

    }
}


/* ============================================================
   INICIALIZAÇÃO
============================================================ */

document.addEventListener(
    'DOMContentLoaded',
    async () => {

        const grid =
            document.getElementById(
                'catalog-grid'
            );


        if (!grid) {
            return;
        }


        grid.innerHTML = `
      <div class="loading-state">
        <i class="bi bi-arrow-repeat spin"></i>
        Carregando produtos...
      </div>
    `;


        await window.productsReadyPromise;


        buildFilterSidebar();


        /* ========================================================
           BUSCA
        ======================================================== */

        const busca =
            getQueryParam('busca');


        if (busca) {

            currentFilters.search =
                busca;


            const input =
                document.getElementById(
                    'search-input'
                );


            if (input) {

                input.value =
                    busca;

            }

        }


        /* ========================================================
           CATEGORIA
        ======================================================== */

        const categoria =
            getQueryParam('categoria');


        if (categoria) {

            currentFilters.categories =
                [categoria];


            checkCategory(
                categoria
            );

        }


        /* ========================================================
           PC GAMER
        ======================================================== */

        const pcGamer =
            getQueryParam('pcgamer');


        if (
            pcGamer === '1' ||
            normalizeText(categoria) === 'pc gamer'
        ) {

            currentFilters.categories =
                [];


            currentFilters.pcGamerOnly =
                true;

        }


        /* ========================================================
           OFERTAS
        ======================================================== */

        const ofertas =
            getQueryParam('ofertas');


        if (
            ofertas === '1'
        ) {

            currentFilters.categories =
                [];


            currentFilters.offersOnly =
                true;

        }


        /* ========================================================
           EVENTOS
        ======================================================== */

        initFilterEvents();


        /* ========================================================
           APLICAR
        ======================================================== */

        applyFilters();

    }
);