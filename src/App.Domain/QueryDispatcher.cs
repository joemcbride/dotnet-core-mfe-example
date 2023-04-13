using FluentValidation;

namespace App.Domain;

public interface IQueryDispatcher
{
    Task<T> Execute<T>(IQuery<T> query);
}

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IDbConnectionFactory _db;
    private readonly IServiceProvider _services;

    public QueryDispatcher(IDbConnectionFactory db, IServiceProvider services)
    {
        _db = db;
        _services = services;
    }

    public Task<T> Execute<T>(IQuery<T> query)
    {
        var queryType = query.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(queryType);

        if (_services.GetService(validatorType) is IValidator validator)
        {
            var validationResult = validator.Validate(new ValidationContext<object>(query));

            if (!validationResult.IsValid)
            {
                var errorString = string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BusinessRuleException(errorString);
            }
        }

        return query.Query(_db);
    }
}
