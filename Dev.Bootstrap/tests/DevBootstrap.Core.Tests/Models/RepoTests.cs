using DevBootstrap.Core.Models;

namespace DevBootstrap.Core.Tests.Models;

public class RepoTests
{
    [Fact]
    public void New_Repo_Has_Empty_Defaults()
    {
        var repo = new Repo();

        Assert.Equal(string.Empty, repo.Name);
        Assert.Equal(string.Empty, repo.Description);
        Assert.NotNull(repo.Dependencies);
        Assert.Empty(repo.Dependencies);
    }

    [Fact]
    public void Repo_Properties_Can_Be_Set()
    {
        var repo = new Repo
        {
            Name = "my-repo",
            Description = "A test repo",
            Dependencies = ["git", "node"]
        };

        Assert.Equal("my-repo", repo.Name);
        Assert.Equal("A test repo", repo.Description);
        Assert.Equal(2, repo.Dependencies.Count);
        Assert.Contains("git", repo.Dependencies);
        Assert.Contains("node", repo.Dependencies);
    }
}
