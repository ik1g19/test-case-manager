using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManager.Api.Dtos.TestCases;
using TestManager.Infrastructure.Persistence;

namespace TestManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestCasesController : ControllerBase
{
    private readonly AppDbContext _db;

    public TestCasesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTestCaseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest("Title is required.");
        if (string.IsNullOrWhiteSpace(request.Steps)) return BadRequest("Steps is required.");
        if (string.IsNullOrWhiteSpace(request.ExpectedResult)) return BadRequest("ExpectedResult is required.");

        var testCase = await _db.TestCases.FirstOrDefaultAsync(tc => tc.Id == id);
        if (testCase is null)
            return NotFound();

        testCase.Update(
            request.Title.Trim(),
            request.Steps.Trim(),
            request.ExpectedResult.Trim());

        await _db.SaveChangesAsync();
        return NoContent();
    }
}
