namespace URLshortner.Exceptions;

public class UnAuthorizedException : Exception
{
    public UnAuthorizedException() : base("Attribute not found") { }

    public UnAuthorizedException(string message) : base(message) { }

    public UnAuthorizedException(string message, Exception innerException)
        : base(message, innerException) { }
}
