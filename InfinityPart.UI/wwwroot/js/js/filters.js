/* ============================================================
   INFINITY PARTS — filters.js
   Página de catálogo: filtros por categoria/marca/preço,
   ordenação, busca vinda da home e painel mobile.
   ============================================================ */

let currentFilters = { categories: [], brands: [], minPrice: null, maxPrice: null, search: '', sort: 'relevance' };

function getQueryParam(name) {
  return new URLSearchParams(window.location.search).get(name);
}

function applyFilters() {
  let result = [...PRODUCTS];

  if (currentFilters.search) {
    const t = currentFilters.search.toLowerCase();
    result = result.filter(p =>
      p.name.toLowerCase().includes(t) ||
      p.category.toLowerCase().includes(t) ||
      p.brand.toLowerCase().includes(t) ||
      p.desc.toLowerCase().includes(t)
    );
  }
  if (currentFilters.categories.length) {
    result = result.filter(p => currentFilters.categories.includes(p.category));
  }
  if (currentFilters.brands.length) {
    result = result.filter(p => currentFilters.brands.includes(p.brand));
  }
  if (currentFilters.minPrice !== null) {
    result = result.filter(p => p.salePrice >= currentFilters.minPrice);
  }
  if (currentFilters.maxPrice !== null) {
    result = result.filter(p => p.salePrice <= currentFilters.maxPrice);
  }

  switch (currentFilters.sort) {
    case 'price-asc':  result.sort((a, b) => a.salePrice - b.salePrice); break;
    case 'price-desc': result.sort((a, b) => b.salePrice - a.salePrice); break;
    case 'best-selling': result.sort((a, b) => b.reviews - a.reviews); break;
    case 'best-rated': result.sort((a, b) => b.rating - a.rating); break;
    case 'newest': result.sort((a, b) => b.id - a.id); break;
    default: break; /* relevância = ordem original */
  }

  renderCatalog(result);
}

function renderCatalog(list) {
  const grid = document.getElementById('catalog-grid');
  const countEl = document.getElementById('results-count');
  if (!grid) return;

  if (countEl) countEl.textContent = `${list.length} produto${list.length !== 1 ? 's' : ''} encontrado${list.length !== 1 ? 's' : ''}`;

  if (list.length === 0) {
    grid.innerHTML = `<div class="empty-state"><i class="bi bi-search"></i><p>Nenhum produto encontrado.</p></div>`;
    return;
  }
  grid.innerHTML = list.map(renderProductCard).join('');
}

function buildFilterSidebar() {
  const categories = [...new Set(PRODUCTS.map(p => p.category))].sort();
  const brands = [...new Set(PRODUCTS.map(p => p.brand))].sort();

  const catBox = document.getElementById('category-filters');
  const brandBox = document.getElementById('brand-filters');

  if (catBox) {
    catBox.innerHTML = categories.map(c => `
      <label class="filter-check">
        <input type="checkbox" value="${c}" class="cat-check"> ${c}
      </label>`).join('');
  }
  if (brandBox) {
    brandBox.innerHTML = brands.map(b => `
      <label class="filter-check">
        <input type="checkbox" value="${b}" class="brand-check"> ${b}
      </label>`).join('');
  }
}

function initFilterEvents() {
  document.body.addEventListener('change', (e) => {
    if (e.target.classList.contains('cat-check')) {
      const checked = [...document.querySelectorAll('.cat-check:checked')].map(i => i.value);
      currentFilters.categories = checked;
      applyFilters();
    }
    if (e.target.classList.contains('brand-check')) {
      const checked = [...document.querySelectorAll('.brand-check:checked')].map(i => i.value);
      currentFilters.brands = checked;
      applyFilters();
    }
    if (e.target.id === 'sort-select') {
      currentFilters.sort = e.target.value;
      applyFilters();
    }
  });

  const applyPriceBtn = document.getElementById('apply-price');
  if (applyPriceBtn) {
    applyPriceBtn.addEventListener('click', () => {
      const min = document.getElementById('price-min').value;
      const max = document.getElementById('price-max').value;
      currentFilters.minPrice = min ? Number(min) : null;
      currentFilters.maxPrice = max ? Number(max) : null;
      applyFilters();
    });
  }

  const clearBtn = document.getElementById('clear-filters');
  if (clearBtn) {
    clearBtn.addEventListener('click', () => {
      currentFilters = { categories: [], brands: [], minPrice: null, maxPrice: null, search: currentFilters.search, sort: 'relevance' };
      document.querySelectorAll('.cat-check, .brand-check').forEach(i => i.checked = false);
      const priceMin = document.getElementById('price-min');
      const priceMax = document.getElementById('price-max');
      if (priceMin) priceMin.value = '';
      if (priceMax) priceMax.value = '';
      const sortSelect = document.getElementById('sort-select');
      if (sortSelect) sortSelect.value = 'relevance';
      applyFilters();
      showToast('Filtros limpos.');
    });
  }

  /* Painel de filtros mobile */
  const mobileToggle = document.getElementById('filter-toggle-mobile');
  const sidebar = document.getElementById('filters-sidebar');
  const overlay = document.getElementById('filters-overlay');
  if (mobileToggle && sidebar) {
    mobileToggle.addEventListener('click', () => {
      sidebar.classList.add('open');
      if (overlay) overlay.classList.add('active');
      document.body.classList.add('no-scroll');
    });
  }
  const closeFilters = document.getElementById('close-filters');
  function closeSidebar() {
    sidebar.classList.remove('open');
    if (overlay) overlay.classList.remove('active');
    document.body.classList.remove('no-scroll');
  }
  if (closeFilters) closeFilters.addEventListener('click', closeSidebar);
  if (overlay) overlay.addEventListener('click', closeSidebar);
}

document.addEventListener('DOMContentLoaded', async () => {
  const grid = document.getElementById('catalog-grid');
  if (!grid) return;
  grid.innerHTML = '<div class="loading-state"><i class="bi bi-arrow-repeat spin"></i> Carregando produtos...</div>';
  await window.productsReadyPromise;
  buildFilterSidebar();

  const busca = getQueryParam('busca');
  const categoria = getQueryParam('categoria');
  if (busca) {
    currentFilters.search = busca;
    const input = document.getElementById('search-input');
    if (input) input.value = busca;
  }
  if (categoria) {
    currentFilters.categories = [categoria];
    const checkbox = document.querySelector(`.cat-check[value="${categoria}"]`);
    if (checkbox) checkbox.checked = true;
  }

  initFilterEvents();
  applyFilters();
});
