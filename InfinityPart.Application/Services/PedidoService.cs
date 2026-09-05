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
            DataPedido = DateTime.UtcNow,
            ValorTotal = dto.ValorTotal,
            Status = dto.Status,
            ApplicationUserId = dto.ApplicationUserId
        };

        _pedidoRepository.Criar(pedido);

        return new PedidoDto
        {
            Id = pedido.Id,
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            ApplicationUserId = pedido.ApplicationUserId
        };
    }

    public IEnumerable<PedidoDto> Listar()
    {
        var pedidos = _pedidoRepository.ObterTodos();

        return pedidos.Select(pedido => new PedidoDto
        {
            Id = pedido.Id,
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            ApplicationUserId = pedido.ApplicationUserId
        });
    }

    public PedidoDto? BuscarPorId(int id)
    {
        var pedido = _pedidoRepository.ObterPorId(id);

        if (pedido == null)
            return null;

        return new PedidoDto
        {
            Id = pedido.Id,
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            ApplicationUserId = pedido.ApplicationUserId
        };
    }

    public IEnumerable<PedidoDto> BuscarPorClienteId(string clienteId)
    {
        var pedidos = _pedidoRepository.ObterPorClienteId(clienteId);

        return pedidos.Select(pedido => new PedidoDto
        {
            Id = pedido.Id,
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            ApplicationUserId = pedido.ApplicationUserId
        });
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