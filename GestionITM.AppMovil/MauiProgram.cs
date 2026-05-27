using GestionITM.AppMovil.Views;
using GestionITM.AppMovil.ViewModels;
using Microsoft.Extensions.Logging;

namespace GestionITM.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ===  INYECCIÓN DE DEPENDENCIAS ===
            // Usamos AddTransient para que cada vez  que entremos a la pantalla,
            // nazca una versión fresca y limpia de la vista y de su cerebro (ViewModel).
            builder.Services.AddTransient<ProfesoresPage>();
            builder.Services.AddTransient<ProfesoresViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<CursosPage>();
            builder.Services.AddTransient<CursosViewModel>();

            builder.Services.AddTransient<Services.AuthenticationHandler>();

            builder.Services.AddHttpClient<Services.ApiService>();

            builder.Services.AddHttpClient<Services.CursoService>(client => 
            {
                client.BaseAddress = new Uri("http://10.0.2.2:5000/api/");
            })
            .AddHttpMessageHandler<Services.AuthenticationHandler>();

            builder.Services.AddHttpClient<Services.MatriculaService>(client => 
            {
                client.BaseAddress = new Uri("http://10.0.2.2:5000/api/");
            })
            .AddHttpMessageHandler<Services.AuthenticationHandler>();

            return builder.Build();
        }
    }
}
