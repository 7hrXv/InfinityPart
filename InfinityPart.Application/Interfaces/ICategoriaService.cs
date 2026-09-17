using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityPart.Application.DTOs.Categorias;

namespace InfinityPart.Application.Interfaces;

public interface ICategoriaService
{
    CategoriaDto Criar(CriarCategoriaDto dto);
    IEnumerable<CategoriaDto> Listar();
    CategoriaDto? BuscarPorId(int id);
    CategoriaDto? Atualizar(AtualizarCategoriaDto dto);
    bool Remover(int id);
}