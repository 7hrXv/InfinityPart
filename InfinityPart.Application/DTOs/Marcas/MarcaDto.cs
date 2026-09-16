namespace InfinityPart.Application.DTOs.Marcas;

public class MarcaDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Cnpj { get; set; }
}