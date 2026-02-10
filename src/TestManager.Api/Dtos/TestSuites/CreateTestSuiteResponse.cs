using TestManager.Api.Dtos.TestCases;

namespace TestManager.Api.Dtos.TestSuites;

public class TestSuiteResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TestCaseCount { get; set; }
    public List<TestCaseResponse> TestCases { get; set; } = new();
}
