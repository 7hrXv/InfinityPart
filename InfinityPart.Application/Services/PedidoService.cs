using InfinityPart.Application.DTOs.Pedidos;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Interfaces;
using InfinittyPart.Domain.Entidades;

namespace InfinityPart.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public PedidoDto Criar(CriarPedidoDto dto)
    {
        var pedido = new Pedido
        {
            DataPedido = DateTime.UtcNow,
            ValorTotal = dto.ValorTotal,
            ClienteId = dto.ClienteId,
            StatusPedidoId = dto.StatusPedidoId
        };

        _pedidoRepository.Adicionar(pedido);

        return MapearParaDto(pedido);
    }

    public IEnumerable<PedidoDto> Listar()
    {
        return _pedidoRepository
            .ObterTodos()
            .Select(MapearParaDto);
    }

    public PedidoDto? BuscarPorId(int id)
    {
        var pedido = _pedidoRepository.ObterPorId(id);

        if (pedido == null)
            return null;

        return MapearParaDto(pedido);
    }

    public IEnumerable<PedidoDto> BuscarPorClienteId(int clienteId)
    {
        return _pedidoRepository
            .ObterPorClienteId(clienteId)
            .Select(MapearParaDto);
    }

    public PedidoDto? Atualizar(AtualizarPedidoDto dto)
    {
        var pedido = _pedidoRepository.ObterPorId(dto.Id);

        if (pedido == null)
            return null;

        pedido.ClienteId = dto.ClienteId;
        pedido.StatusPedidoId = dto.StatusPedidoId;
        pedido.ValorTotal = dto.ValorTotal;

        _pedidoRepository.Atualizar(pedido);

        return MapearParaDto(pedido);
    }

    public bool Remover(int id)
    {
        var pedido = _pedidoRepository.ObterPorId(id);

        if (pedido == null)
            return false;

        _pedidoRepository.Remover(id);

        return true;
    }

    private static PedidoDto MapearParaDto(Pedido pedido)
    {
        return new PedidoDto
        {
            Id = pedido.Id,
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal,
            ClienteId = pedido.ClienteId,
            StatusPedidoId = pedido.StatusPedidoId
        };
    }
}