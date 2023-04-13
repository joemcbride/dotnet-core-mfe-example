using App.Domain;
using App.Website.Schema;
using GraphQL;

namespace App.Website.Jobs;

[GraphQLMetadata("Mutation")]
[GraphQLSchemaMetadataAttribute("")]
public class MutationGraphType
{
    private readonly ICommandDispatcher _commandDispatcher;

    public MutationGraphType(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    public async Task<JobDto> CreateJob(CreateJobInputDto input)
    {
        var command = new CreateJobCommand
        {
            DataFileOnly = input.DataFileOnly
        };

        var result = await _commandDispatcher.Execute<CreateJobCommand, JobDto>(command);
        return result;
    }
}

[GraphQLMetadata("CreateJobInput")]
public class CreateJobInputDto
{
    public bool DataFileOnly { get; set; }
}
