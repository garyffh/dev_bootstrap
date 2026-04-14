using DevBootstrap.Client.Services;
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

            var http = new HttpClient { BaseAddress = new Uri("http://localhost:5223") };
            var apiClient = new ApiClient(http);

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
