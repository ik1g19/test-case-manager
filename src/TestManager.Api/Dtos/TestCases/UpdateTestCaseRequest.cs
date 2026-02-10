namespace TestManager.Api.Dtos.TestCases;

public class UpdateTestCaseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Steps { get; set; } = string.Empty;
    public string ExpectedResult { get; set; } = string.Empty;
}
