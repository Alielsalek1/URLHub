namespace URLshortner.Domain.Exceptions;

public class UnAuthorizedException : Exception
{
    public UnAuthorizedException() : base("Unauthorized access") { }

    public UnAuthorizedException(string message) : base(message) { }
}
