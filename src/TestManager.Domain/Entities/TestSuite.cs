namespace TestManager.Domain.Entities;
using TestManager.Domain.Exceptions;

public class TestSuite
{
    private readonly List<TestCase> _testCases = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public IReadOnlyCollection<TestCase> TestCases => _testCases.AsReadOnly();

    private TestSuite() { }

    public TestSuite(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void AddTestCase(TestCase testCase)
    {
        if (_testCases.Any(tc => tc.Title == testCase.Title))
        {
            throw new DuplicateTestCaseException(testCase.Title);
        }

        _testCases.Add(testCase);
    }
}
