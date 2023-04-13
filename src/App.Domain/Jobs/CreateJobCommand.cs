using FluentValidation;

namespace App.Domain;

public class CreateJobCommand
{
    public bool DataFileOnly { get; set; } = true;
}

public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    { }
}
