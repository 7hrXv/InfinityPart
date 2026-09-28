namespace InfinityPart.Application.DTOs.Categorias;

public class AtualizarCategoriaDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }
}