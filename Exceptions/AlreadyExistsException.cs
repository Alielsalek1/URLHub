namespace URLshortner.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException() : base("Cannot create again, Attribute already exists") { }

    public AlreadyExistsException(string message) : base(message) { }

    public AlreadyExistsException(string message, Exception innerException)
        : base(message, innerException) { }
}
