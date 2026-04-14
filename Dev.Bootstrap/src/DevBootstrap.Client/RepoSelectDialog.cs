using DevBootstrap.Core.Models;

namespace DevBootstrap.Client;

public partial class RepoSelectDialog : Form
{
    private readonly List<Repo> _allRepos;

    public List<Repo> SelectedRepos { get; } = [];

    public RepoSelectDialog(IReadOnlyList<Repo> repos)
    {
        _allRepos = repos.ToList();
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        PopulateList(string.Empty);
    }

    private void PopulateList(string filter)
    {
        clbAvailable.Items.Clear();
        var filtered = string.IsNullOrWhiteSpace(filter)
            ? _allRepos
            : _allRepos.Where(r =>
                r.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                r.Description.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

        foreach (var repo in filtered)
        {
            var display = string.IsNullOrWhiteSpace(repo.Description)
                ? repo.Name
                : $"{repo.Name} - {repo.Description}";
            clbAvailable.Items.Add(display, false);
        }
    }

    private void txtFilter_TextChanged(object sender, EventArgs e)
    {
        PopulateList(txtFilter.Text);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        var filter = txtFilter.Text;
        var filtered = string.IsNullOrWhiteSpace(filter)
            ? _allRepos
            : _allRepos.Where(r =>
                r.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                r.Description.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

        for (int i = 0; i < clbAvailable.Items.Count; i++)
        {
            if (clbAvailable.GetItemChecked(i))
            {
                SelectedRepos.Add(filtered[i]);
            }
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
