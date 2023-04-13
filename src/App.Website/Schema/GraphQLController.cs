using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Transport;
using GraphQL.Types;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace App.Website.Schema;

[ApiController]
[Route("api/[controller]")]
public class GraphQLController : ControllerBase
{
    private readonly ILogger<GraphQLController> _logger;
    private readonly IAntiforgery _antiforgery;
    private readonly IGraphQLSerializer _serializer;
    private readonly IDocumentExecuter _documentExecuter;
    private readonly ISchema _schema;
    private readonly IOptions<GraphQLOptions> _options;

    public GraphQLController(
        ILogger<GraphQLController> logger,
        IAntiforgery antiforgery,
        IGraphQLSerializer serializer,
        IDocumentExecuter documentExecuter,
        ISchema schema,
        IOptions<GraphQLOptions> options)
    {
        _logger = logger;
        _antiforgery = antiforgery;
        _serializer = serializer;
        _documentExecuter = documentExecuter;
        _schema = schema;
        _options = options;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var request = await _serializer.ReadAsync<GraphQLRequest>(HttpContext.Request.Body, HttpContext.RequestAborted);

        if (_options.Value.EnableCSRFChecks)
        {
            await _antiforgery.ValidateRequestAsync(HttpContext);
        }

        var startTime = DateTime.UtcNow;

        var result = await _documentExecuter.ExecuteAsync(_ =>
        {
            _.Schema = _schema;
            _.Query = request.Query;
            _.Variables = request.Variables != null ? new Inputs(request.Variables) : null;
            _.OperationName = request.OperationName;
            _.RequestServices = HttpContext.RequestServices;
            _.UserContext = new GraphQLUserContext
            {
                User = HttpContext.User,
            };
            _.CancellationToken = HttpContext.RequestAborted;

            _.UnhandledExceptionDelegate = (exc) =>
            {
                _logger.LogError(exc.Exception, exc.Exception.Message);
                return Task.CompletedTask;
            };
        });

        if (_options.Value.EnableMetrics)
        {
            result.EnrichWithApolloTracing(startTime);
        }

        return new ExecutionResultActionResult(result);
    }
}
