using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityPart.Application.DTOs.Pedidos;

public class AtualizarPedidoDto
{
    public int Id { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;

    public int ApplicationUserId { get; set; }
}