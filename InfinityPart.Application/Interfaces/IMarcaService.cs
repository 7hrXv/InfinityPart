using InfinityPart.Application.DTOs.Marcas;

namespace InfinityPart.Application.Interfaces;

public interface IMarcaService
{
    MarcaDto Criar(CriarMarcaDto dto);

    IEnumerable<MarcaDto> Listar();

    MarcaDto? BuscarPorId(int id);

    MarcaDto? BuscarPorNome(string nome);

    MarcaDto? Atualizar(AtualizarMarcaDto dto);

    bool Remover(int id);
}