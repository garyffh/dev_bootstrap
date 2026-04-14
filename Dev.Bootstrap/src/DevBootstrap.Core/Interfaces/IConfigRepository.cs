using DevBootstrap.Core.Models;

namespace DevBootstrap.Core.Interfaces;

public interface IConfigRepository
{
    Task<AppConfig> GetAsync();
    Task UpdateAsync(AppConfig config);
}
