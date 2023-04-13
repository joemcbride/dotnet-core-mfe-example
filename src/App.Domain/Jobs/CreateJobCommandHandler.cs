namespace App.Domain;

public class CreateJobCommandHandler : ICommandHandler<CreateJobCommand>
{
    private readonly ISystemClock _clock;
    private readonly IJobRepository _jobRepository;

    public CreateJobCommandHandler(ISystemClock clock, IJobRepository jobRepository)
    {
        _clock = clock;
        _jobRepository = jobRepository;
    }

    public async Task Handle(CreateJobCommand command)
    {
        var newJob = new Job(JobId.Empty, command.DataFileOnly, null, JobStatus.Created, _clock.UtcNow(), null);
        await _jobRepository.Save(newJob);
    }
}
