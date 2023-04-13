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

    public JobsGraphType(
        IQueryDispatcher queryDispatcher,
        AllJobsQuery query)
    {
        _queryDispatcher = queryDispatcher;
        _allJobsQuery = query;
    }

    public Task<IEnumerable<JobDto>> All()
    {
        return _queryDispatcher.Execute(_allJobsQuery);
    }
}

