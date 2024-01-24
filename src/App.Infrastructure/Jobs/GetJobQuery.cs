using App.Domain;
using App.Infrastructure;

public class GetJobQuery : IQuery<JobDto>
{
    private readonly JobsDatastore datastore;

    public GetJobQuery(JobsDatastore datastore)
    {
        this.datastore = datastore;
    }

    public JobId JobId { get; set; }

    Task<JobDto> IQuery<JobDto>.Query(IDbConnectionFactory db)
    {
        var job = datastore.Get(JobId);
        return Task.FromResult(JobDto.From(job));
    }
}
