using InfinityPart.Application.DTOs.Pedidos;

namespace InfinityPart.Application.Interfaces;

public interface IPedidoService
{
    PedidoDto Criar(CriarPedidoDto dto);
    IEnumerable<PedidoDto> Listar();
    PedidoDto? BuscarPorId(int id);
    IEnumerable<PedidoDto> BuscarPorClienteId(string clienteId);
    PedidoDto? Atualizar(AtualizarPedidoDto dto);
}