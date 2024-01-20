using App.Authentication;

namespace App.WebService.Chassis;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization(_ =>
        {
            _.AddPolicy(PolicyNames.Authenticated, policy => policy.RequireAuthenticatedUser());

            _.AddPolicy(
                PolicyNames.ViewJobs,
                policy =>
                    policy.RequireClaim(
                        IdentityClaimTypes.Permission,
                        JobPermissionValues.ViewJobs,
                        PermissionValues.Admin
                    )
            );
        });

        return services;
    }
}

public static class PolicyNames
{
    public const string Authenticated = "AuthenticatedUser";
    public const string ViewJobs = "View:Jobs";
}

public static class JobPermissionValues
{
    public const string ViewJobs = "View:Jobs";
}
