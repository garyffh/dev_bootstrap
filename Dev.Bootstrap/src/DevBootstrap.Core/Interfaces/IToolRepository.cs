using DevBootstrap.Core.Models;

namespace DevBootstrap.Core.Interfaces;

public interface IToolRepository
{
    Task<IReadOnlyList<Tool>> GetAllAsync();
    Task<Tool?> GetByNameAsync(string name);
    Task AddAsync(Tool tool);
    Task UpdateAsync(Tool tool);
    Task DeleteAsync(string name);
}
