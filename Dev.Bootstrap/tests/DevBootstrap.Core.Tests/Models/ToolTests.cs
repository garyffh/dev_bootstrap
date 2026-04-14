using DevBootstrap.Core.Models;

namespace DevBootstrap.Core.Tests.Models;

public class ToolTests
{
    [Fact]
    public void New_Tool_Has_Empty_Defaults()
    {
        var tool = new Tool();

        Assert.Equal(string.Empty, tool.Name);
        Assert.Equal(string.Empty, tool.WingetId);
        Assert.Equal(string.Empty, tool.Type);
    }

    [Fact]
    public void Tool_Properties_Can_Be_Set()
    {
        var tool = new Tool
        {
            Name = "Git",
            WingetId = "Git.Git",
            Type = "base"
        };

        Assert.Equal("Git", tool.Name);
        Assert.Equal("Git.Git", tool.WingetId);
        Assert.Equal("base", tool.Type);
    }
}
