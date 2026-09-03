using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityPart.Application.DTOs.Produtos;

public class CriarProdutoDto
{
    public string Nome { get; set; } = string.Empty;
    public string CodigoPeca { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
    public int CategoriaId { get; set; }
    public int FabricanteId { get; set; }
}
