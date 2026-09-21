using InfinityPart.Application.DTOs.Administradores;
using InfinityPart.Application.DTOs.Clientes;

namespace InfinityPart.Application.DTOs.Autenticacao;

public class LoginResultadoDto
{
    public bool Autenticado { get; set; }

    public string Mensagem { get; set; } = string.Empty;

    public bool SenhaNaoDefinida { get; set; }

    public AdministradorDto? Administrador { get; set; }

    public ClienteDto? Cliente { get; set; }

    public static LoginResultadoDto Falha(string mensagem) => new()
    {
        Autenticado = false,
        Mensagem = mensagem
    };

    public static LoginResultadoDto Sucesso(AdministradorDto administrador) => new()
    {
        Autenticado = true,
        Mensagem = "Administrador autenticado com sucesso.",
        Administrador = administrador
    };

    public static LoginResultadoDto Sucesso(ClienteDto cliente) => new()
    {
        Autenticado = true,
        Mensagem = "Cliente autenticado com sucesso.",
        Cliente = cliente
    };
}