/* ============================================================
   INFINITY PARTS — cart.js
   Carrinho de compras com persistência em localStorage,
   cupons de desconto e cálculo de frete simulado.
   ============================================================ */

const CART_KEY = 'ip_cart';
const COUPONS = {
  'INFINITY10': { type: 'percent', value: 10 },
  'GANHE10':    { type: 'fixed',   value: 10 },
  'PARTS5':     { type: 'percent', value: 5 }
};

function getCart() {
  return JSON.parse(localStorage.getItem(CART_KEY) || '[]');
}
function saveCart(cart) {
  localStorage.setItem(CART_KEY, JSON.stringify(cart));
  updateCartCount();
}
function updateCartCount() {
  const el = document.getElementById('cart-count');
  if (!el) return;
  const cart = getCart();
  el.textContent = cart.reduce((sum, i) => sum + i.qty, 0);
}

function addToCart(id, qty = 1) {
  const product = getProductById(id);
  if (!product || product.stock <= 0) return;
  const cart = getCart();
  const existing = cart.find(i => i.id === id);
  if (existing) {
    existing.qty = Math.min(existing.qty + qty, product.stock);
  } else {
    cart.push({ id, qty: Math.min(qty, product.stock) });
  }
  saveCart(cart);
  showToast('Produto adicionado ao carrinho.');
}

function removeFromCart(id) {
  let cart = getCart().filter(i => i.id !== id);
  saveCart(cart);
  showToast('Produto removido.');
  renderCartPage();
}

function updateCartQty(id, qty) {
  const product = getProductById(id);
  const cart = getCart();
  const item = cart.find(i => i.id === id);
  if (!item) return;
  item.qty = Math.max(1, Math.min(qty, product.stock));
  saveCart(cart);
  renderCartPage();
}

function getAppliedCoupon() {
  return JSON.parse(sessionStorage.getItem('ip_coupon') || 'null');
}

function calcCartTotals() {
  const cart = getCart();
  let subtotal = 0;
  cart.forEach(item => {
    const p = getProductById(item.id);
    if (p) subtotal += p.salePrice * item.qty;
  });

  let discount = 0;
  const coupon = getAppliedCoupon();
  if (coupon && COUPONS[coupon]) {
    const c = COUPONS[coupon];
    discount = c.type === 'percent' ? subtotal * (c.value / 100) : c.value;
    discount = Math.min(discount, subtotal);
  }

  const shipping = JSON.parse(sessionStorage.getItem('ip_shipping') || 'null');
  const shippingCost = shipping ? shipping.price : 0;

  const total = Math.max(subtotal - discount + shippingCost, 0);
  return { subtotal, discount, shippingCost, total, shipping, coupon };
}

/* ---------- RENDER: PÁGINA DO CARRINHO ---------- */
function renderCartPage() {
  const container = document.getElementById('cart-items');
  if (!container) return;
  const cart = getCart();

  if (cart.length === 0) {
    container.innerHTML = `
      <div class="empty-state">
        <i class="bi bi-cart-x"></i>
        <p>Seu carrinho está vazio.</p>
        <a href="produtos.html" class="btn btn-primary">VER PRODUTOS</a>
      </div>`;
  } else {
    container.innerHTML = cart.map(item => {
      const p = getProductById(item.id);
      if (!p) return '';
      const subtotal = p.salePrice * item.qty;
      return `
      <div class="cart-item" data-id="${p.id}">
        ${p.image
          ? `<img src="${resolveImageUrl(p.image)}" alt="${p.name}">`
          : `<div class="no-image-placeholder compact"><i class="bi bi-image"></i></div>`}
        <div class="cart-item-info">
          <a href="produto.html?id=${p.id}" class="cart-item-name">${p.name}</a>
          <span class="cart-item-brand">${p.brand}</span>
          <span class="cart-item-price">${formatBRL(p.salePrice)} / un</span>
        </div>
        <div class="qty-selector">
          <button class="qty-btn qty-minus" data-id="${p.id}" aria-label="Diminuir quantidade">−</button>
          <input type="number" class="qty-input" data-id="${p.id}" value="${item.qty}" min="1" max="${p.stock}" aria-label="Quantidade">
          <button class="qty-btn qty-plus" data-id="${p.id}" aria-label="Aumentar quantidade">+</button>
        </div>
        <span class="cart-item-subtotal">${formatBRL(subtotal)}</span>
        <button class="cart-item-remove" data-id="${p.id}" aria-label="Remover produto"><i class="bi bi-trash3"></i></button>
      </div>`;
    }).join('');
  }

  renderCartSummary();
}

function renderCartSummary() {
  const { subtotal, discount, shippingCost, total, shipping, coupon } = calcCartTotals();
  const elSub = document.getElementById('summary-subtotal');
  const elDisc = document.getElementById('summary-discount');
  const elShip = document.getElementById('summary-shipping');
  const elTotal = document.getElementById('summary-total');
  if (elSub) elSub.textContent = formatBRL(subtotal);
  if (elDisc) elDisc.textContent = `- ${formatBRL(discount)}`;
  if (elShip) elShip.textContent = shipping ? `${formatBRL(shippingCost)} (${shipping.name})` : 'Calcular';
  if (elTotal) elTotal.textContent = formatBRL(total);

  const couponMsg = document.getElementById('coupon-message');
  if (couponMsg) {
    couponMsg.textContent = coupon ? `Cupom "${coupon}" aplicado!` : '';
    couponMsg.className = coupon ? 'coupon-message success' : 'coupon-message';
  }

  const checkoutBtn = document.getElementById('btn-checkout');
  if (checkoutBtn) checkoutBtn.classList.toggle('disabled', getCart().length === 0);
}

/* ---------- CUPOM ---------- */
function initCoupon() {
  const btn = document.getElementById('apply-coupon');
  const input = document.getElementById('coupon-input');
  if (!btn || !input) return;
  btn.addEventListener('click', () => {
    const code = input.value.trim().toUpperCase();
    if (!code) return;
    if (COUPONS[code]) {
      sessionStorage.setItem('ip_coupon', JSON.stringify(code));
      showToast('Cupom aplicado com sucesso.');
    } else {
      showToast('Cupom inválido.', 'error');
      sessionStorage.removeItem('ip_coupon');
    }
    renderCartSummary();
  });
}

/* ---------- FRETE (simulado) ---------- */
function initShipping() {
  const btn = document.getElementById('calc-shipping');
  const input = document.getElementById('cep-input');
  const result = document.getElementById('shipping-result');
  if (!btn || !input) return;

  btn.addEventListener('click', () => {
    const cep = input.value.replace(/\D/g, '');
    if (cep.length !== 8) {
      showToast('Digite um CEP válido.', 'error');
      return;
    }
    /* Simulação de cálculo de frete no front-end.
       Estrutura preparada para futura integração com API real (Correios/transportadora). */
    const seed = parseInt(cep.slice(0, 3), 10) || 100;
    const options = [
      { id: 'pac', name: 'PAC', price: +(19.9 + (seed % 15)).toFixed(2), days: '5 a 8 dias úteis' },
      { id: 'sedex', name: 'SEDEX', price: +(34.9 + (seed % 20)).toFixed(2), days: '2 a 4 dias úteis' },
      { id: 'transp', name: 'Transportadora', price: +(14.9 + (seed % 10)).toFixed(2), days: '8 a 12 dias úteis' }
    ];

    result.innerHTML = options.map(o => `
      <label class="shipping-option">
        <input type="radio" name="shipping" value="${o.id}" data-name="${o.name}" data-price="${o.price}">
        <span class="shipping-name">${o.name}</span>
        <span class="shipping-days">${o.days}</span>
        <span class="shipping-price">${formatBRL(o.price)}</span>
      </label>`).join('');
    result.classList.add('active');
    showToast('CEP calculado.');

    result.querySelectorAll('input[name="shipping"]').forEach(radio => {
      radio.addEventListener('change', () => {
        sessionStorage.setItem('ip_shipping', JSON.stringify({ name: radio.dataset.name, price: +radio.dataset.price }));
        renderCartSummary();
      });
    });
  });
}

/* ---------- EVENTOS DELEGADOS DO CARRINHO ---------- */
function initCartEvents() {
  const container = document.getElementById('cart-items');
  if (!container) return;

  container.addEventListener('click', (e) => {
    const minus = e.target.closest('.qty-minus');
    const plus = e.target.closest('.qty-plus');
    const remove = e.target.closest('.cart-item-remove');
    if (minus) {
      const id = Number(minus.dataset.id);
      const item = getCart().find(i => i.id === id);
      if (item) updateCartQty(id, item.qty - 1 <= 0 ? 1 : item.qty - 1);
      if (item && item.qty <= 1) return;
    }
    if (plus) {
      const id = Number(plus.dataset.id);
      const item = getCart().find(i => i.id === id);
      if (item) updateCartQty(id, item.qty + 1);
    }
    if (remove) {
      removeFromCart(Number(remove.dataset.id));
    }
  });

  container.addEventListener('change', (e) => {
    if (e.target.classList.contains('qty-input')) {
      updateCartQty(Number(e.target.dataset.id), Number(e.target.value));
    }
  });
}

document.addEventListener('DOMContentLoaded', async () => {
  const grid = document.getElementById('cart-items');
  if (grid) grid.innerHTML = '<div class="loading-state"><i class="bi bi-arrow-repeat spin"></i> Carregando carrinho...</div>';
  await window.productsReadyPromise;
  renderCartPage();
  initCoupon();
  initShipping();
  initCartEvents();
});
