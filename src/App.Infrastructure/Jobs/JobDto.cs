namespace App.Infrastructure;

public class JobDto
{
    public Guid JobId { get; set; }
    public string Status { get; set; }
    public string ResultFile { get; set; }
    public DateTimeOffset StartedAtTimeUtc { get; set; }
    public DateTimeOffset? EndedAtTimeUtc { get; set; }
}
