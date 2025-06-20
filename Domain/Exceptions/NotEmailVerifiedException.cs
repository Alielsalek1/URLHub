namespace URLshortner.Domain.Exceptions;

public class NotEmailVerifiedException : Exception
{
    public NotEmailVerifiedException() : base("Email not verified") { }

    public NotEmailVerifiedException(string message) : base(message) { }
}
