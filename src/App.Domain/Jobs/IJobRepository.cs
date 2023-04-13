namespace App.Domain;

public interface IJobRepository
{
    Task<Job> GetById(JobId id);
    Task Save(Job job);
    Task Delete(JobId jobId);
}
