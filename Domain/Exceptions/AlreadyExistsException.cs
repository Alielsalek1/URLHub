namespace URLshortner.Domain.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException() : base("Cannot create again, Attribute already exists") { }

    public AlreadyExistsException(string message) : base(message) { }
}
