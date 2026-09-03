using System.Collections.Generic;
using System.Threading.Tasks;
using InfinittyPart.Domain.Entidades; // Corrigido para dois 't's

namespace InfinittyPart.Domain.Interfaces // Ajustado também para o padrão do projeto (dois 't's)
{
    // Interface para gerenciar o catálogo de peças/produtos da InfinityPart
    public interface IProdutoRepository
    {
        // Cadastra um novo produto/peça no estoque
        Task AdicionarAsync(Produto produto);

        // Atualiza o preço, estoque ou detalhes de um produto
        void Atualizar(Produto produto);

        // Remove um produto do sistema pelo ID
        void Remover(int id);

        // Busca um produto específico pelo ID
        Produto ObterPorId(int id);

        // Lista todos os produtos cadastrados
        List<Produto> ObterTodos();

        // Filtra os produtos por categoria (ex: listar só Placas de Vídeo)
        List<Produto> ObterPorCategoria(int categoriaId);

        // Busca produtos pelo nome (útil para a barra de pesquisa do site)
        List<Produto> ObterPorNome(string nome);
        Task<IEnumerable<object>> ListarTodosAsync();
        Task BuscarPorIdAsync(int id);
    }
}