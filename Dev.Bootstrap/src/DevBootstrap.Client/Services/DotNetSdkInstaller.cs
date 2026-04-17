using System.Diagnostics;
using Serilog;

namespace DevBootstrap.Client.Services;

public class DotNetSdkInstaller
{
    private const string WingetId = "Microsoft.DotNet.SDK.8";
    private const string RequiredMajorMinor = "8.";

    public async Task EnsureInstalledAsync(Action<string> onStatus)
    {
        if (await HasSdkAsync(RequiredMajorMinor))
        {
            onStatus(".NET 8 SDK already installed.");
            return;
        }

        onStatus("Installing .NET 8 SDK via winget...");
        var exit = await RunProcessAsync(
            "winget",
            $"install -e --id {WingetId} --accept-source-agreements --accept-package-agreements --silent");

        if (exit == 0)
        {
            onStatus(".NET 8 SDK installed.");
        }
        else
        {
            onStatus($".NET 8 SDK install failed (winget exit {exit}). See log for details.");
        }
    }

    private static async Task<bool> HasSdkAsync(string majorMinorPrefix)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "--list-sdks",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null) return false;

            var stdout = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return process.ExitCode == 0 && stdout
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Any(line => line.TrimStart().StartsWith(majorMinorPrefix, StringComparison.Ordinal));
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to query installed .NET SDKs");
            return false;
        }
    }

    private static async Task<int> RunProcessAsync(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
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
}
