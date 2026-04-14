using System.Diagnostics;
using System.Text.Json;
using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Dal;

public class GitHubRepoSync : IGitHubRepoSync
{
    private readonly IRepoRepository _repoRepository;

    public GitHubRepoSync(IRepoRepository repoRepository)
    {
        _repoRepository = repoRepository;
    }

    public async Task SyncAsync(string gitHubAccount)
    {
        var json = await RunGhAsync(gitHubAccount);
        if (string.IsNullOrWhiteSpace(json))
            return;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var ghRepos = JsonSerializer.Deserialize<List<GhRepoEntry>>(json, options);
        if (ghRepos == null)
            return;

        foreach (var entry in ghRepos)
        {
            var existing = await _repoRepository.GetByNameAsync(entry.Name);
            if (existing == null)
            {
                await _repoRepository.AddAsync(new Repo
                {
                    Name = entry.Name,
                    Description = entry.Description ?? string.Empty
                });
            }
        }
    }

    private static async Task<string> RunGhAsync(string account)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "gh",
            Arguments = $"repo list {account} --json name,description --limit 100",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        if (process == null)
            return string.Empty;

        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return process.ExitCode == 0 ? output : string.Empty;
    }

    private class GhRepoEntry
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
