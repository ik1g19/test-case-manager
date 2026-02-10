using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManager.Api.Dtos.TestSuites;
using TestManager.Domain.Entities;
using TestManager.Infrastructure.Persistence;
using TestManager.Api.Dtos.TestCases;
using TestManager.Domain.Exceptions;

namespace TestManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestSuitesController : ControllerBase
{
    private readonly AppDbContext _db;

    public TestSuitesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<TestSuiteResponse>> Create([FromBody] CreateTestSuiteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required.");

        if (request.Name.Length > 200)
            return BadRequest("Name must be 200 characters or fewer.");

        var suite = new TestSuite(request.Name.Trim());

        _db.TestSuites.Add(suite);
        await _db.SaveChangesAsync();

        var response = new TestSuiteResponse
        {
            Id = suite.Id,
            Name = suite.Name,
            TestCaseCount = 0
        };

        // Returns 201 + Location header pointing at GET by id
        return CreatedAtAction(nameof(GetById), new { id = suite.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult<List<TestSuiteResponse>>> GetAll()
    {
        var suites = await _db.TestSuites
            .AsNoTracking()
            .Select(ts => new TestSuiteResponse
            {
                Id = ts.Id,
                Name = ts.Name,
                TestCaseCount = ts.TestCases.Count
            })
            .OrderBy(ts => ts.Name)
            .ToListAsync();

        return Ok(suites);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TestSuiteResponse>> GetById(Guid id)
    {
        var suite = await _db.TestSuites
            .AsNoTracking()
            .Include(ts => ts.TestCases)
            .FirstOrDefaultAsync(ts => ts.Id == id);

        if (suite is null)
            return NotFound();

        var response = new TestSuiteResponse
        {
            Id = suite.Id,
            Name = suite.Name,
            TestCaseCount = suite.TestCases.Count,
            TestCases = suite.TestCases.Select(tc => new TestCaseResponse
            {
                Id = tc.Id,
                Title = tc.Title,
                Steps = tc.Steps,
                ExpectedResult = tc.ExpectedResult
            }).ToList()
        };

        return Ok(response);
    }

    [HttpPost("{id:guid}/testcases")]
    public async Task<ActionResult<TestCaseResponse>> AddTestCase(Guid id, [FromBody] CreateTestCaseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest("Title is required.");
        if (string.IsNullOrWhiteSpace(request.Steps)) return BadRequest("Steps is required.");
        if (string.IsNullOrWhiteSpace(request.ExpectedResult)) return BadRequest("ExpectedResult is required.");

        var suite = await _db.TestSuites
            .Include(ts => ts.TestCases)
            .FirstOrDefaultAsync(ts => ts.Id == id);

        if (suite is null)
            return NotFound();

        var testCase = new TestCase(
            request.Title.Trim(),
            request.Steps.Trim(),
            request.ExpectedResult.Trim());

        try
        {
            suite.AddTestCase(testCase); // domain rule lives here
        }
        catch (DuplicateTestCaseException ex)
        {
            // 409 Conflict is a nice REST-y outcome
            return Conflict(ex.Message);
        }

        await _db.SaveChangesAsync();

        var response = new TestCaseResponse
        {
            Id = testCase.Id,
            Title = testCase.Title,
            Steps = testCase.Steps,
            ExpectedResult = testCase.ExpectedResult
        };

        return Ok(response);
    }
}
