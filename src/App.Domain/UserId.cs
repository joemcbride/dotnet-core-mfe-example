namespace App.Domain;

public class UserId : IdentityValueObject<Guid>
{
    public static readonly UserId Empty = new UserId(Guid.Empty);

    public UserId(Guid value) : base(value)
    {
    }
}
