using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using TestManager.Api.Dtos.TestSuites;

public class TestSuitesApiTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TestSuitesApiTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_then_get_returns_created_suite()
    {
        var create = new CreateTestSuiteRequest { Name = "Login Tests" };

        var postResponse = await _client.PostAsJsonAsync("/api/testsuites", create);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await postResponse.Content.ReadFromJsonAsync<TestSuiteResponse>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBeEmpty();
        created.Name.Should().Be("Login Tests");

        var getResponse = await _client.GetAsync($"/api/testsuites/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
