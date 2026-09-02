using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InfinityPart.Application.DTOs.Clientes;

namespace InfinityPart.Application.Interfaces;

public interface IClienteService
{
    Task<ClienteDto> CriarAsync(CriarClienteDto dto);

    IEnumerable<ClienteDto> Listar();

    ClienteDto? BuscarPorId(int id);

    ClienteDto? Atualizar(AtualizarClienteDto dto);

    bool Remover(int id);
}