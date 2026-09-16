using InfinityPart.Application.DTOs.Marcas;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class MarcaService : IMarcaService
{
    private readonly IMarcaRepository _marcaRepository;

    public MarcaService(IMarcaRepository marcaRepository)
    {
        _marcaRepository = marcaRepository;
    }

    public MarcaDto Criar(CriarMarcaDto dto)
    {
        var marca = new Marca
        {
            Nome = dto.Nome,
            Cnpj = dto.Cnpj
        };

        _marcaRepository.Adicionar(marca);

        return MapearParaDto(marca);
    }

    public IEnumerable<MarcaDto> Listar()
    {
        return _marcaRepository
            .ObterTodos()
            .Select(MapearParaDto);
    }

    public MarcaDto? BuscarPorId(int id)
    {
        var marca = _marcaRepository.ObterPorId(id);

        if (marca == null)
            return null;

        return MapearParaDto(marca);
    }

    public MarcaDto? BuscarPorNome(string nome)
    {
        var marca = _marcaRepository.ObterPorNome(nome);

        if (marca == null)
            return null;

        return MapearParaDto(marca);
    }

    public MarcaDto? Atualizar(AtualizarMarcaDto dto)
    {
        var marca = _marcaRepository.ObterPorId(dto.Id);

        if (marca == null)
            return null;

        marca.Nome = dto.Nome;
        marca.Cnpj = dto.Cnpj;

        _marcaRepository.Atualizar(marca);

        return MapearParaDto(marca);
    }

    public bool Remover(int id)
    {
        var marca = _marcaRepository.ObterPorId(id);

        if (marca == null)
            return false;

        _marcaRepository.Remover(id);

        return true;
    }

    private static MarcaDto MapearParaDto(Marca marca)
    {
        return new MarcaDto
        {
            Id = marca.Id,
            Nome = marca.Nome,
            Cnpj = marca.Cnpj
        };
    }
}