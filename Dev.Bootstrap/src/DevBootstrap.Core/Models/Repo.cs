namespace DevBootstrap.Core.Models;

public class Repo
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Dependencies { get; set; } = [];
}
