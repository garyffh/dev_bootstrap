using DevBootstrap.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DevBootstrap.Dal;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<IRepoRepository, InMemoryRepoRepository>();
        services.AddSingleton<IToolRepository, InMemoryToolRepository>();
        services.AddSingleton<IConfigRepository, InMemoryConfigRepository>();
        services.AddTransient<IGitHubRepoSync, GitHubRepoSync>();
        return services;
    }
}
