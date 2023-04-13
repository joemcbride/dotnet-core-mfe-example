namespace App.Domain;

public interface IJobRepository
{
    Task<Job> GetById(JobId id);
    Task<Job> Save(Job job);
    Task Delete(JobId jobId);
}
