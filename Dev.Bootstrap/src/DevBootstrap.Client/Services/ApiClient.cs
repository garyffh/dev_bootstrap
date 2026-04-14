using System.Net.Http.Json;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Client.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<Repo>> GetReposAsync()
    {
        var repos = await _http.GetFromJsonAsync<List<Repo>>("/api/repos");
        return repos ?? [];
    }

    public async Task<IReadOnlyList<Tool>> GetToolsAsync()
    {
        var tools = await _http.GetFromJsonAsync<List<Tool>>("/api/tools");
        return tools ?? [];
    }

    public async Task<AppConfig> GetConfigAsync()
    {
        var config = await _http.GetFromJsonAsync<AppConfig>("/api/config");
        return config ?? new AppConfig();
    }
}
