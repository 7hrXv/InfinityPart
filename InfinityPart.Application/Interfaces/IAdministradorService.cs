using InfinityPart.Application.DTOs.Administradores;

namespace InfinityPart.Application.Interfaces;

public interface IAdministradorService
{
    AdministradorDto Criar(CriarAdministradorDto dto);

    IEnumerable<AdministradorDto> Listar();

    AdministradorDto? BuscarPorId(int id);

    AdministradorDto? BuscarPorCpf(string cpf);

    AdministradorDto? Atualizar(AtualizarAdministradorDto dto);

    bool Remover(int id);
}