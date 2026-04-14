using DevBootstrap.Client.Services;
using DevBootstrap.Core.Models;
using Serilog;

namespace DevBootstrap.Client;

public partial class MainForm : Form
{
    private readonly IApiClient _apiClient;
    private readonly RepoCloneService _cloneService;
    private IReadOnlyList<Repo> _repos = [];

    public MainForm(IApiClient apiClient, RepoCloneService cloneService)
    {
        _apiClient = apiClient;
        _cloneService = cloneService;
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
            _repos = await _apiClient.GetReposAsync();
            RefreshRepoList();
            LogStatus($"Loaded {_repos.Count} repos.");
        }
        catch (Exception ex)
        {
            LogStatus($"Error loading repos: {ex.Message}");
        }
    }

    private void RefreshRepoList()
    {
        clbRepos.Items.Clear();
        foreach (var repo in _repos)
        {
            var cloned = Directory.Exists(Path.Combine(@"C:\Projects", repo.Name));
            var display = cloned ? $"{repo.Name} [cloned]" : repo.Name;
            clbRepos.Items.Add(display, cloned);
        }
    }

    private async void btnCloneRepos_Click(object sender, EventArgs e)
    {
        using var dialog = new RepoSelectDialog(_repos);
        if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedRepos.Count == 0)
            return;

        btnCloneRepos.Enabled = false;
        try
        {
            foreach (var repo in dialog.SelectedRepos)
            {
                await _cloneService.CloneAsync(repo, LogStatus);
            }
            await LoadReposAsync();
        }
        finally
        {
            btnCloneRepos.Enabled = true;
        }
    }

    private void LogStatus(string message)
    {
        Log.Information(message);
        if (InvokeRequired)
        {
            Invoke(() => txtStatus.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}"));
        }
        else
        {
            txtStatus.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }
    }
}
