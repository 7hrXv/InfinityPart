using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityPart.Application.DTOs.Pedidos;

public class CriarPedidoDto
{
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Pendente";

    public int ApplicationUserId { get; set; }
}