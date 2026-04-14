using DevBootstrap.Core.Interfaces;
using DevBootstrap.Core.Models;

namespace DevBootstrap.Server.Api;

public static class ToolEndpoints
{
    public static void MapToolEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tools");

        group.MapGet("/", async (IToolRepository toolRepo) =>
            Results.Ok(await toolRepo.GetAllAsync()));

        group.MapGet("/{name}", async (string name, IToolRepository toolRepo) =>
            await toolRepo.GetByNameAsync(name) is { } found
                ? Results.Ok(found)
                : Results.NotFound());

        group.MapPost("/", async (Tool newTool, IToolRepository toolRepo) =>
        {
            await toolRepo.AddAsync(newTool);
            return Results.Created($"/api/tools/{newTool.Name}", newTool);
        });

        group.MapPut("/{name}", async (string name, Tool updated, IToolRepository toolRepo) =>
        {
            updated.Name = name;
            await toolRepo.UpdateAsync(updated);
            return Results.NoContent();
        });

        group.MapDelete("/{name}", async (string name, IToolRepository toolRepo) =>
        {
            await toolRepo.DeleteAsync(name);
            return Results.NoContent();
        });
    }
}
