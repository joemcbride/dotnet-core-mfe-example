namespace App.Domain;

public class JobId : IdentityValueObject<Guid>
{
    public static readonly JobId Empty = new JobId(Guid.Empty);

    public JobId(Guid value) : base(value)
    {
    }
}
