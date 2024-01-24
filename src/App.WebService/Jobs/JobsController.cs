using App.Infrastructure;
using App.WebService.Chassis;
using Microsoft.AspNetCore.Authorization;

namespace App.WebService.Jobs;

[Authorize(PolicyNames.ViewJobs)]
[ApiController]
[Route("[controller]")]
public class JobsController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public JobsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
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

    [HttpPost(Name = "/")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromServices] CreateJobDto createJobDto)
    {
        var command = new CreateJobCommand { DataFileOnly = createJobDto.DataFileOnly };
        var job = await _commandDispatcher.Execute<CreateJobCommand, JobDto>(command);
        return Ok(job);
    }
}

public class CreateJobDto
{
    public bool DataFileOnly { get; set; }
}
