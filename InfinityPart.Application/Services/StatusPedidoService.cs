using InfinityPart.Application.DTOs.StatusPedidos;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class StatusPedidoService : IStatusPedidoService
{
    private readonly IStatusPedidoRepository _statusPedidoRepository;

    public StatusPedidoService(IStatusPedidoRepository statusPedidoRepository)
    {
        _statusPedidoRepository = statusPedidoRepository;
    }

    public StatusPedidoDto Criar(CriarStatusPedidoDto dto)
    {
        var status = new StatusPedido
        {
            Nome = dto.Nome
        };

        _statusPedidoRepository.Adicionar(status);

        return MapearParaDto(status);
    }

    public IEnumerable<StatusPedidoDto> Listar()
    {
        return _statusPedidoRepository
            .ObterTodos()
            .Select(MapearParaDto);
    }

    public StatusPedidoDto? BuscarPorId(int id)
    {
        var status = _statusPedidoRepository.ObterPorId(id);

        if (status == null)
            return null;

        return MapearParaDto(status);
    }

    public StatusPedidoDto? BuscarPorNome(string nome)
    {
        var status = _statusPedidoRepository.ObterPorNome(nome);

        if (status == null)
            return null;

        return MapearParaDto(status);
    }

    public StatusPedidoDto? Atualizar(AtualizarStatusPedidoDto dto)
    {
        var status = _statusPedidoRepository.ObterPorId(dto.Id);

        if (status == null)
            return null;

        status.Nome = dto.Nome;

        _statusPedidoRepository.Atualizar(status);

        return MapearParaDto(status);
    }

    public bool Remover(int id)
    {
        var status = _statusPedidoRepository.ObterPorId(id);

        if (status == null)
            return false;

        _statusPedidoRepository.Remover(id);

        return true;
    }

    private static StatusPedidoDto MapearParaDto(StatusPedido status)
    {
        return new StatusPedidoDto
        {
            Id = status.Id,
            Nome = status.Nome
        };
    }
}