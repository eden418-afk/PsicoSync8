using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using PsicoSync.Servicios;
using PsicoSync.Views;

namespace PsicoSync
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FontAwesome-Regular.otf", "FAR");
                    fonts.AddFont("FontAwesome-Solid.otf", "FAS");
                });

            builder.Services.AddTransient<NuevaCitaPage>();
            builder.Services.AddTransient<NuevoClientePage>();
            builder.Services.AddTransient<NuevoTipoCitaPage>();

            builder.Services.AddSingleton<ServicioCliente>();
            builder.Services.AddSingleton<ServicioTipoCita>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
