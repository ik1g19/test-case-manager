namespace TestManager.Domain.Entities;

public class TestCase
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Steps { get; private set; }
    public string ExpectedResult { get; private set; }

    private TestCase() { } // For ORM later

    public TestCase(string title, string steps, string expectedResult)
    {
        Id = Guid.NewGuid();
        Title = title;
        Steps = steps;
        ExpectedResult = expectedResult;
    }

    public void Update(string title, string steps, string expectedResult)
    {
        Title = title;
        Steps = steps;
        ExpectedResult = expectedResult;
    }
}
