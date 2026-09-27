using InfinityPart.Application.DTOs.Pedidos;
using InfinityPart.Application.Exceptions;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class PedidoService : IPedidoService
{
    private const string StatusInicialPedido = "Pendente";

    private readonly IPedidoRepository _pedidoRepository;
    private readonly IStatusPedidoRepository _statusPedidoRepository;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IStatusPedidoRepository statusPedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _statusPedidoRepository = statusPedidoRepository;
    }

    public PedidoDto Criar(CriarPedidoDto dto)
    {
        if (dto == null)
        {
            throw new ValidacaoException(
                "Os dados do pedido são obrigatórios."
            );
        }

        if (dto.ClienteId <= 0)
        {
            throw new ValidacaoException(
                "O cliente do pedido é obrigatório."
            );
        }

        if (dto.ValorTotal <= 0)
        {
            throw new ValidacaoException(
                "O valor total do pedido deve ser maior que zero."
            );
        }

        var statusPendente =
            _statusPedidoRepository.ObterPorNome(
                StatusInicialPedido
            );

        if (statusPendente == null)
        {
            throw new RecursoNaoEncontradoException(
                $"O status '{StatusInicialPedido}' não está cadastrado."
            );
        }

        var pedido = new Pedido
        {
            DataPedido = DateTime.UtcNow,
            ValorTotal = dto.ValorTotal,
            ClienteId = dto.ClienteId,
            StatusPedidoId = statusPendente.Id
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
        var pedido =
            _pedidoRepository.ObterPorId(id);

        if (pedido == null)
            return null;

        return MapearParaDto(pedido);
    }

    public IEnumerable<PedidoDto> BuscarPorClienteId(
        int clienteId)
    {
        return _pedidoRepository
            .ObterPorClienteId(clienteId)
            .Select(MapearParaDto);
    }

    public PedidoDto? Atualizar(
        AtualizarPedidoDto dto)
    {
        if (dto == null)
        {
            throw new ValidacaoException(
                "Os dados do pedido são obrigatórios."
            );
        }

        var pedido =
            _pedidoRepository.ObterPorId(dto.Id);

        if (pedido == null)
            return null;

        if (dto.ClienteId <= 0)
        {
            throw new ValidacaoException(
                "O cliente do pedido é obrigatório."
            );
        }

        if (dto.StatusPedidoId <= 0)
        {
            throw new ValidacaoException(
                "O status do pedido é obrigatório."
            );
        }

        if (dto.ValorTotal <= 0)
        {
            throw new ValidacaoException(
                "O valor total do pedido deve ser maior que zero."
            );
        }

        var status =
            _statusPedidoRepository.ObterPorId(
                dto.StatusPedidoId
            );

        if (status == null)
        {
            throw new RecursoNaoEncontradoException(
                "O status informado não foi encontrado."
            );
        }

        pedido.ClienteId =
            dto.ClienteId;

        pedido.StatusPedidoId =
            status.Id;

        pedido.ValorTotal =
            dto.ValorTotal;

        _pedidoRepository.Atualizar(pedido);

        return MapearParaDto(pedido);
    }

    public bool Remover(int id)
    {
        var pedido =
            _pedidoRepository.ObterPorId(id);

        if (pedido == null)
            return false;

        _pedidoRepository.Remover(id);

        return true;
    }

    private static PedidoDto MapearParaDto(
        Pedido pedido)
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