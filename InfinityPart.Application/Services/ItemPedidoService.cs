using InfinityPart.Application.DTOs.ItensPedido;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class ItemPedidoService : IItemPedidoService
{
    private readonly IItemPedidoRepository _itemPedidoRepository;

    public ItemPedidoService(IItemPedidoRepository itemPedidoRepository)
    {
        _itemPedidoRepository = itemPedidoRepository;
    }

    public ItemPedidoDto Criar(CriarItemPedidoDto dto)
    {
        var item = new ItemPedido
        {
            PedidoId = dto.PedidoId,
            ProdutoId = dto.ProdutoId,
            Quantidade = dto.Quantidade,
            PrecoUnitario = dto.PrecoUnitario
        };

        _itemPedidoRepository.Adicionar(item);

        return MapearParaDto(item);
    }

    public IEnumerable<ItemPedidoDto> Listar()
    {
        return _itemPedidoRepository
            .ObterTodos()
            .Select(MapearParaDto);
    }

    public ItemPedidoDto? BuscarPorId(int id)
    {
        var item = _itemPedidoRepository.ObterPorId(id);

        if (item == null)
            return null;

        return MapearParaDto(item);
    }

    public IEnumerable<ItemPedidoDto> BuscarPorPedidoId(int pedidoId)
    {
        return _itemPedidoRepository
            .ObterPorPedidoId(pedidoId)
            .Select(MapearParaDto);
    }

    public IEnumerable<ItemPedidoDto> BuscarPorProdutoId(int produtoId)
    {
        return _itemPedidoRepository
            .ObterPorProdutoId(produtoId)
            .Select(MapearParaDto);
    }

    public ItemPedidoDto? Atualizar(AtualizarItemPedidoDto dto)
    {
        var item = _itemPedidoRepository.ObterPorId(dto.Id);

        if (item == null)
            return null;

        item.PedidoId = dto.PedidoId;
        item.ProdutoId = dto.ProdutoId;
        item.Quantidade = dto.Quantidade;
        item.PrecoUnitario = dto.PrecoUnitario;

        _itemPedidoRepository.Atualizar(item);

        return MapearParaDto(item);
    }

    public bool Remover(int id)
    {
        var item = _itemPedidoRepository.ObterPorId(id);

        if (item == null)
            return false;

        _itemPedidoRepository.Remover(id);

        return true;
    }

    private static ItemPedidoDto MapearParaDto(ItemPedido item)
    {
        return new ItemPedidoDto
        {
            Id = item.Id,
            PedidoId = item.PedidoId,
            ProdutoId = item.ProdutoId,
            Quantidade = item.Quantidade,
            PrecoUnitario = item.PrecoUnitario,
            Subtotal = item.Subtotal
        };
    }
}