namespace App.Domain;

public class Job
{
    public Job(
        JobId id,
        bool dataFileOnly,
        string resultFile,
        JobStatus status,
        DateTimeOffset startedAtTimeUtc,
        DateTimeOffset? endedAtTimeUtc)
    {
        Id = id;
        DataFileOnly = dataFileOnly;
        ResultFile = resultFile;
        Status = status;
        StartedAtTimeUtc = startedAtTimeUtc;
        EndedAtTimeUtc = endedAtTimeUtc;
    }

    public JobId Id { get; }
    public bool DataFileOnly { get; }
    public string ResultFile { get; private set; }
    public JobStatus Status { get; private set; }
    public DateTimeOffset StartedAtTimeUtc { get; }
    public DateTimeOffset? EndedAtTimeUtc { get; private set; }

    public void CompletedWithResult(string resultFile, ISystemClock clock)
    {
        Status = JobStatus.Completed;
        ResultFile = resultFile;
        EndedAtTimeUtc = clock.UtcNow();
    }
}
