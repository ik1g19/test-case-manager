using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TestManager.Api.Dtos.TestCases;
using TestManager.Api.Dtos.TestSuites;
using Xunit;

public class TestCasesApiTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TestCasesApiTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Can_add_test_case_to_suite()
    {
        // Create suite
        var suiteCreate = await _client.PostAsJsonAsync("/api/testsuites", new CreateTestSuiteRequest { Name = "Login" });
        suiteCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        var suite = await suiteCreate.Content.ReadFromJsonAsync<TestSuiteResponse>();
        suite!.Id.Should().NotBeEmpty();

        // Add test case
        var addResponse = await _client.PostAsJsonAsync(
            $"/api/testsuites/{suite.Id}/testcases",
            new CreateTestCaseRequest { Title = "Valid login", Steps = "Enter creds", ExpectedResult = "Logged in" });

        addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Fetch suite -> should contain test case
        var getSuite = await _client.GetAsync($"/api/testsuites/{suite.Id}");
        getSuite.StatusCode.Should().Be(HttpStatusCode.OK);

        var suiteAfter = await getSuite.Content.ReadFromJsonAsync<TestSuiteResponse>();
        suiteAfter!.TestCaseCount.Should().Be(1);
        suiteAfter.TestCases.Should().ContainSingle(tc => tc.Title == "Valid login");
    }

    [Fact]
    public async Task Adding_duplicate_test_case_title_returns_conflict()
    {
        var suiteCreate = await _client.PostAsJsonAsync("/api/testsuites", new CreateTestSuiteRequest { Name = "Login" });
        var suite = await suiteCreate.Content.ReadFromJsonAsync<TestSuiteResponse>();

        var req = new CreateTestCaseRequest { Title = "Valid login", Steps = "Steps", ExpectedResult = "Expected" };

        (await _client.PostAsJsonAsync($"/api/testsuites/{suite!.Id}/testcases", req)).StatusCode
            .Should().Be(HttpStatusCode.OK);

        (await _client.PostAsJsonAsync($"/api/testsuites/{suite.Id}/testcases", req)).StatusCode
            .Should().Be(HttpStatusCode.Conflict);
    }
}
