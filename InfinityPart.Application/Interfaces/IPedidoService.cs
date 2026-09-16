using InfinityPart.Application.DTOs.Pedidos;

namespace InfinityPart.Application.Interfaces;

public interface IPedidoService
{
    PedidoDto Criar(CriarPedidoDto dto);

    IEnumerable<PedidoDto> Listar();

    PedidoDto? BuscarPorId(int id);

    IEnumerable<PedidoDto> BuscarPorClienteId(int clienteId);

    PedidoDto? Atualizar(AtualizarPedidoDto dto);

    bool Remover(int id);
}