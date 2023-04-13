namespace App.Domain;

public interface ISystemClock
{
    DateTimeOffset UtcNow();
}

public class SystemClock : ISystemClock
{
    public DateTimeOffset UtcNow()
    {
        return DateTimeOffset.UtcNow;
    }
}
