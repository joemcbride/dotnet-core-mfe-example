using App.Core;
using System.Security.Claims;

namespace App.Authentication;

public interface IAuthenticationClaimsBuilder
{
    Task<ClaimsPrincipal> BuildClaimsPrincipal(ClaimsPrincipal authPrincipal, string authenticationType);
}

public class AuthenticationClaimsBuilder : IAuthenticationClaimsBuilder
{
    readonly IPermissionService _permissionService;
    readonly IJsonSerializer _serializer;

    public AuthenticationClaimsBuilder(IPermissionService permissionService, IJsonSerializer serializer)
    {
        _permissionService = permissionService;
        _serializer = serializer;
    }

    public async Task<ClaimsPrincipal> BuildClaimsPrincipal(ClaimsPrincipal authPrincipal, string authenticationType)
    {
        var claims = authPrincipal.Claims.ToList();
        var identityValue = claims.FirstOrDefault(_ => _.Type == IdentityClaimTypes.Identity)?.Value;

        if (string.IsNullOrEmpty(identityValue))
        {
            return authPrincipal;
        }

        var identity = _serializer.Deserialize<IdentityTokenDto>(identityValue);
        var permissions = await _permissionService.GetPermissionAsync(identity.Id, identity.Role);

        foreach (var permission in permissions)
        {
            claims.Add(new Claim(IdentityClaimTypes.Permission, permission));
        }

        var claimsIdentity = new ClaimsIdentity(claims, authenticationType);
        return new ClaimsPrincipal(claimsIdentity);
    }
}
