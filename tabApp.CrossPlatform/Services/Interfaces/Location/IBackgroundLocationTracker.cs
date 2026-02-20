namespace tabApp.CrossPlatform.Services.Interfaces.Location;

public interface IBackgroundLocationTracker
{
    TimeSpan UpdateInterval { get; }
    Task<bool> StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}

