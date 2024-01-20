using System.Text.Json.Serialization;

namespace App.Authentication;

public class IdentityTokenDto
{
    public string Id { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [JsonIgnore]
    public bool IsUser => Role?.ToLower() == IdentityClaimTypes.UserRole;
}
