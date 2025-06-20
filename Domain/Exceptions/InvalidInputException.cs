namespace URLshortner.Domain.Exceptions;

public class InvalidInputException : Exception
{
    public InvalidInputException() : base("Invalid Credentials") { }

    public InvalidInputException(string message) : base(message) { }
}
