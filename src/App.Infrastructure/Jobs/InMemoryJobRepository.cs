using App.Domain;

namespace App.Infrastructure;

public class InMemoryJobRepository : IJobRepository
{
    private readonly JobsDatastore _db;

    public InMemoryJobRepository(JobsDatastore db)
    {
        _db = db;
    }

    public Task Delete(JobId jobId)
    {
        _db.Delete(jobId);
        return Task.CompletedTask;
    }

    public Task<Job> GetById(JobId id)
    {
        return Task.FromResult(_db.Get(id));
    }

    public Task<Job> Save(Job job)
    {
        var result = _db.Save(job);
        return Task.FromResult(result);
    }
}
