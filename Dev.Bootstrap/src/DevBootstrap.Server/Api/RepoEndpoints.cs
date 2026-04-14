using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Server.Api;

public static class RepoEndpoints
{
    public static void MapRepoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/repos");

        group.MapGet("/", async (IRepoRepository repo) =>
            Results.Ok(await repo.GetAllAsync()));

        group.MapGet("/{name}", async (string name, IRepoRepository repo) =>
            await repo.GetByNameAsync(name) is { } found
                ? Results.Ok(found)
                : Results.NotFound());

        group.MapPost("/", async (Repo newRepo, IRepoRepository repo) =>
        {
            await repo.AddAsync(newRepo);
            return Results.Created($"/api/repos/{newRepo.Name}", newRepo);
        });

        group.MapPut("/{name}", async (string name, Repo updated, IRepoRepository repo) =>
        {
            updated.Name = name;
            await repo.UpdateAsync(updated);
            return Results.NoContent();
        });

        group.MapDelete("/{name}", async (string name, IRepoRepository repo) =>
        {
            await repo.DeleteAsync(name);
            return Results.NoContent();
        });
    }
}
