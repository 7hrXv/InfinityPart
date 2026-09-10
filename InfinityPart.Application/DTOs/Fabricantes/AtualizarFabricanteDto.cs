namespace InfinityPart.Application.DTOs.Fabricantes;

public class AtualizarFabricanteDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? CNPJ { get; set; }
}