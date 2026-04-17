using DevBootstrap.Core.Interfaces;
using Serilog;

namespace DevBootstrap.Client.Services;

public class BootstrapPipeline
{
    private readonly IGitHubRepoSync _sync;
    private readonly ClaudeSkillsInstaller _skills;
    private readonly DotNetSdkInstaller _sdk;
    private readonly ClaudeLauncherInstaller _launcher;
    private readonly string _gitHubAccount;

    public BootstrapPipeline(
        IGitHubRepoSync sync,
        ClaudeSkillsInstaller skills,
        DotNetSdkInstaller sdk,
        ClaudeLauncherInstaller launcher,
        string gitHubAccount)
    {
        _sync = sync;
        _skills = skills;
        _sdk = sdk;
        _launcher = launcher;
        _gitHubAccount = gitHubAccount;
    }

    public async Task RunAsync(Action<string> onStatus)
    {
        onStatus("Bootstrap starting...");

        await SafeRunAsync("GitHub repo sync", onStatus, async () =>
        {
            onStatus($"Syncing repos from github.com/{_gitHubAccount}...");
            await _sync.SyncAsync(_gitHubAccount);
            onStatus("GitHub repo sync complete.");
        });

        await SafeRunAsync("claude_skills install", onStatus,
            () => _skills.InstallOrUpdateAsync(onStatus));

        await SafeRunAsync(".NET 8 SDK check", onStatus,
            () => _sdk.EnsureInstalledAsync(onStatus));

        await SafeRunAsync("claude_launcher install", onStatus,
            () => _launcher.InstallOrUpdateAsync(onStatus));

        onStatus("Bootstrap complete.");
    }

    private static async Task SafeRunAsync(string step, Action<string> onStatus, Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "{Step} failed", step);
            onStatus($"{step} failed: {ex.Message}");
        }
    }
}
