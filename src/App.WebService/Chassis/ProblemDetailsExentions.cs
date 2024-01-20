namespace App.WebService.Chassis;

public static class ProblemDetailsExentions
{
    public static IServiceCollection AddProblemDetailsServices(this IServiceCollection services)
    {
        services.AddProblemDetails(_ =>
        {
            _.IncludeExceptionDetails = (ctx, ex) =>
            {
                var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                return env.IsDevelopment();
            };

            _.Map<App.Domain.ApplicationException>((ctx, ex) =>
            {
                return new ProblemDetails
                {
                    Title = "Application Exception",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = ex.Message
                };
            });

            _.Map<BusinessRuleException>((ctx, ex) =>
            {
                return new ProblemDetails
                {
                    Title = "Validation Exception",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = ex.Message
                };
            });
        });

        return services;
    }
}
