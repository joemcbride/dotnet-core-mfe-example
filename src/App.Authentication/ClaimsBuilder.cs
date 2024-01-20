using App.Core;
using System.Security.Claims;

namespace App.Authentication;

public class ClaimsBuilder
{
    private readonly IJsonSerializer _serializer;

    public ClaimsBuilder(IJsonSerializer serializer)
    {
        _serializer = serializer;
    }

    public IEnumerable<Claim> Build(
        string id,
        string role,
        string firstName,
        string lastName
    )
    {
        var claims = new List<Claim>();

        if (!string.IsNullOrEmpty(id))
        {
            var identityToken = new IdentityTokenDto
            {
                Id = id,
                Role = role,
                FirstName = firstName,
                LastName = lastName
            };
            claims.Add(new Claim(IdentityClaimTypes.Subject, $"{role}/{id}"));
            claims.Add(
                new Claim(IdentityClaimTypes.Identity, _serializer.Serialize(identityToken))
            );
        }

        if (role != null && role == IdentityClaimTypes.UserRole)
        {
            claims.Add(new Claim(IdentityClaimTypes.Role, IdentityClaimTypes.UserRole));
            claims.Add(new Claim(IdentityClaimTypes.UserId, id));
        }

        if (role != null && role == IdentityClaimTypes.EmployeeRole)
        {
            claims.Add(new Claim(IdentityClaimTypes.Role, IdentityClaimTypes.EmployeeRole));
            claims.Add(new Claim(IdentityClaimTypes.EmployeeId, id));
        }

        return claims;
    }
}
