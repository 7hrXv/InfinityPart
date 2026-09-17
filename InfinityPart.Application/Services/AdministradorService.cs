using InfinityPart.Application.DTOs.Administradores;
using InfinityPart.Application.Exceptions;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class AdministradorService : IAdministradorService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly ISenhaHasher _senhaHasher;

    public AdministradorService(
        IAdministradorRepository administradorRepository,
        ISenhaHasher senhaHasher)
    {
        _administradorRepository = administradorRepository;
        _senhaHasher = senhaHasher;
    }

    public AdministradorDto Criar(CriarAdministradorDto dto)
    {
        // A senha é validada e transformada em hash antes de qualquer persistência.
        AutenticacaoService.ValidarSenha(dto.Senha);

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ValidacaoException("O e-mail é obrigatório.");

        var emailNormalizado = dto.Email.Trim();

        if (_administradorRepository.ObterPorEmail(emailNormalizado) != null)
            throw new ValidacaoException("Já existe um administrador com este e-mail.");

        var administrador = new Administrador
        {
            Nome = dto.Nome,
            Email = emailNormalizado,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            DataCadastro = DateTime.UtcNow,
            Ativo = true,
            SenhaHash = _senhaHasher.GerarHash(dto.Senha)
        };

        _administradorRepository.Adicionar(administrador);

        return MapearParaDto(administrador);
    }

    public IEnumerable<AdministradorDto> Listar()
    {
        return _administradorRepository
            .ObterTodos()
            .Select(MapearParaDto);
    }

    public AdministradorDto? BuscarPorId(int id)
    {
        var administrador = _administradorRepository.ObterPorId(id);

        if (administrador == null)
            return null;

        return MapearParaDto(administrador);
    }

    public AdministradorDto? BuscarPorCpf(string cpf)
    {
        var administrador = _administradorRepository.ObterPorCpf(cpf);

        if (administrador == null)
            return null;

        return MapearParaDto(administrador);
    }

    public AdministradorDto? Atualizar(AtualizarAdministradorDto dto)
    {
        var administrador = _administradorRepository.ObterPorId(dto.Id);

        if (administrador == null)
            return null;

        administrador.Nome = dto.Nome;
        administrador.Email = dto.Email;
        administrador.Cpf = dto.Cpf;
        administrador.Telefone = dto.Telefone;
        administrador.Ativo = dto.Ativo;

        // SenhaHash não é tocado aqui: troca de senha passa por IAutenticacaoService.
        _administradorRepository.Atualizar(administrador);

        return MapearParaDto(administrador);
    }

    public bool Remover(int id)
    {
        var administrador = _administradorRepository.ObterPorId(id);

        if (administrador == null)
            return false;

        _administradorRepository.Remover(id);

        return true;
    }

    private static AdministradorDto MapearParaDto(Administrador administrador)
    {
        return new AdministradorDto
        {
            Id = administrador.Id,
            Nome = administrador.Nome,
            Email = administrador.Email,
            Cpf = administrador.Cpf,
            Telefone = administrador.Telefone,
            DataCadastro = administrador.DataCadastro,
            Ativo = administrador.Ativo,
            UltimoAcesso = administrador.UltimoAcesso,
            PossuiSenhaDefinida = administrador.PossuiSenhaDefinida
        };
    }
}
