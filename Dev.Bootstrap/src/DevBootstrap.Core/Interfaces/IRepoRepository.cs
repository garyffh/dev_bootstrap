using DevBootstrap.Core.Models;

namespace DevBootstrap.Core.Interfaces;

public interface IRepoRepository
{
    Task<IReadOnlyList<Repo>> GetAllAsync();
    Task<Repo?> GetByNameAsync(string name);
    Task AddAsync(Repo repo);
    Task UpdateAsync(Repo repo);
    Task DeleteAsync(string name);
}
