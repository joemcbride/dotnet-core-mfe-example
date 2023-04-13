using FluentValidation;

namespace App.Domain;

public interface ICommandDispatcher
{
    Task Execute<T>(T command);
    Task<TResult> Execute<T, TResult>(T command);
}

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _services;

    public CommandDispatcher(IServiceProvider services)
    {
        _services = services;
    }

    public async Task Execute<TCommand>(TCommand command)
    {
        var handlerType = typeof(ICommandHandler<TCommand>);
        var handler = _services.GetService(handlerType) as ICommandHandler<TCommand>;

        if (handler == null)
            throw new ApplicationException($"There is no ICommandHandler registered for command {typeof(TCommand).Name}");

        var validator = _services.GetService(typeof(IValidator<TCommand>)) as IValidator<TCommand>;
        if (validator == null)
            throw new ApplicationException($"There is no IValidator registered for command {typeof(TCommand).Name}");

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            var errorString = string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BusinessRuleException(errorString);
        }

        await handler.Handle(command);
    }

    public async Task<TResult> Execute<TCommand, TResult>(TCommand command)
    {
        var handlerType = typeof(ICommandHandler<TCommand, TResult>);
        var handler = _services.GetService(handlerType) as ICommandHandler<TCommand, TResult>;

        if (handler == null)
            throw new ApplicationException($"There is no ICommandHandler registered for command {typeof(TCommand).Name} that returns {typeof(TResult).Name}");

        var validator = _services.GetService(typeof(IValidator<TCommand>)) as IValidator<TCommand>;
        if (validator == null)
            throw new ApplicationException($"There is no IValidator registered for command {typeof(TCommand).Name}");

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            var errorString = string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BusinessRuleException(errorString);
        }

        return await handler.Handle(command);
    }
}
