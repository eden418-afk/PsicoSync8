using PsicoSync.Views;

namespace PsicoSync
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(NuevaCitaPage), typeof(NuevaCitaPage));
            Routing.RegisterRoute(nameof(NuevoClientePage), typeof(NuevoClientePage));
            Routing.RegisterRoute(nameof(NuevoTipoCitaPage), typeof(NuevoTipoCitaPage));
            Routing.RegisterRoute(nameof(ClientesPage), typeof(ClientesPage));
            Routing.RegisterRoute(nameof(TipoCitasPage), typeof(TipoCitasPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(HistorialCitasPage), typeof(HistorialCitasPage));
        }
    }
}
