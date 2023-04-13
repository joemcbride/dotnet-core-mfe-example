using App.Domain;
using App.Infrastructure;
using App.Website.Schema;
using GraphQL;

namespace App.Website.Jobs;

[GraphQLMetadata("Jobs")]
[GraphQLSchemaMetadataAttribute(@"
    input CreateJobInput {
        dataFileOnly: Boolean!
    }

    type Job {
        jobId: ID!
        status: String!
        resultFile: String
        startedAtTimeUtc: DateTimeOffset!
        endedAtTimeUtc: DateTimeOffset
    }

    type Jobs {
        all: [Job]!
        create(input: CreateJobInput!): Job
    }

    extend type Query {
        jobs: Jobs!
    }
")]
public class JobsGraphType
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly AllJobsQuery _allJobsQuery;

    public JobsGraphType(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        AllJobsQuery query)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _allJobsQuery = query;
    }

    public Task<IEnumerable<JobDto>> All()
    {
        return _queryDispatcher.Execute(_allJobsQuery);
    }

    public async Task<JobDto> Create(CreateJobInputDto input)
    {
        var command = new CreateJobCommand
        {
            DataFileOnly = input.DataFileOnly
        };

        var result = await _commandDispatcher.Execute<CreateJobCommand, JobDto>(command);
        return result;
    }
}

[GraphQLMetadata("CreateJobInput")]
public class CreateJobInputDto
{
    public bool DataFileOnly { get; set; }
}
