using Microsoft.Extensions.DependencyInjection;

namespace DevBootstrap.Dal;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        // Register concrete repository implementations here
        // when the DB provider is chosen.
        return services;
    }
}
