using App.Infrastructure;

namespace App.WebService.Chassis;

public static class TokenGeneratorMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenGeneratorMiddleware(this IApplicationBuilder app, JwtOptions options)
    {
        app.Map(
            "/createtoken",
            api =>
            {
                api.UseMiddleware<GenerateTokenMiddleware>(options);
            }
        );

        return app;
    }
}
