using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Dal;

public class InMemoryConfigRepository : IConfigRepository
{
    private AppConfig _config = new()
    {
        GitHubAccount = "garyffh",
        ClonePath = @"C:\Projects"
    };

    public Task<AppConfig> GetAsync()
    {
        return Task.FromResult(_config);
    }

    public Task UpdateAsync(AppConfig config)
    {
        _config = config;
        return Task.CompletedTask;
    }
}
