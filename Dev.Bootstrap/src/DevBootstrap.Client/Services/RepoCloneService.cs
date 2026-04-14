using System.Diagnostics;
using DevBootstrap.Core.Models;
using Serilog;

namespace DevBootstrap.Client.Services;

public class RepoCloneService
{
    private readonly string _gitHubAccount;
    private readonly string _clonePath;

    public RepoCloneService(string gitHubAccount, string clonePath)
    {
        _gitHubAccount = gitHubAccount;
        _clonePath = clonePath;
    }

    public async Task<bool> CloneAsync(Repo repo, Action<string> onStatus)
    {
        var targetDir = Path.Combine(_clonePath, repo.Name);
        if (Directory.Exists(targetDir))
        {
            onStatus($"Skipping {repo.Name} -- already exists at {targetDir}");
            return true;
        }

        Directory.CreateDirectory(_clonePath);

        var repoUrl = $"https://github.com/{_gitHubAccount}/{repo.Name}.git";
        onStatus($"Cloning {repo.Name}...");

        var psi = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = $"clone {repoUrl} \"{targetDir}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        if (process == null)
        {
            onStatus($"Failed to start git for {repo.Name}");
            return false;
        }

        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode == 0)
        {
            onStatus($"Cloned {repo.Name} to {targetDir}");
            return true;
        }
        else
        {
            Log.Error("git clone failed for {Repo}: {Error}", repo.Name, stderr);
            onStatus($"Failed to clone {repo.Name}: {stderr.Trim()}");
            return false;
        }
    }
}
