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
        job(jobId: ID!): Job
    }

    extend type Query {
        jobs: Jobs!
    }

    extend type Mutation {
        createJob(input: CreateJobInput!): Job
    }
")]
public class JobsGraphType
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly AllJobsQuery _allJobsQuery;
    private readonly GetJobQuery _getJobQuery;

    public JobsGraphType(
        IQueryDispatcher queryDispatcher,
        AllJobsQuery query,
        GetJobQuery getJobQuery)
    {
        _queryDispatcher = queryDispatcher;
        _allJobsQuery = query;
        _getJobQuery = getJobQuery;
    }

    public Task<IEnumerable<JobDto>> All()
    {
        return _queryDispatcher.Execute(_allJobsQuery);
    }

    [GraphQLMetadata("job")]
    public async Task<JobDto> GetJob(Guid jobId)
    {
        _getJobQuery.JobId = new JobId(jobId);

        return await _queryDispatcher.Execute(_getJobQuery);
    }
}

