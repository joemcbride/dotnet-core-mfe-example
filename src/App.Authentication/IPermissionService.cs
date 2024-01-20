namespace App.Authentication;

public interface IPermissionService
{
    Task<string[]> GetPermissionAsync(string userId, string userType);
}

public class AlwaysAdminPermissionService : IPermissionService
{
    public Task<string[]> GetPermissionAsync(string userId, string userType)
    {
        return Task.FromResult(new string[] { PermissionValues.Admin });
    }
}
