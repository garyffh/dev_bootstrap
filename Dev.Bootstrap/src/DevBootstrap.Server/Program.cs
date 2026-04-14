using DevBootstrap.Dal;
using DevBootstrap.Server.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess();

var app = builder.Build();

app.MapRepoEndpoints();
app.MapToolEndpoints();
app.MapConfigEndpoints();

app.Run();
