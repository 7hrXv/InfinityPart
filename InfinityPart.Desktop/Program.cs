namespace InfinityPart.Desktop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // A aplicação inicia pelo FrmLogin. O FrmPrincipal só é aberto
            // depois que a API confirma as credenciais (POST api/autenticacao/login)
            // e a SessaoAtual é preenchida com o administrador autenticado.
            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;
            }

            Application.Run(new FrmPrincipal());
        }
    }
}
