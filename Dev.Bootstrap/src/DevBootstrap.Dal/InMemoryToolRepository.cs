using System.Collections.Concurrent;
using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Dal;

public class InMemoryToolRepository : IToolRepository
{
    private readonly ConcurrentDictionary<string, Tool> _tools = new();

    public Task<IReadOnlyList<Tool>> GetAllAsync()
    {
        IReadOnlyList<Tool> result = _tools.Values.ToList();
        return Task.FromResult(result);
    }

    public Task<Tool?> GetByNameAsync(string name)
    {
        _tools.TryGetValue(name, out var tool);
        return Task.FromResult(tool);
    }

    public Task AddAsync(Tool tool)
    {
        _tools[tool.Name] = tool;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Tool tool)
    {
        _tools[tool.Name] = tool;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string name)
    {
        _tools.TryRemove(name, out _);
        return Task.CompletedTask;
    }
}
