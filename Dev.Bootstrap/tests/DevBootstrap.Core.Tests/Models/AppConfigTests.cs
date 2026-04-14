using DevBootstrap.Core.Models;

namespace DevBootstrap.Core.Tests.Models;

public class AppConfigTests
{
    [Fact]
    public void New_AppConfig_Has_Expected_Defaults()
    {
        var config = new AppConfig();

        Assert.Equal(string.Empty, config.GitHubAccount);
        Assert.Equal(@"C:\Projects", config.ClonePath);
    }

    [Fact]
    public void AppConfig_Properties_Can_Be_Set()
    {
        var config = new AppConfig
        {
            GitHubAccount = "garyffh",
            ClonePath = @"D:\Repos"
        };

        Assert.Equal("garyffh", config.GitHubAccount);
        Assert.Equal(@"D:\Repos", config.ClonePath);
    }
}
