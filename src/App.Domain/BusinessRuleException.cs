namespace App.Domain;

public class ApplicationException : Exception
{
    public ApplicationException(string message)
        : base(message)
    { }
}

public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message)
        : base(message)
    { }
}
