using System.Data;

namespace App.Domain;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetConnection();
}
