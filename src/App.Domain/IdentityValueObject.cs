namespace App.Domain;

public class IdentityValueObject<T> : ValueObject
{
    public IdentityValueObject(T value)
    {
        Value = value;
    }

    public T Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
