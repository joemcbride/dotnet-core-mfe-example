using App.Authentication;
using App.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace App.WebService.Chassis;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        JwtOptions options
    )
    {
        services
            .AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(_ =>
            {
                _.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    ValidAudience = options.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(options.SecretKey)
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };

                if (_.Events == null)
                {
                    _.Events = new JwtBearerEvents();
                }

                _.Events.OnTokenValidated = async context =>
                {
                    var claimsBuilder = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationClaimsBuilder>();
                    var principal = await claimsBuilder.BuildClaimsPrincipal(context.Principal, context.Scheme.Name);
                    context.Principal = principal;
                };
            });

        return services;
    }
}
