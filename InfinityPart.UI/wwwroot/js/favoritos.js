/* ============================================================
   INFINITY PARTS — favoritos.js
   Página de favoritos
   ============================================================ */


document.addEventListener(
    'DOMContentLoaded',
    async function() {

        const favoritesGrid =
            document.getElementById(
                'favorites-grid'
            );


        if (!favoritesGrid) {
            return;
        }


        /*
         * Aguarda os produtos reais da API.
         */

        try {

            await window.productsReadyPromise;


            /*
             * Lê os favoritos salvos.
             */

            const favoriteIds =
                getFavorites();


            /*
             * Atualiza o contador.
             */

            updateFavCount();


            /*
             * Nenhum favorito salvo.
             */

            if (
                !Array.isArray(favoriteIds) ||
                favoriteIds.length === 0
            ) {

                favoritesGrid.innerHTML = `

                    <div
                        style="
                            grid-column: 1 / -1;
                            text-align: center;
                            padding: 60px 20px;
                        "
                    >

                        <i
                            class="bi bi-heart"
                            style="
                                font-size: 48px;
                                opacity: .5;
                                display: block;
                                margin-bottom: 16px;
                            "
                        ></i>

                        <h2>
                            Você ainda não tem favoritos
                        </h2>

                        <p
                            style="
                                margin: 10px 0 24px;
                                color: var(--c-gray);
                            "
                        >
                            Salve produtos que você gostou
                            para encontrá-los aqui.
                        </p>

                        <a
                            href="produtos.html"
                            class="btn btn-primary"
                        >
                            VER PRODUTOS
                        </a>

                    </div>

                `;

                return;
            }


            /*
             * Procura os favoritos dentro dos
             * produtos carregados da API.
             */

            const favoriteProducts =
                PRODUCTS.filter(
                    function(product) {

                        return favoriteIds.includes(
                            Number(product.id)
                        );

                    }
                );


            /*
             * Nenhum dos IDs salvos existe mais
             * nos produtos retornados pela API.
             */

            if (
                favoriteProducts.length === 0
            ) {

                favoritesGrid.innerHTML = `

                    <div
                        style="
                            grid-column: 1 / -1;
                            text-align: center;
                            padding: 60px 20px;
                        "
                    >

                        <i
                            class="bi bi-heart"
                            style="
                                font-size: 48px;
                                opacity: .5;
                                display: block;
                                margin-bottom: 16px;
                            "
                        ></i>

                        <h2>
                            Nenhum favorito encontrado
                        </h2>

                        <p
                            style="
                                margin: 10px 0 24px;
                                color: var(--c-gray);
                            "
                        >
                            Os produtos salvos não estão
                            disponíveis no catálogo atual.
                        </p>

                        <a
                            href="produtos.html"
                            class="btn btn-primary"
                        >
                            VER PRODUTOS
                        </a>

                    </div>

                `;

                return;
            }


            /*
             * Renderiza usando o mesmo card
             * utilizado pelo restante do site.
             */

            favoritesGrid.innerHTML =
                favoriteProducts
                    .map(
                        function(product) {

                            return renderProductCard(
                                product
                            );

                        }
                    )
                    .join('');


        } catch (error) {

            console.error(
                'Erro ao carregar favoritos:',
                error
            );


            favoritesGrid.innerHTML = `

                <div
                    style="
                        grid-column: 1 / -1;
                        text-align: center;
                        padding: 60px 20px;
                    "
                >

                    <i
                        class="bi bi-exclamation-triangle"
                        style="
                            font-size: 48px;
                            opacity: .6;
                            display: block;
                            margin-bottom: 16px;
                        "
                    ></i>

                    <h2>
                        Não foi possível carregar os favoritos
                    </h2>

                    <p
                        style="
                            margin-top: 10px;
                            color: var(--c-gray);
                        "
                    >
                        Verifique se a API está funcionando
                        e tente novamente.
                    </p>

                </div>

            `;

        }

    }
);