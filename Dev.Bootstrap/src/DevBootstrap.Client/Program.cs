using DevBootstrap.Client.Services;
using DevBootstrap.Core.Interfaces;
using DevBootstrap.Dal;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DevBootstrap.Client;

static class Program
{
    [STAThread]
    static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/client-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting DevBootstrap Client");

            ApplicationConfiguration.Initialize();

            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            services.AddDataAccess();
            var provider = services.BuildServiceProvider();

            var sync = provider.GetRequiredService<IGitHubRepoSync>();
            sync.SyncAsync("garyffh").GetAwaiter().GetResult();
            Log.Information("GitHub repo sync complete");

            var apiClient = new ApiClient(
                provider.GetRequiredService<IRepoRepository>(),
                provider.GetRequiredService<IToolRepository>(),
                provider.GetRequiredService<IConfigRepository>());

            Application.Run(new MainForm(apiClient));
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Client terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
