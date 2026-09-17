using InfinityPart.Desktop.Models;

namespace InfinityPart.Desktop.Services
{
    /// <summary>
    /// Guarda em memória o administrador autenticado na sessão atual do app desktop.
    /// Preenchido pelo FrmLogin (etapa 2) e lido pelo FrmPrincipal (cabeçalho).
    /// </summary>
    public static class SessaoAtual
    {
        public static AdministradorModel? Administrador { get; set; }

        public static string NomeExibicao =>
            Administrador?.Nome is { Length: > 0 } nome ? nome : "Administrador";

        public static void Encerrar()
        {
            Administrador = null;
        }
    }
}
