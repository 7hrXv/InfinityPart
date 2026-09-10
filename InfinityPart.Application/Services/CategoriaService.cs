using InfinittyPart.Domain.Entidades;
using InfinityPart.Application.DTOs.Categorias;
using InfinityPart.Application.Interfaces;
using InfinityPart.Domain.Entidades;
using InfinityPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public CategoriaDto Criar(CriarCategoriaDto dto)
    {
        var categoria = new Categoria
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao
        };

        _categoriaRepository.Adicionar(categoria);

        return MapearParaDto(categoria);
    }

    public IEnumerable<CategoriaDto> Listar()
    {
        var categorias = _categoriaRepository.ObterTodas();

        return categorias.Select(MapearParaDto);
    }

    public CategoriaDto? BuscarPorId(int id)
    {
        var categoria = _categoriaRepository.ObterPorId(id);

        if (categoria == null)
            return null;

        return MapearParaDto(categoria);
    }

    public CategoriaDto? Atualizar(AtualizarCategoriaDto dto)
    {
        var categoria = _categoriaRepository.ObterPorId(dto.Id);

        if (categoria == null)
            return null;

        categoria.Nome = dto.Nome;
        categoria.Descricao = dto.Descricao;

        _categoriaRepository.Atualizar(categoria);

        return MapearParaDto(categoria);
    }

    public bool Remover(int id)
    {
        var categoria = _categoriaRepository.ObterPorId(id);

        if (categoria == null)
            return false;

        _categoriaRepository.Remover(id);

        return true;
    }

    private static CategoriaDto MapearParaDto(Categoria categoria)
    {
        return new CategoriaDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao
        };
    }
}