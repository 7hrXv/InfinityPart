using InfinityPart.Domain.Entidades;
using InfinityPart.Entidades;
using System.Collections.Generic;

namespace InfinityPart.Domain.Interfaces
{
    // Interface para gerenciar as Categorias de produtos da InfinityPart
    public interface ICategoriaRepository
    {
        // Cadastra uma nova categoria no sistema
        void Adicionar(Categoria categoria);

        // Atualiza o nome ou descrição de uma categoria
        void Atualizar(Categoria categoria);

        // Remove uma categoria pelo ID
        void Remover(int id);

        // Busca uma categoria específica pelo ID
        Categoria ObterPorId(int id);

        // Lista todas as categorias cadastradas
        List<Categoria> ObterTodas();
    }
}