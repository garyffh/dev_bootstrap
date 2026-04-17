using System.Diagnostics;
using DevBootstrap.Core.Models;
using Serilog;

namespace DevBootstrap.Client.Services;

public class ClaudeSkillsInstaller
{
    private const string SkillsRepoName = "claude_skills";
    private const string InstallScript = "install-update.sh";

    private readonly RepoCloneService _cloneService;
    private readonly string _clonePath;

    public ClaudeSkillsInstaller(RepoCloneService cloneService, string clonePath)
    {
        _cloneService = cloneService;
        _clonePath = clonePath;
    }

    public async Task InstallOrUpdateAsync(Action<string> onStatus)
    {
        var skillsDir = Path.Combine(_clonePath, SkillsRepoName);

        if (!Directory.Exists(skillsDir))
        {
            await _cloneService.CloneAsync(new Repo { Name = SkillsRepoName }, onStatus);
        }

        var script = Path.Combine(skillsDir, InstallScript);
        if (!File.Exists(script))
        {
            onStatus($"{InstallScript} not found at {script}");
            return;
        }

        var bashPath = ResolveBashPath();
        if (bashPath == null)
        {
            onStatus("bash.exe not found -- install Git for Windows to enable claude_skills setup.");
            return;
        }

        onStatus($"Running {InstallScript}...");

        var psi = new ProcessStartInfo
        {
            FileName = bashPath,
            Arguments = InstallScript,
            WorkingDirectory = skillsDir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        if (process == null)
        {
            onStatus($"Failed to start bash for {InstallScript}");
            return;
        }

        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (!string.IsNullOrWhiteSpace(stdout))
        {
            Log.Information("claude_skills install-update stdout: {Stdout}", stdout);
        }

        if (process.ExitCode == 0)
        {
            onStatus("claude_skills installed/updated.");
        }
        else
        {
            Log.Error("claude_skills install-update failed (exit {Code}): {Error}", process.ExitCode, stderr);
            onStatus($"claude_skills install-update failed: {stderr.Trim()}");
        }
    }

    private static string? ResolveBashPath()
    {
        string[] candidates =
        {
            @"C:\Program Files\Git\bin\bash.exe",
            @"C:\Program Files\Git\usr\bin\bash.exe",
            @"C:\Program Files (x86)\Git\bin\bash.exe"
        };

        foreach (var path in candidates)
        {
            if (File.Exists(path)) return path;
        }

        return null;
    }
}
