using System.Collections.Concurrent;
using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Dal;

public class InMemoryRepoRepository : IRepoRepository
{
    private readonly ConcurrentDictionary<string, Repo> _repos = new();

    public Task<IReadOnlyList<Repo>> GetAllAsync()
    {
        IReadOnlyList<Repo> result = _repos.Values.ToList();
        return Task.FromResult(result);
    }

    public Task<Repo?> GetByNameAsync(string name)
    {
        _repos.TryGetValue(name, out var repo);
        return Task.FromResult(repo);
    }

    public Task AddAsync(Repo repo)
    {
        _repos[repo.Name] = repo;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Repo repo)
    {
        _repos[repo.Name] = repo;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string name)
    {
        _repos.TryRemove(name, out _);
        return Task.CompletedTask;
    }
}
