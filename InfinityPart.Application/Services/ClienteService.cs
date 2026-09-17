using InfinityPart.Application.DTOs.Clientes;
using InfinityPart.Application.Exceptions;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ISenhaHasher _senhaHasher;

    public ClienteService(
        IClienteRepository clienteRepository,
        ISenhaHasher senhaHasher)
    {
        _clienteRepository = clienteRepository;
        _senhaHasher = senhaHasher;
    }

    public Task<ClienteDto> CriarAsync(CriarClienteDto dto)
    {
        if (dto == null)
            throw new ValidacaoException("Dados do cliente são obrigatórios.");

        if (string.IsNullOrWhiteSpace(dto.Senha))
            throw new ValidacaoException("A senha é obrigatória.");

        AutenticacaoService.ValidarSenha(dto.Senha);

        var clienteExistente = _clienteRepository.ObterPorCpf(dto.Cpf);

        if (clienteExistente != null)
            throw new ValidacaoException("Já existe um cliente cadastrado com este CPF.");

        var cliente = new Cliente
        {
            Nome = dto.Nome,
            Cpf = dto.Cpf,
            Email = dto.Email,
            Telefone = dto.Telefone,

            // A senha nunca é salva em texto puro.
            SenhaHash = _senhaHasher.GerarHash(dto.Senha),

            Cep = dto.Cep,
            Endereco = dto.Endereco,
            Numero = dto.Numero,
            Cidade = dto.Cidade,
            Estado = dto.Estado
        };

        _clienteRepository.Adicionar(cliente);

        return Task.FromResult(MapearParaDto(cliente));
    }

    public IEnumerable<ClienteDto> Listar()
    {
        return _clienteRepository
            .ObterTodos()
            .Select(MapearParaDto);
    }

    public ClienteDto? BuscarPorId(int id)
    {
        var cliente = _clienteRepository.ObterPorId(id);

        if (cliente == null)
            return null;

        return MapearParaDto(cliente);
    }

    public ClienteDto? Atualizar(AtualizarClienteDto dto)
    {
        var cliente = _clienteRepository.ObterPorId(dto.Id);

        if (cliente == null)
            return null;

        cliente.Nome = dto.Nome;
        cliente.Cpf = dto.Cpf;
        cliente.Email = dto.Email;
        cliente.Telefone = dto.Telefone;
        cliente.Cep = dto.Cep;
        cliente.Endereco = dto.Endereco;
        cliente.Numero = dto.Numero;
        cliente.Cidade = dto.Cidade;
        cliente.Estado = dto.Estado;

        _clienteRepository.Atualizar(cliente);

        return MapearParaDto(cliente);
    }

    public bool Remover(int id)
    {
        var cliente = _clienteRepository.ObterPorId(id);

        if (cliente == null)
            return false;

        _clienteRepository.Remover(id);

        return true;
    }

    private static ClienteDto MapearParaDto(Cliente cliente)
    {
        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Cpf = cliente.Cpf,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            Cep = cliente.Cep,
            Endereco = cliente.Endereco,
            Numero = cliente.Numero,
            Cidade = cliente.Cidade,
            Estado = cliente.Estado
        };
    }
}