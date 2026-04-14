using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Server.Api;

public static class ConfigEndpoints
{
    public static void MapConfigEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/config");

        group.MapGet("/", async (IConfigRepository configRepo) =>
            Results.Ok(await configRepo.GetAsync()));

        group.MapPut("/", async (AppConfig updated, IConfigRepository configRepo) =>
        {
            await configRepo.UpdateAsync(updated);
            return Results.NoContent();
        });
    }
}
