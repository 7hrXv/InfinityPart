/* ============================================================
   INFINITY PARTS — checkout.js
   Checkout integrado com a API:
   POST /api/Pedido
   POST /api/ItemPedido
   ============================================================ */

const API_BASE_URL = 'http://localhost:5022/api';

// Status "Pendente" da API
const STATUS_PEDIDO_ID = 2;

let checkoutStep = 1;
let finalizandoPedido = false;


/* ============================================================
   USUÁRIO LOGADO
   ============================================================ */

function getCheckoutUser() {
  try {
    return JSON.parse(
      localStorage.getItem('ip_user') || 'null'
    );
  } catch (error) {
    console.error(
      '[Infinity Parts] Erro ao carregar usuário:',
      error
    );

    return null;
  }
}


/* ============================================================
   ID DO CLIENTE LOGADO
   ============================================================ */

function getCheckoutClientId() {

  const user = getCheckoutUser();

  if (!user) {
    return null;
  }

  /*
   * Compatível com diferentes formatos:
   *
   * ip_user = {
   *   id: 1004,
   *   ...
   * }
   *
   * ou
   *
   * ip_user = {
   *   cliente: {
   *     id: 1004
   *   }
   * }
   *
   * ou
   *
   * ip_user = {
   *   Cliente: {
   *     Id: 1004
   *   }
   * }
   */

  const cliente =
    user.cliente ??
    user.Cliente ??
    null;

  const id =
    user.id ??
    user.Id ??
    user.clienteId ??
    user.ClienteId ??
    cliente?.id ??
    cliente?.Id ??
    cliente?.clienteId ??
    cliente?.ClienteId;

  const clientId = Number(id);

  if (!Number.isInteger(clientId) || clientId <= 0) {
    return null;
  }

  return clientId;
}


/* ============================================================
   NAVEGAÇÃO ENTRE ETAPAS
   ============================================================ */

function goToStep(step) {

  if (getCart().length === 0 && step !== 4) {
    window.location.href = 'carrinho.html';
    return;
  }

  checkoutStep = step;

  document
    .querySelectorAll('.checkout-step-panel')
    .forEach(panel =>
      panel.classList.remove('active')
    );

  const panel = document.querySelector(
    `.checkout-step-panel[data-step="${step}"]`
  );

  if (panel) {
    panel.classList.add('active');
  }

  document
    .querySelectorAll('.step-indicator')
    .forEach(indicator => {

      const currentStep =
        Number(indicator.dataset.step);

      indicator.classList.toggle(
        'active',
        currentStep === step
      );

      indicator.classList.toggle(
        'done',
        currentStep < step
      );

    });

  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  });
}


/* ============================================================
   VALIDAÇÃO
   ============================================================ */

function validateStep(step) {

  const panel = document.querySelector(
    `.checkout-step-panel[data-step="${step}"]`
  );

  if (!panel) {
    return true;
  }

  const requiredFields =
    panel.querySelectorAll('[required]');

  let valid = true;

  requiredFields.forEach(field => {

    field.classList.remove('input-error');

    if (!field.value.trim()) {
      field.classList.add('input-error');
      valid = false;
    }

  });

  if (!valid) {

    showToast(
      'Preencha todos os campos obrigatórios.',
      'error'
    );

  }

  return valid;
}


/* ============================================================
   RESUMO DO CHECKOUT
   ============================================================ */

function renderCheckoutSummary() {

  const box =
    document.getElementById(
      'checkout-summary-items'
    );

  const {
    subtotal,
    discount,
    shippingCost,
    total
  } = calcCartTotals();

  if (box) {

    box.innerHTML =
      getCart()
        .map(item => {

          const product =
            getProductById(item.id);

          if (!product) {
            return '';
          }

          return `
            <div class="checkout-summary-item">

              <span>
                ${item.qty}x ${product.name}
              </span>

              <span>
                ${formatBRL(
                  product.salePrice * item.qty
                )}
              </span>

            </div>
          `;

        })
        .join('');
  }

  const setText = (id, value) => {

    const element =
      document.getElementById(id);

    if (element) {
      element.textContent = value;
    }
  };

  setText(
    'checkout-subtotal',
    formatBRL(subtotal)
  );

  setText(
    'checkout-discount',
    `- ${formatBRL(discount)}`
  );

  setText(
    'checkout-shipping',
    formatBRL(shippingCost)
  );

  setText(
    'checkout-total',
    formatBRL(total)
  );

  const pixValue =
    +(total * 0.95).toFixed(2);

  setText(
    'pix-value',
    formatBRL(pixValue)
  );

  setText(
    'final-total',
    formatBRL(total)
  );
}


/* ============================================================
   MÁSCARAS
   ============================================================ */

function maskCPF(value) {

  return value
    .replace(/\D/g, '')
    .slice(0, 11)
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(
      /(\d{3})(\d{1,2})$/,
      '$1-$2'
    );
}


function maskCEP(value) {

  return value
    .replace(/\D/g, '')
    .slice(0, 8)
    .replace(
      /(\d{5})(\d)/,
      '$1-$2'
    );
}


function maskPhone(value) {

  return value
    .replace(/\D/g, '')
    .slice(0, 11)
    .replace(
      /(\d{2})(\d)/,
      '($1) $2'
    )
    .replace(
      /(\d{5})(\d)/,
      '$1-$2'
    );
}


function maskCard(value) {

  return value
    .replace(/\D/g, '')
    .slice(0, 16)
    .replace(
      /(\d{4})(?=\d)/g,
      '$1 '
    );
}


function initMasks() {

  const cpf =
    document.getElementById('cpf');

  if (cpf) {

    cpf.addEventListener(
      'input',
      () => {
        cpf.value =
          maskCPF(cpf.value);
      }
    );

  }

  const cep =
    document.getElementById('cep');

  if (cep) {

    cep.addEventListener(
      'input',
      () => {
        cep.value =
          maskCEP(cep.value);
      }
    );

  }

  const phone =
    document.getElementById('telefone');

  if (phone) {

    phone.addEventListener(
      'input',
      () => {
        phone.value =
          maskPhone(phone.value);
      }
    );

  }

  const card =
    document.getElementById('card-number');

  if (card) {

    card.addEventListener(
      'input',
      () => {
        card.value =
          maskCard(card.value);
      }
    );

  }
}


/* ============================================================
   PAGAMENTO
   ============================================================ */

function initPaymentToggle() {

  const radios =
    document.querySelectorAll(
      'input[name="payment-method"]'
    );

  radios.forEach(radio => {

    radio.addEventListener(
      'change',
      () => {

        document
          .querySelectorAll('.payment-form')
          .forEach(form =>
            form.classList.remove('active')
          );

        const target =
          document.getElementById(
            `payment-${radio.value}`
          );

        if (target) {
          target.classList.add('active');
        }

      }
    );

  });
}


/* ============================================================
   PIX
   ============================================================ */

function initPixCopy() {

  const button =
    document.getElementById('copy-pix');

  if (!button) {
    return;
  }

  button.addEventListener(
    'click',
    () => {

      const total =
        calcCartTotals().total;

      const fakeCode =
        '00020126580014BR.GOV.BCB.PIX' +
        '0136infinityparts-pix-demo' +
        '520400005303986540' +
        total.toFixed(2).replace('.', '') +
        '5802BR5913INFINITY PARTS6009SAO PAULO' +
        '62070503***6304ABCD';

      navigator.clipboard
        .writeText(fakeCode)
        .then(() => {

          showToast(
            'Pix copiado!'
          );

        })
        .catch(() => {

          showToast(
            'Não foi possível copiar. Copie manualmente.',
            'error'
          );

        });

    }
  );
}


/* ============================================================
   CRIA PEDIDO NA API
   ============================================================ */

async function criarPedidoNaAPI(total) {

  console.log(
    '[Infinity Parts] Criando pedido na API...'
  );

  /*
   * PEGA O CLIENTE REAL QUE ESTÁ LOGADO.
   */
  const clienteId =
    getCheckoutClientId();

  if (!clienteId) {

    throw new Error(
      'Não foi possível identificar o cliente logado.'
    );
  }

  const pedido = {

    clienteId: clienteId,

    statusPedidoId:
      STATUS_PEDIDO_ID,

    valorTotal:
      Number(total.toFixed(2))

  };

  console.log(
    '[Infinity Parts] Cliente logado:',
    clienteId
  );

  console.log(
    '[Infinity Parts] Dados enviados:',
    pedido
  );

  const response =
    await fetch(
      `${API_BASE_URL}/Pedido`,
      {
        method: 'POST',

        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        },

        body:
          JSON.stringify(pedido)
      }
    );

  if (!response.ok) {

    const erro =
      await response.text();

    console.error(
      '[Infinity Parts] Erro ao criar pedido:',
      erro
    );

    throw new Error(
      `Erro ao criar pedido (${response.status})`
    );
  }

  const data =
    await response.json();

  console.log(
    '[Infinity Parts] Pedido criado:',
    data
  );

  return data;
}


/* ============================================================
   CRIA ITEM DO PEDIDO NA API
   ============================================================ */

async function criarItemPedidoNaAPI(
  pedidoId,
  produtoId,
  quantidade,
  precoUnitario
) {

  const itemPedido = {

    pedidoId:
      Number(pedidoId),

    produtoId:
      Number(produtoId),

    quantidade:
      Number(quantidade),

    precoUnitario:
      Number(
        precoUnitario.toFixed(2)
      )

  };

  console.log(
    '[Infinity Parts] Criando item:',
    itemPedido
  );

  const response =
    await fetch(
      `${API_BASE_URL}/ItemPedido`,
      {
        method: 'POST',

        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        },

        body:
          JSON.stringify(itemPedido)
      }
    );

  if (!response.ok) {

    const erro =
      await response.text();

    console.error(
      '[Infinity Parts] Erro ao criar item:',
      erro
    );

    throw new Error(
      `Erro ao criar item do pedido (${response.status})`
    );
  }

  const data =
    await response.json();

  console.log(
    '[Infinity Parts] Item criado:',
    data
  );

  return data;
}


/* ============================================================
   FINALIZAÇÃO DO PEDIDO
   ============================================================ */

async function finalizarPedido() {

  if (finalizandoPedido) {
    return;
  }

  const cart =
    getCart();

  if (!cart || cart.length === 0) {

    showToast(
      'Seu carrinho está vazio.',
      'error'
    );

    window.location.href =
      'carrinho.html';

    return;
  }

  /*
   * Garante que existe um cliente logado.
   */
  const clienteId =
    getCheckoutClientId();

  if (!clienteId) {

    showToast(
      'Faça login para finalizar sua compra.',
      'error'
    );

    setTimeout(() => {

      window.location.href =
        'login.html';

    }, 700);

    return;
  }

  finalizandoPedido = true;

  const finishButton =
    document.getElementById(
      'finish-order'
    );

  const textoOriginal =
    finishButton
      ? finishButton.innerHTML
      : '';

  try {

    if (finishButton) {

      finishButton.disabled = true;

      finishButton.innerHTML =
        `
          <i class="bi bi-arrow-repeat spin"></i>
          PROCESSANDO PEDIDO...
        `;

    }


    /* --------------------------------------------------------
       CALCULA O TOTAL
       -------------------------------------------------------- */

    const {
      total
    } = calcCartTotals();

    if (
      !Number.isFinite(total) ||
      total <= 0
    ) {

      throw new Error(
        'O valor total do pedido é inválido.'
      );
    }


    /* --------------------------------------------------------
       1. CRIA O PEDIDO
       -------------------------------------------------------- */

    console.log(
      '[Infinity Parts] Finalizando para cliente:',
      clienteId
    );

    const pedidoCriado =
      await criarPedidoNaAPI(total);


    /* --------------------------------------------------------
       ID DO PEDIDO
       -------------------------------------------------------- */

    const pedidoId =
      pedidoCriado?.id ??
      pedidoCriado?.pedidoId ??
      pedidoCriado?.Id ??
      pedidoCriado?.PedidoId;

    if (!pedidoId) {

      console.error(
        '[Infinity Parts] Resposta da API:',
        pedidoCriado
      );

      throw new Error(
        'A API criou o pedido, mas não retornou o ID.'
      );
    }


    /* --------------------------------------------------------
       2. CRIA OS ITENS DO PEDIDO
       -------------------------------------------------------- */

    for (const item of cart) {

      const product =
        getProductById(item.id);

      if (!product) {

        throw new Error(
          `Produto ${item.id} não encontrado.`
        );
      }

      const precoUnitario =
        Number(
          product.salePrice
        );

      await criarItemPedidoNaAPI(
        pedidoId,
        item.id,
        item.qty,
        precoUnitario
      );

    }


    /* --------------------------------------------------------
       3. SUCESSO
       -------------------------------------------------------- */

    const orderNumber =
      'IP' +
      String(pedidoId).padStart(
        5,
        '0'
      );

    const setText =
      (id, value) => {

        const element =
          document.getElementById(id);

        if (element) {
          element.textContent =
            value;
        }

      };

    setText(
      'order-number',
      orderNumber
    );


    /* --------------------------------------------------------
       LIMPA O CARRINHO
       SOMENTE APÓS PEDIDO + ITENS
       -------------------------------------------------------- */

    localStorage.removeItem(
      CART_KEY
    );

    sessionStorage.removeItem(
      'ip_coupon'
    );

    sessionStorage.removeItem(
      'ip_shipping'
    );

    if (
      typeof updateCartCount ===
      'function'
    ) {

      updateCartCount();

    }


    showToast(
      'Pedido realizado com sucesso!'
    );


    goToStep(4);


    console.log(
      '[Infinity Parts] PEDIDO FINALIZADO COM SUCESSO',
      {
        pedidoId,
        clienteId,
        orderNumber
      }
    );

  } catch (error) {

    console.error(
      '[Infinity Parts] ERRO AO FINALIZAR PEDIDO:',
      error
    );

    showToast(
      'Não foi possível finalizar o pedido. Verifique a API.',
      'error'
    );

  } finally {

    finalizandoPedido =
      false;

    if (finishButton) {

      finishButton.disabled =
        false;

      finishButton.innerHTML =
        textoOriginal;

    }

  }
}


/* ============================================================
   NAVEGAÇÃO ENTRE ETAPAS
   ============================================================ */

function initStepNavigation() {

  document
    .querySelectorAll('.btn-next-step')
    .forEach(button => {

      button.addEventListener(
        'click',
        () => {

          if (
            validateStep(checkoutStep)
          ) {

            goToStep(
              checkoutStep + 1
            );

          }

        }
      );

    });


  document
    .querySelectorAll('.btn-prev-step')
    .forEach(button => {

      button.addEventListener(
        'click',
        () => {

          goToStep(
            checkoutStep - 1
          );

        }
      );

    });


  const finishButton =
    document.getElementById(
      'finish-order'
    );

  if (finishButton) {

    finishButton.addEventListener(
      'click',
      async () => {

        if (
          !validateStep(checkoutStep)
        ) {

          return;

        }

        await finalizarPedido();

      }
    );

  }
}


/* ============================================================
   INICIALIZAÇÃO
   ============================================================ */

document.addEventListener(
  'DOMContentLoaded',
  async () => {

    if (
      !document.querySelector(
        '.checkout-wrapper'
      )
    ) {

      return;

    }


    if (
      getCart().length === 0
    ) {

      window.location.href =
        'carrinho.html';

      return;

    }


    /*
     * Verifica se existe usuário logado.
     */
    const clienteId =
      getCheckoutClientId();

    if (!clienteId) {

      showToast(
        'Faça login para continuar com a compra.',
        'error'
      );

      setTimeout(() => {

        window.location.href =
          'login.html';

      }, 700);

      return;

    }


    console.log(
      '[Infinity Parts] Cliente identificado no checkout:',
      clienteId
    );


    /*
     * Aguarda os produtos carregados.
     */
    if (
      window.productsReadyPromise
    ) {

      await window.productsReadyPromise;

    }


    renderCheckoutSummary();

    initMasks();

    initPaymentToggle();

    initPixCopy();

    initStepNavigation();

    goToStep(1);

  }
);