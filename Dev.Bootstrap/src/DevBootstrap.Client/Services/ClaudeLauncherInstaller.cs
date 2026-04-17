using System.Diagnostics;
using System.Reflection;
using DevBootstrap.Core.Models;
using Serilog;

namespace DevBootstrap.Client.Services;

public class ClaudeLauncherInstaller
{
    private const string RepoName = "claude_launcher";
    private const string SolutionFile = "claude_launcher.sln";
    private const string ExeRelativePath = @"ClaudeLauncher\bin\Release\net8.0-windows\ClaudeLauncher.exe";
    private const string ShortcutName = "Claude Launcher.lnk";

    private readonly RepoCloneService _cloneService;
    private readonly string _clonePath;

    public ClaudeLauncherInstaller(RepoCloneService cloneService, string clonePath)
    {
        _cloneService = cloneService;
        _clonePath = clonePath;
    }

    public async Task InstallOrUpdateAsync(Action<string> onStatus)
    {
        var repoDir = Path.Combine(_clonePath, RepoName);
        if (!Directory.Exists(repoDir))
        {
            await _cloneService.CloneAsync(new Repo { Name = RepoName }, onStatus);
        }

        var solutionDir = Path.Combine(repoDir, RepoName);
        var solution = Path.Combine(solutionDir, SolutionFile);
        if (!File.Exists(solution))
        {
            onStatus($"{SolutionFile} not found at {solution}");
            return;
        }

        onStatus("Building claude_launcher (Release)...");
        var buildExit = await RunProcessAsync("dotnet", $"build \"{solution}\" -c Release --nologo", solutionDir);
        if (buildExit != 0)
        {
            onStatus($"claude_launcher build failed (exit {buildExit}). See log for details.");
            return;
        }

        var exe = Path.Combine(solutionDir, ExeRelativePath);
        if (!File.Exists(exe))
        {
            onStatus($"ClaudeLauncher.exe not found at {exe}");
            return;
        }

        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var shortcutPath = Path.Combine(desktop, ShortcutName);
        if (File.Exists(shortcutPath))
        {
            onStatus($"Claude Launcher shortcut already exists: {shortcutPath}");
            return;
        }

        try
        {
            CreateShortcut(shortcutPath, exe);
            onStatus($"Created desktop shortcut: {shortcutPath}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create Claude Launcher shortcut");
            onStatus($"Failed to create Claude Launcher shortcut: {ex.Message}");
        }
    }

    private static async Task<int> RunProcessAsync(string fileName, string arguments, string workingDirectory)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        if (process == null)
        {
            Log.Error("Failed to start {File} {Args}", fileName, arguments);
            return -1;
        }

        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (!string.IsNullOrWhiteSpace(stdout))
            Log.Information("{File} stdout: {Stdout}", fileName, stdout);
        if (process.ExitCode != 0 && !string.IsNullOrWhiteSpace(stderr))
            Log.Error("{File} stderr: {Stderr}", fileName, stderr);

        return process.ExitCode;
    }

    private static void CreateShortcut(string shortcutPath, string targetPath)
    {
        var shellType = Type.GetTypeFromProgID("WScript.Shell")
            ?? throw new InvalidOperationException("WScript.Shell COM type not available");
        var shell = Activator.CreateInstance(shellType)
            ?? throw new InvalidOperationException("Failed to create WScript.Shell instance");

        var shortcut = shellType.InvokeMember(
            "CreateShortcut", BindingFlags.InvokeMethod, null, shell, new object[] { shortcutPath })!;
        var scType = shortcut.GetType();

        scType.InvokeMember("TargetPath", BindingFlags.SetProperty, null, shortcut, new object[] { targetPath });
        scType.InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, shortcut,
            new object[] { Path.GetDirectoryName(targetPath)! });
        scType.InvokeMember("Save", BindingFlags.InvokeMethod, null, shortcut, null);
    }
}
