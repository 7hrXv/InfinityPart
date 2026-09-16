using InfinityPart.Application.DTOs.Administradores;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class AdministradorService : IAdministradorService
{
    private readonly IAdministradorRepository _administradorRepository;

    public AdministradorService(IAdministradorRepository administradorRepository)
    {
        _administradorRepository = administradorRepository;
    }

    public AdministradorDto Criar(CriarAdministradorDto dto)
    {
        var administrador = new Administrador
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            DataCadastro = DateTime.UtcNow
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
            DataCadastro = administrador.DataCadastro
        };
    }
}