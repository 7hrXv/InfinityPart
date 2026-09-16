using InfinityPart.Application.DTOs.StatusPedidos;

namespace InfinityPart.Application.Interfaces;

public interface IStatusPedidoService
{
    StatusPedidoDto Criar(CriarStatusPedidoDto dto);

    IEnumerable<StatusPedidoDto> Listar();

    StatusPedidoDto? BuscarPorId(int id);

    StatusPedidoDto? BuscarPorNome(string nome);

    StatusPedidoDto? Atualizar(AtualizarStatusPedidoDto dto);

    bool Remover(int id);
}