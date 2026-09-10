using InfinittyPart.Domain.Entidades;
using InfinityPart.Application.DTOs.Fabricantes;
using InfinityPart.Application.Interfaces;
using InfinityPart.Domain.Interfaces;
using InfinityPart.Entidades;

namespace InfinityPart.Application.Services;

public class FabricanteService : IFabricanteService
{
    private readonly IFabricanteRepository _fabricanteRepository;

    public FabricanteService(IFabricanteRepository fabricanteRepository)
    {
        _fabricanteRepository = fabricanteRepository;
    }

    public FabricanteDto Criar(CriarFabricanteDto dto)
    {
        var fabricante = new Fabricante
        {
            Nome = dto.Nome,
            CNPJ = dto.CNPJ
        };

        _fabricanteRepository.Adicionar(fabricante);

        return MapearParaDto(fabricante);
    }

    public IEnumerable<FabricanteDto> Listar()
    {
        var fabricantes = _fabricanteRepository.ObterTodos();

        return fabricantes.Select(MapearParaDto);
    }

    public FabricanteDto? BuscarPorId(int id)
    {
        var fabricante = _fabricanteRepository.ObterPorId(id);

        if (fabricante == null)
            return null;

        return MapearParaDto(fabricante);
    }

    public FabricanteDto? Atualizar(AtualizarFabricanteDto dto)
    {
        var fabricante = _fabricanteRepository.ObterPorId(dto.Id);

        if (fabricante == null)
            return null;

        fabricante.Nome = dto.Nome;
        fabricante.CNPJ = dto.CNPJ;

        _fabricanteRepository.Atualizar(fabricante);

        return MapearParaDto(fabricante);
    }

    public bool Remover(int id)
    {
        var fabricante = _fabricanteRepository.ObterPorId(id);

        if (fabricante == null)
            return false;

        _fabricanteRepository.Remover(id);

        return true;
    }

    private static FabricanteDto MapearParaDto(Fabricante fabricante)
    {
        return new FabricanteDto
        {
            Id = fabricante.Id,
            Nome = fabricante.Nome,
            CNPJ = fabricante.CNPJ
        };
    }
}