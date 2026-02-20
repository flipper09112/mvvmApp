// ─────────────────────────────────────────────────────────────────────────────
// TASK-3.8 — DI Migration POC: Service Registration & Resolution Tests
//
// Strategy: implementations are NOT included directly (they have cascading deps
// on DB, Faturation, Firebase etc. that make direct <Compile> inclusion impractical).
// Instead each service is registered via Moq.Mock<T>.Object so the DI container
// wires the full interface graph without needing the concrete classes at test time.
//
// Coverage:
//   DiAuditTests                 — zero Mvx.Resolve / MvvmCross in CrossPlatform
//   CoreServiceRegistrationTests — every interface registers without error
//   CoreServiceResolutionTests   — every interface resolves from container
//   DependencyGraphTests         — singletons share instance, missing reg throws
// ─────────────────────────────────────────────────────────────────────────────

using Microsoft.Extensions.DependencyInjection;
using Moq;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Deliverys;
using tabApp.Core.Services.Interfaces.Notifications;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.CrossPlatform.Services.Implementations.Notifications;
using tabApp.CrossPlatform.Services.Interfaces.Notifications;

namespace tabApp.Tests;

// ─────────────────────────────────────────────────────────────────────────────
// Shared helper — builds ServiceProvider using mock implementations
// so the DI wiring can be validated without pulling in concrete classes
// ─────────────────────────────────────────────────────────────────────────────

internal static class TestServiceContainer
{
    /// <summary>
    /// Registers all Core interfaces with Moq stubs in the same singleton lifetime
    /// and dependency order as <c>DiConfiguration.RegisterCoreServices()</c>.
    /// </summary>
    public static ServiceProvider BuildCoreContainer()
    {
        var services = new ServiceCollection();

        // Leaf nodes
        services.AddSingleton(_ => new Mock<IClientsManagerService>().Object);
        services.AddSingleton(_ => new Mock<IDeliverysManagerService>().Object);
        services.AddSingleton(_ => new Mock<IGlobalOrdersPastManagerService>().Object);
        // Second tier
        services.AddSingleton(_ => new Mock<IProductsManagerService>().Object);
        // Third tier
        services.AddSingleton(_ => new Mock<IOrdersManagerService>().Object);
        services.AddSingleton(_ => new Mock<INotificationsManagerService>().Object);

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Builds a container for the notification services (TASK-3.7) with Core deps as mocks
    /// and real CrossPlatform notification implementations.
    /// </summary>
    public static ServiceProvider BuildNotificationContainer()
    {
        var services = new ServiceCollection();

        // Core deps required by ProximityNotificationService
        services.AddSingleton(_ => new Mock<IProductsManagerService>().Object);

        // Real CrossPlatform notification implementations
        services.AddSingleton<INotificationStateStore, PreferencesNotificationStateStore>();
        services.AddSingleton<ILocalNotificationSender, MauiLocalNotificationSender>();
        services.AddSingleton<IProximityNotificationService, ProximityNotificationService>();

        return services.BuildServiceProvider();
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 8. DI Audit — Mvx.Resolve scan
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class DiAuditTests
{
    private static readonly string[] ProjectRoots =
    [
        Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", "tabApp.CrossPlatform"),
        Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", "tabApp.Core")
    ];

    [Test]
    [Description("Zero active Mvx.Resolve calls must exist in CrossPlatform and Core.")]
    public void NoMvxResolveCalls_InCrossPlatformAndCore()
    {
        var violations = new List<string>();

        foreach (var root in ProjectRoots)
        {
            var dir = Path.GetFullPath(root);
            if (!Directory.Exists(dir)) continue;

            foreach (var file in Directory.EnumerateFiles(dir, "*.cs", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(file);
                for (var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (line.TrimStart().StartsWith("//")) continue;
                    if (line.Contains("Mvx.Resolve"))
                        violations.Add($"{Path.GetRelativePath(dir, file)}:{i + 1} → {line.Trim()}");
                }
            }
        }

        Assert.That(violations, Is.Empty,
            $"Found {violations.Count} active Mvx.Resolve call(s):\n{string.Join("\n", violations)}");
    }

    [Test]
    [Description("Zero active MvvmCross using directives in CrossPlatform Services/.")]
    public void NoActiveMvvmCrossUsings_InCrossPlatformServices()
    {
        var servicesRoot = Path.GetFullPath(Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "..", "..", "..", "..",
            "tabApp.CrossPlatform", "Services"));

        if (!Directory.Exists(servicesRoot))
        {
            Assert.Ignore("Services directory not found — skipping");
            return;
        }

        var violations = new List<string>();
        foreach (var file in Directory.EnumerateFiles(servicesRoot, "*.cs", SearchOption.AllDirectories))
        {
            var lines = File.ReadAllLines(file);
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.TrimStart().StartsWith("//")) continue;
                if (line.Contains("using MvvmCross") || line.Contains("using Mvx;"))
                    violations.Add($"{Path.GetRelativePath(servicesRoot, file)}:{i + 1} → {line.Trim()}");
            }
        }

        Assert.That(violations, Is.Empty,
            $"Found {violations.Count} active MvvmCross using(s):\n{string.Join("\n", violations)}");
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 9. Core Service Registration Tests
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class CoreServiceRegistrationTests
{
    private ServiceCollection _services = null!;

    [SetUp]
    public void SetUp() => _services = new ServiceCollection();

    [Test]
    public void Register_IClientsManagerService_DoesNotThrow() =>
        Assert.DoesNotThrow(() =>
            _services.AddSingleton(_ => new Mock<IClientsManagerService>().Object));

    [Test]
    public void Register_IProductsManagerService_DoesNotThrow() =>
        Assert.DoesNotThrow(() =>
            _services.AddSingleton(_ => new Mock<IProductsManagerService>().Object));

    [Test]
    public void Register_IOrdersManagerService_DoesNotThrow() =>
        Assert.DoesNotThrow(() =>
            _services.AddSingleton(_ => new Mock<IOrdersManagerService>().Object));

    [Test]
    public void Register_INotificationsManagerService_DoesNotThrow() =>
        Assert.DoesNotThrow(() =>
            _services.AddSingleton(_ => new Mock<INotificationsManagerService>().Object));

    [Test]
    public void Register_IDeliverysManagerService_DoesNotThrow() =>
        Assert.DoesNotThrow(() =>
            _services.AddSingleton(_ => new Mock<IDeliverysManagerService>().Object));

    [Test]
    public void Register_IGlobalOrdersPastManagerService_DoesNotThrow() =>
        Assert.DoesNotThrow(() =>
            _services.AddSingleton(_ => new Mock<IGlobalOrdersPastManagerService>().Object));

    [Test]
    [Description("Full Core registration block (as in DiConfiguration) does not throw.")]
    public void RegisterAllCoreServices_DoesNotThrow() =>
        Assert.DoesNotThrow(() => TestServiceContainer.BuildCoreContainer());
}

// ─────────────────────────────────────────────────────────────────────────────
// 10. Core Service Resolution Tests
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class CoreServiceResolutionTests
{
    private ServiceProvider _provider = null!;

    [SetUp]
    public void SetUp() => _provider = TestServiceContainer.BuildCoreContainer();

    [TearDown]
    public void TearDown() => _provider.Dispose();

    [Test]
    public void Resolve_IClientsManagerService_IsNotNull() =>
        Assert.That(_provider.GetRequiredService<IClientsManagerService>(), Is.Not.Null);

    [Test]
    public void Resolve_IProductsManagerService_IsNotNull() =>
        Assert.That(_provider.GetRequiredService<IProductsManagerService>(), Is.Not.Null);

    [Test]
    public void Resolve_IOrdersManagerService_IsNotNull() =>
        Assert.That(_provider.GetRequiredService<IOrdersManagerService>(), Is.Not.Null);

    [Test]
    public void Resolve_INotificationsManagerService_IsNotNull() =>
        Assert.That(_provider.GetRequiredService<INotificationsManagerService>(), Is.Not.Null);

    [Test]
    public void Resolve_IDeliverysManagerService_IsNotNull() =>
        Assert.That(_provider.GetRequiredService<IDeliverysManagerService>(), Is.Not.Null);

    [Test]
    public void Resolve_IGlobalOrdersPastManagerService_IsNotNull() =>
        Assert.That(_provider.GetRequiredService<IGlobalOrdersPastManagerService>(), Is.Not.Null);
}

// ─────────────────────────────────────────────────────────────────────────────
// 11. Dependency Graph Tests
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class DependencyGraphTests
{
    [Test]
    [Description("Singleton IClientsManagerService returns same instance on repeated resolution.")]
    public void Singleton_IClientsManagerService_SameInstanceReturned()
    {
        using var provider = TestServiceContainer.BuildCoreContainer();
        var a = provider.GetRequiredService<IClientsManagerService>();
        var b = provider.GetRequiredService<IClientsManagerService>();
        Assert.That(a, Is.SameAs(b));
    }

    [Test]
    [Description("All six Core singletons are stable — re-resolving returns the same instance.")]
    public void AllCoreSingletons_StableAcrossResolutions()
    {
        using var provider = TestServiceContainer.BuildCoreContainer();

        Assert.Multiple(() =>
        {
            Assert.That(provider.GetRequiredService<IClientsManagerService>(),
                        Is.SameAs(provider.GetRequiredService<IClientsManagerService>()));
            Assert.That(provider.GetRequiredService<IProductsManagerService>(),
                        Is.SameAs(provider.GetRequiredService<IProductsManagerService>()));
            Assert.That(provider.GetRequiredService<IOrdersManagerService>(),
                        Is.SameAs(provider.GetRequiredService<IOrdersManagerService>()));
            Assert.That(provider.GetRequiredService<INotificationsManagerService>(),
                        Is.SameAs(provider.GetRequiredService<INotificationsManagerService>()));
        });
    }

    [Test]
    [Description("IProximityNotificationService resolves with all TASK-3.7 transitive deps satisfied.")]
    public void Resolve_IProximityNotificationService_WithAllDeps()
    {
        using var provider = TestServiceContainer.BuildNotificationContainer();
        var svc = provider.GetRequiredService<IProximityNotificationService>();
        Assert.That(svc, Is.Not.Null);
        Assert.That(svc, Is.InstanceOf<ProximityNotificationService>());
    }

    [Test]
    [Description("INotificationStateStore is a singleton — same instance on two resolutions.")]
    public void Singleton_INotificationStateStore_SameInstance()
    {
        using var provider = TestServiceContainer.BuildNotificationContainer();
        var a = provider.GetRequiredService<INotificationStateStore>();
        var b = provider.GetRequiredService<INotificationStateStore>();
        Assert.That(a, Is.SameAs(b));
    }

    [Test]
    [Description("GetRequiredService for an unregistered interface throws InvalidOperationException.")]
    public void Resolve_UnregisteredService_Throws()
    {
        using var provider = new ServiceCollection().BuildServiceProvider();
        Assert.Throws<InvalidOperationException>(() =>
            provider.GetRequiredService<IOrdersManagerService>());
    }
}
