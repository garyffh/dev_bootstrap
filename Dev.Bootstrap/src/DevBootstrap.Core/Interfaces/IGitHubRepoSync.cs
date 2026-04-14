namespace DevBootstrap.Core.Interfaces;

public interface IGitHubRepoSync
{
    Task SyncAsync(string gitHubAccount);
}
