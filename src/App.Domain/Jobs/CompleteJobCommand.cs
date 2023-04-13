using FluentValidation;

namespace App.Domain;

public class CompleteJobCommand
{
    public JobId JobId { get; set; }
    public string ResultFile { get; set; }
}

public class CompleteJobCommandValidator : AbstractValidator<CompleteJobCommand>
{
    public CompleteJobCommandValidator()
    { }
}
