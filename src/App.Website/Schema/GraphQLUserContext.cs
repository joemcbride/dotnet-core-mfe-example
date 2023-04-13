using System.Security.Claims;

namespace App.Website.Schema;

public class GraphQLUserContext : Dictionary<string, object>
{
    public ClaimsPrincipal User { get; set; }
}
