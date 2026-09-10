using InfinityPart.Application.DTOs.Fabricantes;

namespace InfinityPart.Application.Interfaces;

public interface IFabricanteService
{
    FabricanteDto Criar(CriarFabricanteDto dto);

    IEnumerable<FabricanteDto> Listar();

    FabricanteDto? BuscarPorId(int id);

    FabricanteDto? Atualizar(AtualizarFabricanteDto dto);

    bool Remover(int id);
}