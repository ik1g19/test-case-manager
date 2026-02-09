using FluentAssertions;
using TestManager.Domain.Entities;
using TestManager.Domain.Exceptions;
using Xunit;

public class TestSuiteTests
{
    [Fact]
    public void Creating_test_suite_sets_name_and_id()
    {
        var suite = new TestSuite("Login Tests");

        suite.Id.Should().NotBeEmpty();
        suite.Name.Should().Be("Login Tests");
    }

    [Fact]
    public void Can_add_test_case_to_suite()
    {
        var suite = new TestSuite("Login Tests");
        var testCase = new TestCase(
            "Valid login",
            "Enter valid username and password",
            "User is logged in");

        suite.AddTestCase(testCase);

        suite.TestCases.Should().ContainSingle();
    }

    [Fact]
    public void Adding_duplicate_test_case_title_throws_exception()
    {
        var suite = new TestSuite("Login Tests");
        var testCase1 = new TestCase("Valid login", "Steps", "Expected");
        var testCase2 = new TestCase("Valid login", "Other steps", "Expected");

        suite.AddTestCase(testCase1);

        Action action = () => suite.AddTestCase(testCase2);

        action.Should().Throw<DuplicateTestCaseException>();
    }
}
