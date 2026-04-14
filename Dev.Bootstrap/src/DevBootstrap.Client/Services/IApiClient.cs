using DevBootstrap.Core.Models;

namespace DevBootstrap.Client.Services;

public interface IApiClient
{
    Task<IReadOnlyList<Repo>> GetReposAsync();
    Task<IReadOnlyList<Tool>> GetToolsAsync();
    Task<AppConfig> GetConfigAsync();
}
