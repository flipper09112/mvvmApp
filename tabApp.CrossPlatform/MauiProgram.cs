using Microsoft.Extensions.Logging;
using tabApp.CrossPlatform.Services;
using tabApp.CrossPlatform.ViewModels.Main;
using tabApp.CrossPlatform.Views.Settings;

namespace tabApp.CrossPlatform
{
    /// <summary>
    /// MAUI application configuration and service registration.
    /// 
    /// MIGRATION NOTE:
    /// This replaces the MvvmCross App.cs and Setup.cs pattern.
    /// All dependency injection configuration is done here using the built-in
    /// Microsoft.Extensions.DependencyInjection container.
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
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register Services
            RegisterServices(builder.Services);

            // Register ViewModels
            RegisterViewModels(builder.Services);

            // Register Pages
            RegisterPages(builder.Services);

            return builder.Build();
        }

        /// <summary>
        /// Register application services (business logic, data access, etc.)
        /// </summary>
        private static void RegisterServices(IServiceCollection services)
        {
            // TODO: Register services from tabApp.Core
            // Example patterns:
            // services.AddSingleton<IFileService, FileService>();
            // services.AddSingleton<ISQLiteService, SQLiteService>();
            // services.AddTransient<IDialogService, DialogService>();
        }

        /// <summary>
        /// Register ViewModels.
        /// Typically use Transient lifetime so each page gets a fresh ViewModel instance.
        /// </summary>
        private static void RegisterViewModels(IServiceCollection services)
        {
            // POC: Register SimpleSettingsViewModel
            services.AddTransient<SimpleSettingsViewModel>();

            // TODO: Register all other ViewModels as they are migrated
        }

        /// <summary>
        /// Register Pages (Views).
        /// Typically use Transient lifetime so each navigation creates a new page instance.
        /// </summary>
        private static void RegisterPages(IServiceCollection services)
        {
            // POC: Register SimpleSettingsPage
            services.AddTransient<SimpleSettingsPage>();

            // TODO: Register all other Pages as they are migrated
        }
    }
}


