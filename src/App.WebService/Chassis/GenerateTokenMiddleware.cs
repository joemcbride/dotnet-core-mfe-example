using App.Authentication;
using App.Infrastructure;
using System.Security.Claims;
using System.Text;

namespace App.WebService.Chassis;

public class GenerateTokenMiddleware
{
    readonly JwtOptions _options;
    readonly IJwtTokenGenerator _generator;
    readonly ClaimsBuilder _claimsBuilder;
    readonly RequestDelegate _next;

    public GenerateTokenMiddleware(
        JwtOptions options,
        IJwtTokenGenerator generator,
        ClaimsBuilder claimsBuilder,
        RequestDelegate next
    )
    {
        _options = options;
        _generator = generator;
        _claimsBuilder = claimsBuilder;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var id = context.Request.Query["id"].FirstOrDefault();
        var role =
            context.Request.Query["role"].FirstOrDefault()?.ToLower()
            ?? IdentityClaimTypes.UserRole;
        var firstName = context.Request.Query["firstName"].FirstOrDefault();
        var lastName = context.Request.Query["lastName"].FirstOrDefault();

        var validRoles = new[] { IdentityClaimTypes.UserRole, IdentityClaimTypes.EmployeeRole };

        var errors = new List<string>();

        if (string.IsNullOrEmpty(id))
        {
            errors.Add("An id for the user or employee is required.");
        }

        if (!validRoles.Contains(role))
        {
            errors.Add($"Invalid role. Must be one of {string.Join(", ", validRoles)}");
        }

        if (errors.Any())
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(errors.Aggregate((a, b) => $"{a}\n{b}"));
            return;
        }

        var (token, principal) = BuildPrincipal(id, role, firstName, lastName);
        var html = BuildOutputHtml(token, principal);

        context.Response.StatusCode = 200;
        await context.Response.WriteAsync(html);
    }

    private (string, ClaimsPrincipal) BuildPrincipal(
        string id,
        string role,
        string firstName,
        string lastName
    )
    {
        var claims = _claimsBuilder.Build(
            id,
            role,
            firstName,
            lastName
        );

        var token = _generator.GenerateToken(claims, _options);
        var principal = _generator.ValidateToken(token, _options);

        return (token, principal);
    }

    private string BuildOutputHtml(string token, ClaimsPrincipal principal)
    {
        var builder = new StringBuilder();
        builder.Append("<div><b>Token</b></div>");
        builder.AppendLine($"<div><code style=\"overflow-wrap: break-word;\">{token}</code></div>");
        builder.AppendLine("<br/>");

        builder.Append("<div><b>Claims</b></div>");
        foreach (var claim in principal.Claims)
        {
            builder.AppendLine($"<div>{claim.Type}: {claim.Value}</div>");
        }

        return builder.ToString();
    }
}
