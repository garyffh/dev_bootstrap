using DevBootstrap.Dal;
using DevBootstrap.Server.Api;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/server-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting DevBootstrap Server");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services.AddDataAccess();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.MapRepoEndpoints();
    app.MapToolEndpoints();
    app.MapConfigEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
