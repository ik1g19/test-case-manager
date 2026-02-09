namespace TestManager.Domain.Exceptions;

public class DuplicateTestCaseException : Exception
{
    public DuplicateTestCaseException(string title)
        : base($"A test case with title '{title}' already exists.")
    {
    }
}
