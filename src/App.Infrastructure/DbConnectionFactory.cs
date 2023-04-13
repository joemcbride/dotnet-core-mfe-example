using System.Data;
using App.Domain;

namespace App.Infrastructure;

public class DbConnectionFactory : IDbConnectionFactory
{
    public Task<IDbConnection> GetConnection()
    {
        return null;
    }
}
