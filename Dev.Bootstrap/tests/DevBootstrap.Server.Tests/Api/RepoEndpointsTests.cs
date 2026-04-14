using System.Net;
using System.Net.Http.Json;
using DevBootstrap.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DevBootstrap.Server.Tests.Api;

public class RepoEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RepoEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRepos_Returns_Ok()
    {
        var response = await _client.GetAsync("/api/repos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRepos_Returns_Json_Array()
    {
        var repos = await _client.GetFromJsonAsync<List<Repo>>("/api/repos");

        Assert.NotNull(repos);
    }
}
