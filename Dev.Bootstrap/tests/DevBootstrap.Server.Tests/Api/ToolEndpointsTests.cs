using System.Net;
using System.Net.Http.Json;
using DevBootstrap.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DevBootstrap.Server.Tests.Api;

public class ToolEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ToolEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTools_Returns_Ok()
    {
        var response = await _client.GetAsync("/api/tools");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTools_Returns_Json_Array()
    {
        var tools = await _client.GetFromJsonAsync<List<Tool>>("/api/tools");

        Assert.NotNull(tools);
    }
}
