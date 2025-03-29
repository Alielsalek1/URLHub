namespace URLshortner.Exceptions;

public class AlreadyVerifiedException : Exception
{
    public AlreadyVerifiedException() : base("user is already verified") { }

    public AlreadyVerifiedException(string message) : base(message) { }
}
