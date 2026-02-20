using Microsoft.Extensions.Logging;
using tabApp.CrossPlatform.ViewModels.Main;
using tabApp.CrossPlatform.Views.Settings;

namespace tabApp.CrossPlatform
{
    /// <summary>
    /// MAUI application entry point and DI configuration.
    ///
    /// Service registration is delegated to <see cref="DIConfiguration"/>
    /// via the <c>ConfigureCoreServices()</c> extension method (TASK-3.8).
    /// </summary>
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
                })
                // TASK-3.8: Register all Core + Location + Notification services via DIConfiguration
                .ConfigureCoreServices();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register ViewModels
            RegisterViewModels(builder.Services);

            // Register Pages
            RegisterPages(builder.Services);

            return builder.Build();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<SimpleSettingsViewModel>();
            // TODO: Register all other ViewModels as they are migrated
        }

        private static void RegisterPages(IServiceCollection services)
        {
            services.AddTransient<SimpleSettingsPage>();
            // TODO: Register all other Pages as they are migrated
        }
    }
}
