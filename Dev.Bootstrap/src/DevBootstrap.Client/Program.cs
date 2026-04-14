using DevBootstrap.Client.Services;

namespace DevBootstrap.Client;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var http = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
        var apiClient = new ApiClient(http);

        Application.Run(new MainForm(apiClient));
    }
}
