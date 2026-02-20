using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.Deliverys;
using tabApp.Core.Services.Implementations.Notifications;
using tabApp.Core.Services.Implementations.Orders;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Deliverys;
using tabApp.Core.Services.Interfaces.Notifications;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.CrossPlatform.Services.Implementations.Location;
using tabApp.CrossPlatform.Services.Implementations.Notifications;
using tabApp.CrossPlatform.Services.Interfaces.Location;
using tabApp.CrossPlatform.Services.Interfaces.Notifications;

namespace tabApp.CrossPlatform;

/// <summary>
/// MAUI Dependency Injection configuration — TASK-3.8 POC.
///
/// Replaces the MvvmCross <c>Mvx.LazyConstructAndRegisterSingleton</c> pattern from
/// the legacy <c>Setup.cs</c> / <c>App.cs</c> files with standard
/// <c>Microsoft.Extensions.DependencyInjection</c> registration.
///
/// POC audit result (2026-02-20):
///   Zero active <c>Mvx.Resolve</c> calls found in tabApp.CrossPlatform or tabApp.Core.
///   All legacy MvvmCross code is guarded by <c>#if FALSE</c> in Setup.cs.
///   Constructor injection is already the pattern used throughout the CrossPlatform
///   service implementations — this file formalises the registration so the MAUI
///   DI container can resolve the full dependency graph at runtime.
///
/// Dependency graph (bottom-up registration order):
///
///   IClientsManagerService          ← no deps (parameterless ctor)
///   IDeliverysManagerService        ← no deps
///   IGlobalOrdersPastManagerService ← no deps
///   IProductsManagerService         ← IClientsManagerService
///   IOrdersManagerService           ← IProductsManagerService, IClientsManagerService
///   INotificationsManagerService    ← IClientsManagerService
///
/// Deferred services (platform-specific — no CrossPlatform implementation yet):
///   ISQLiteService      → requires platform file-path (Android/iOS) — register in platform startup
///   IDataBaseManagerService → depends on ISQLiteService — blocked until above
///   IFileService        → platform-specific file I/O
///   IFirebaseService    → platform-specific Firebase SDK
///   IDialogService      → platform-specific UI dialogs
///   IInativityTimerService → still depends on IMvxNavigationService (TASK-3.9 scope)
/// </summary>
public static class DiConfiguration
{
    /// <summary>
    /// Registers all Core business-logic services that have no platform dependency.
    /// Call from <see cref="MauiProgram.CreateMauiApp"/> via
    /// <c>builder.ConfigureCoreServices()</c>.
    /// </summary>
    public static MauiAppBuilder ConfigureCoreServices(this MauiAppBuilder builder)
    {
        RegisterCoreServices(builder.Services);
        RegisterLocationServices(builder.Services);
        RegisterNotificationServices(builder.Services);
        return builder;
    }

    // ── Core business-logic services ─────────────────────────────────────────

    /// <summary>
    /// Registers Core services that are platform-independent and have no remaining
    /// MvvmCross dependencies. Registration order follows the dependency graph.
    /// </summary>
    public static void RegisterCoreServices(IServiceCollection services)
    {
        // Leaf nodes — no dependencies
        services.AddSingleton<IClientsManagerService, ClientsManagerService>();
        services.AddSingleton<IDeliverysManagerService, DeliverysManagerService>();
        services.AddSingleton<IGlobalOrdersPastManagerService, GlobalOrdersPastManagerService>();

        // IClientsManagerService → IProductsManagerService
        services.AddSingleton<IProductsManagerService, ProductsManagerService>();

        // IProductsManagerService + IClientsManagerService → IOrdersManagerService
        services.AddSingleton<IOrdersManagerService, OrdersManagerService>();

        // IClientsManagerService → INotificationsManagerService
        services.AddSingleton<INotificationsManagerService, NotificationsManagerService>();
    }

    // ── Location services (TASK-3.5 + 3.6) ───────────────────────────────────

    /// <summary>
    /// Registers background location tracking and proximity detection services.
    /// Platform-specific <see cref="IBackgroundLocationTracker"/> implementation
    /// is resolved from the platform layer via the MAUI conditional compilation.
    /// </summary>
    public static void RegisterLocationServices(IServiceCollection services)
    {
        services.AddSingleton<IBackgroundLocationTracker, BackgroundLocationTracker>();
        services.AddSingleton<IProximityService, ProximityService>();
    }

    // ── Notification services (TASK-3.7) ─────────────────────────────────────

    /// <summary>
    /// Registers notification deduplication and send services.
    /// </summary>
    public static void RegisterNotificationServices(IServiceCollection services)
    {
        services.AddSingleton<INotificationStateStore, PreferencesNotificationStateStore>();
        services.AddSingleton<ILocalNotificationSender, MauiLocalNotificationSender>();
        services.AddSingleton<IProximityNotificationService, ProximityNotificationService>();
    }
}



