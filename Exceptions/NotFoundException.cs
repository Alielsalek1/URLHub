namespace URLshortner.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Attribute not found") { }

    public NotFoundException(string message) : base(message) { }
}
