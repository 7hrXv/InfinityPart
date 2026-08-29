using System.ComponentModel.DataAnnotations; // Usado para definir regras de validação (como tamanho máximo de texto)

namespace InfinityPart.Domain.Entities
{
    // Esta classe representa as categorias/seções da loja (ex: Placas de Vídeo, Processadores)
    public class Categoria
    {
        // Número de identificação único da categoria no banco de dados
        public int Id { get; set; }

        // Nome da categoria (ex: "Processadores"). É obrigatório e aceita no máximo 100 caracteres
        [MaxLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
        public required string Nome { get; set; }

        // Texto explicativo opcional sobre a categoria
        public string? Descricao { get; set; }

        // Indica se a categoria está visível na loja (true = sim, false = desativada)
        public bool Ativo { get; set; } = true;

        // Data e hora em que a categoria foi cadastrada no sistema
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        // Data e hora da última vez que a categoria foi editada (null se nunca foi alterada)
        public DateTime? DataAtualizacao { get; set; }

        // Data e hora em que a categoria foi desativada (null se ainda estiver ativa)
        public DateTime? DataExclusao { get; set; }

        // Lista de peças que pertencem a esta categoria (uma categoria pode ter várias peças)
        public ICollection<Peca> Pecas { get; set; } = new List<Peca>();
    }
}