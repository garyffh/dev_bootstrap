using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Client.Services;

public class ApiClient : IApiClient
{
    private readonly IRepoRepository _repoRepository;
    private readonly IToolRepository _toolRepository;
    private readonly IConfigRepository _configRepository;

    public ApiClient(
        IRepoRepository repoRepository,
        IToolRepository toolRepository,
        IConfigRepository configRepository)
    {
        _repoRepository = repoRepository;
        _toolRepository = toolRepository;
        _configRepository = configRepository;
    }

    public async Task<IReadOnlyList<Repo>> GetReposAsync()
    {
        return await _repoRepository.GetAllAsync();
    }

    public async Task<IReadOnlyList<Tool>> GetToolsAsync()
    {
        return await _toolRepository.GetAllAsync();
    }

    public async Task<AppConfig> GetConfigAsync()
    {
        return await _configRepository.GetAsync();
    }
}
