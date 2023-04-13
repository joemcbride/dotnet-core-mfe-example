namespace App.Domain;

public interface IQuery<TResult>
{
    Task<TResult> Query(IDbConnectionFactory db);
}
