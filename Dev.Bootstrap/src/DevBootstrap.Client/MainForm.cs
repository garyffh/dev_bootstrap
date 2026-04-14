using DevBootstrap.Client.Services;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Client;

public partial class MainForm : Form
{
    private readonly IApiClient _apiClient;

    public MainForm(IApiClient apiClient)
    {
        _apiClient = apiClient;
        InitializeComponent();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadReposAsync();
    }

    private async Task LoadReposAsync()
    {
        try
        {
            LogStatus("Loading repos...");
            var repos = await _apiClient.GetReposAsync();
            clbRepos.Items.Clear();
            foreach (var repo in repos)
            {
                clbRepos.Items.Add(repo.Name, false);
            }
            LogStatus($"Loaded {repos.Count} repos.");
        }
        catch (Exception ex)
        {
            LogStatus($"Error loading repos: {ex.Message}");
        }
    }

    private void LogStatus(string message)
    {
        txtStatus.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
    }
}
