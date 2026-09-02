using InfinittyPart.Domain.Entidades;
using Infinitypart.Domain.Excecoes;
using InfinityPart.Domain.Entidades;
using InfinityPart.Domain.Excecoes;
using System;

namespace InfinityPart.Domain.Services
{
    public class PedidoDomainService
    {
        // Regra 1: Calcular desconto para pagamento via PIX
        public decimal CalcularDescontoPix(decimal valorTotal)
        {
            if (valorTotal <= 0)
            {
                throw new RegraNegocioException("O valor do pedido deve ser maior que zero.");
            }

            decimal desconto = valorTotal * 0.10m;
            return valorTotal - desconto;
        }

        // Regra 2: Validar estoque antes de adicionar ao carrinho
        public void ValidarEstoqueParaVenda(Produto produto, int quantidadeDesejada)
        {
            if (quantidadeDesejada <= 0)
            {
                throw new RegraNegocioException("Selecione ao menos 1 unidade para comprar.");
            }

            if (produto.QuantidadeEstoque < quantidadeDesejada)
            {
                throw new EstoqueInsuficienteException(
                    $"O produto '{produto.Nome}' não possui estoque suficiente. Disponível: {produto.QuantidadeEstoque}"
                );
            }
        }
    }
}