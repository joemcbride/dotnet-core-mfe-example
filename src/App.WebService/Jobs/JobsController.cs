using App.Infrastructure;
using App.WebService.Chassis;
using Microsoft.AspNetCore.Authorization;

namespace App.WebService.Jobs;

[Authorize(PolicyNames.ViewJobs)]
[ApiController]
[Route("[controller]")]
public class JobsController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;

    public JobsController(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet(Name = "/")]
    [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromServices] AllJobsQuery query)
    {
        var jobs = await _queryDispatcher.Execute(query);
        return Ok(jobs);
    }
}
