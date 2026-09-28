using InfinityPart.Application.DTOs.Categorias;

namespace InfinityPart.Application.Interfaces;

public interface ICategoriaService
{
    IEnumerable<CategoriaDto> Listar();

    CategoriaDto? BuscarPorId(int id);

    Task<CategoriaDto> CriarAsync(CriarCategoriaDto dto);

    CategoriaDto? Atualizar(AtualizarCategoriaDto dto);

    bool Remover(int id);
}