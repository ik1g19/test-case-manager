namespace TestManager.Api.Dtos.TestCases;

public class TestCaseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Steps { get; set; } = string.Empty;
    public string ExpectedResult { get; set; } = string.Empty;
}
