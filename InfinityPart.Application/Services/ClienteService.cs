using InfinityPart.Application.DTOs.Clientes;
using InfinityPart.Application.Exceptions;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    // =========================================================
    // CRIAR CLIENTE
    // =========================================================

    public Task<ClienteDto> CriarAsync(CriarClienteDto dto)
    {
        if (dto == null)
            throw new ValidacaoException(
                "Dados do cliente são obrigatórios."
            );

        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ValidacaoException(
                "O nome é obrigatório."
            );

        // =====================================================
        // CPF
        // =====================================================

        var cpf = SomenteNumeros(dto.Cpf);

        if (!CpfValido(cpf))
            throw new ValidacaoException(
                "O CPF informado é inválido."
            );

        var clienteExistente =
            _clienteRepository.ObterPorCpf(cpf);

        if (clienteExistente != null)
        {
            throw new ValidacaoException(
                "Já existe um cliente cadastrado com este CPF."
            );
        }

        // =====================================================
        // DATA DE NASCIMENTO
        // =====================================================

        ValidarDataNascimento(dto.DataNascimento);

        // =====================================================
        // SENHA
        // =====================================================

        if (string.IsNullOrWhiteSpace(dto.Senha))
            throw new ValidacaoException(
                "A senha é obrigatória."
            );

        AutenticacaoService.ValidarSenha(dto.Senha);

        // =====================================================
        // CLIENTE
        // =====================================================

        var cliente = new Cliente
        {
            Nome = dto.Nome.Trim(),

            Cpf = cpf,

            Email = dto.Email.Trim(),

            Telefone = SomenteNumeros(dto.Telefone),

            SenhaHash = _senhaHasher.GerarHash(dto.Senha),

            DataNascimento = dto.DataNascimento,

            Cep = SomenteNumeros(dto.Cep),

            Endereco = dto.Endereco.Trim(),

            Numero = dto.Numero.Trim(),

            Complemento = dto.Complemento.Trim(),

            Bairro = dto.Bairro.Trim(),

            Cidade = dto.Cidade.Trim(),

            Estado = dto.Estado.Trim().ToUpper()
        };

        _clienteRepository.Adicionar(cliente);

        return Task.FromResult(
            MapearParaDto(cliente)
        );
    }

    // =========================================================
    // LISTAR CLIENTES
    // =========================================================

    public IEnumerable<ClienteDto> Listar()
    {
        return _clienteRepository
            .ObterTodos()
            .Select(MapearParaDto);
    }

    // =========================================================
    // BUSCAR POR ID
    // =========================================================

    public ClienteDto? BuscarPorId(int id)
    {
        var cliente =
            _clienteRepository.ObterPorId(id);

        if (cliente == null)
            return null;

        return MapearParaDto(cliente);
    }

    // =========================================================
    // ATUALIZAR CLIENTE
    // =========================================================

    public ClienteDto? Atualizar(
        AtualizarClienteDto dto)
    {
        if (dto == null)
            throw new ValidacaoException(
                "Dados do cliente são obrigatórios."
            );

        var cliente =
            _clienteRepository.ObterPorId(dto.Id);

        if (cliente == null)
            return null;

        // =====================================================
        // CPF
        // =====================================================

        var cpf = SomenteNumeros(dto.Cpf);

        if (!CpfValido(cpf))
            throw new ValidacaoException(
                "O CPF informado é inválido."
            );

        var clienteComMesmoCpf =
            _clienteRepository.ObterPorCpf(cpf);

        if (
            clienteComMesmoCpf != null &&
            clienteComMesmoCpf.Id != cliente.Id
        )
        {
            throw new ValidacaoException(
                "Já existe outro cliente cadastrado com este CPF."
            );
        }

        // =====================================================
        // DATA DE NASCIMENTO
        // =====================================================

        ValidarDataNascimento(
            dto.DataNascimento
        );

        // =====================================================
        // ATUALIZAÇÃO
        // =====================================================

        cliente.Nome = dto.Nome.Trim();

        cliente.Cpf = cpf;

        cliente.Email = dto.Email.Trim();

        cliente.Telefone =
            SomenteNumeros(dto.Telefone);

        cliente.DataNascimento =
            dto.DataNascimento;

        cliente.Cep =
            SomenteNumeros(dto.Cep);

        cliente.Endereco =
            dto.Endereco.Trim();

        cliente.Numero =
            dto.Numero.Trim();

        cliente.Complemento =
            dto.Complemento.Trim();

        cliente.Bairro =
            dto.Bairro.Trim();

        cliente.Cidade =
            dto.Cidade.Trim();

        cliente.Estado =
            dto.Estado.Trim().ToUpper();

        _clienteRepository.Atualizar(cliente);

        return MapearParaDto(cliente);
    }

    // =========================================================
    // REMOVER CLIENTE
    // =========================================================

    public bool Remover(int id)
    {
        var cliente =
            _clienteRepository.ObterPorId(id);

        if (cliente == null)
            return false;

        try
        {
            _clienteRepository.Remover(id);
        }
        catch (DbUpdateException)
        {
            throw new ValidacaoException(
                "Não é possível excluir este cliente porque ele possui pedidos cadastrados."
            );
        }

        return true;
    }

    // =========================================================
    // MAPEAR PARA DTO
    // =========================================================

    private static ClienteDto MapearParaDto(
        Cliente cliente)
    {
        return new ClienteDto
        {
            Id = cliente.Id,

            Nome = cliente.Nome,

            Cpf = cliente.Cpf,

            Email = cliente.Email,

            Telefone = cliente.Telefone,

            DataNascimento =
                cliente.DataNascimento,

            Cep = cliente.Cep,

            Endereco = cliente.Endereco,

            Numero = cliente.Numero,

            Complemento =
                cliente.Complemento,

            Bairro =
                cliente.Bairro,

            Cidade = cliente.Cidade,

            Estado = cliente.Estado
        };
    }

    // =========================================================
    // SOMENTE NÚMEROS
    // =========================================================

    private static string SomenteNumeros(
        string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        return new string(
            valor
                .Where(char.IsDigit)
                .ToArray()
        );
    }

    // =========================================================
    // VALIDAÇÃO REAL DO CPF
    // =========================================================

    private static bool CpfValido(string cpf)
    {
        cpf = SomenteNumeros(cpf);

        if (cpf.Length != 11)
            return false;

        if (cpf.All(c => c == cpf[0]))
            return false;

        // Primeiro dígito
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma +=
                (cpf[i] - '0') *
                (10 - i);
        }

        int resto = soma % 11;

        int primeiroDigito =
            resto < 2
                ? 0
                : 11 - resto;

        if (
            primeiroDigito !=
            cpf[9] - '0'
        )
        {
            return false;
        }

        // Segundo dígito
        soma = 0;

        for (int i = 0; i < 10; i++)
        {
            soma +=
                (cpf[i] - '0') *
                (11 - i);
        }

        resto = soma % 11;

        int segundoDigito =
            resto < 2
                ? 0
                : 11 - resto;

        if (
            segundoDigito !=
            cpf[10] - '0'
        )
        {
            return false;
        }

        return true;
    }

    // =========================================================
    // VALIDAÇÃO DA DATA DE NASCIMENTO
    // =========================================================

    private static void ValidarDataNascimento(
        DateTime dataNascimento)
    {
        if (dataNascimento == default)
        {
            throw new ValidacaoException(
                "A data de nascimento é obrigatória."
            );
        }

        var hoje = DateTime.Today;

        if (dataNascimento > hoje)
        {
            throw new ValidacaoException(
                "A data de nascimento não pode estar no futuro."
            );
        }

        var idade =
            hoje.Year - dataNascimento.Year;

        if (
            dataNascimento.Date >
            hoje.AddYears(-idade)
        )
        {
            idade--;
        }

        if (idade < 13)
        {
            throw new ValidacaoException(
                "O cliente deve ter pelo menos 13 anos."
            );
        }

        if (idade > 120)
        {
            throw new ValidacaoException(
                "Informe uma data de nascimento válida."
            );
        }
    }
}