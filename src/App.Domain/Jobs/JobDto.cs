namespace App.Domain;

public class JobDto
{
    public Guid JobId { get; set; }
    public string Status { get; set; }
    public string ResultFile { get; set; }
    public DateTimeOffset StartedAtTimeUtc { get; set; }
    public DateTimeOffset? EndedAtTimeUtc { get; set; }

    public static JobDto From(Job job)
    {
        return new JobDto
        {
            JobId = job.Id.Value,
            Status = job.Status.ToString(),
            ResultFile = job.ResultFile,
            StartedAtTimeUtc = job.StartedAtTimeUtc,
            EndedAtTimeUtc = job.EndedAtTimeUtc
        };
    }
}
