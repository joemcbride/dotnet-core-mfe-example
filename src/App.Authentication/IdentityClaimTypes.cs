using System.Security.Claims;

namespace App.Authentication;

public static class IdentityClaimTypes
{
    public const string Subject = ClaimTypes.NameIdentifier;
    public const string Role = ClaimTypes.Role;
    public const string Identity = "http://schemas.app.com/identity";
    public const string UserRole = "user";
    public const string EmployeeRole = "employee";
    public const string UserId = "UserId";
    public const string EmployeeId = "EmployeeId";
    public const string Permission = "permission";
}
