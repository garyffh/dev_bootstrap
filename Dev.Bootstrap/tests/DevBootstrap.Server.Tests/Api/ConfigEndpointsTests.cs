using System.Net;
using System.Net.Http.Json;
using DevBootstrap.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DevBootstrap.Server.Tests.Api;

public class ConfigEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ConfigEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetConfig_Returns_Ok()
    {
        var response = await _client.GetAsync("/api/config");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetConfig_Returns_AppConfig()
    {
        var config = await _client.GetFromJsonAsync<AppConfig>("/api/config");

        Assert.NotNull(config);
    }
}
