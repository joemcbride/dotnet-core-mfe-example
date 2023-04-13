namespace App.Domain;

public class CompleteJobCommandHandler : ICommandHandler<CompleteJobCommand>
{
    private readonly ISystemClock _clock;
    private readonly IJobRepository _jobRepository;

    public CompleteJobCommandHandler(ISystemClock clock, IJobRepository jobRepository)
    {
        _clock = clock;
        _jobRepository = jobRepository;
    }

    public async Task Handle(CompleteJobCommand command)
    {
        var job = await _jobRepository.GetById(command.JobId);

        if (job == null)
            throw new BusinessRuleException($"Could not find Job with Id {command.JobId.Value}");

        job.CompletedWithResult(command.ResultFile, _clock);

        await _jobRepository.Save(job);
    }
}
