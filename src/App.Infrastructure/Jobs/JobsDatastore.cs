using App.Domain;

namespace App.Infrastructure;

public class JobsDatastore
{
    private readonly Dictionary<JobId, Job> _jobs = new Dictionary<JobId, Job>();

    public JobsDatastore()
    {
        var job = new Job(new JobId(Guid.NewGuid()), true, null, JobStatus.Created, DateTimeOffset.UtcNow, null);
        Save(job);
        job = new Job(new JobId(Guid.NewGuid()), true, "//test.csv", JobStatus.Completed, DateTimeOffset.UtcNow.AddHours(-2), DateTimeOffset.UtcNow.AddHours(-1));
        Save(job);
    }

    public IEnumerable<Job> All()
    {
        return _jobs.Values.ToList();
    }

    public Job Get(JobId jobId)
    {
        Job result = null;

        if (_jobs.TryGetValue(jobId, out Job value))
        {
            result = value;
        }

        return result;
    }

    public void Delete(JobId jobId)
    {
        if (_jobs.ContainsKey(jobId))
        {
            _jobs.Remove(jobId);
        }
    }

    public Job Save(Job job)
    {
        if (job.Id == JobId.Empty)
        {
            var newJob = new Job(new JobId(Guid.NewGuid()), job.DataFileOnly, job.ResultFile, job.Status, job.StartedAtTimeUtc, job.EndedAtTimeUtc);
            _jobs[newJob.Id] = newJob;
            return newJob;
        }

        _jobs[job.Id] = job;
        return job;
    }
}
