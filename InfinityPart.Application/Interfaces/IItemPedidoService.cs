using InfinityPart.Application.DTOs.ItensPedido;

namespace InfinityPart.Application.Interfaces;

public interface IItemPedidoService
{
    ItemPedidoDto Criar(CriarItemPedidoDto dto);

    IEnumerable<ItemPedidoDto> Listar();

    ItemPedidoDto? BuscarPorId(int id);

    IEnumerable<ItemPedidoDto> BuscarPorPedidoId(int pedidoId);

    IEnumerable<ItemPedidoDto> BuscarPorProdutoId(int produtoId);

    ItemPedidoDto? Atualizar(AtualizarItemPedidoDto dto);

    bool Remover(int id);
}