using App.Domain;

namespace App.Infrastructure;

public class AllJobsQuery : IQuery<IEnumerable<JobDto>>
{
    private readonly JobsDatastore jobsDatastore;

    public AllJobsQuery(JobsDatastore jobsDatastore)
    {
        this.jobsDatastore = jobsDatastore;
    }

    Task<IEnumerable<JobDto>> IQuery<IEnumerable<JobDto>>.Query(IDbConnectionFactory db)
    {
        IEnumerable<JobDto> jobs = jobsDatastore.All()
            .Select(JobDto.From)
            .ToList();

        return Task.FromResult(jobs);
    }
}
