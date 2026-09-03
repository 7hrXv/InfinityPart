using InfinityPart.Application.DTOs.Pedidos;
using InfinityPart.Application.Interfaces;
using InfinityPart.Domain.Interfaces;
using InfinityPart.Entidades;

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
            ValorTotal = dto.ValorTotal,
            Status = dto.Status,
            ApplicationUserId = dto.ApplicationUserId
        };

        _pedidoRepository.Criar(pedido);

        return MapearParaDto(pedido);
    }

    public IEnumerable<PedidoDto> Listar()
    {
        var pedidos = _pedidoRepository.ObterTodos();

        return pedidos.Select(MapearParaDto);
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
        var pedidos = _pedidoRepository.ObterPorClienteId(clienteId);

        return pedidos.Select(MapearParaDto);
    }

    public PedidoDto? Atualizar(AtualizarPedidoDto dto)
    {
        var pedido = _pedidoRepository.ObterPorId(dto.Id);

        if (pedido == null)
            return null;

        pedido.ValorTotal = dto.ValorTotal;
        pedido.Status = dto.Status;
        pedido.ApplicationUserId = dto.ApplicationUserId;

        _pedidoRepository.Atualizar(pedido);

        return MapearParaDto(pedido);
    }

    private static PedidoDto MapearParaDto(Pedido pedido)
    {
        return new PedidoDto
        {
            Id = pedido.Id,
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            ApplicationUserId = pedido.ApplicationUserId
        };
    }
}