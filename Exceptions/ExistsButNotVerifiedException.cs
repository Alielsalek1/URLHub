namespace URLshortner.Exceptions;

public class ExistsButNotVerifiedException : Exception
{
    public ExistsButNotVerifiedException() : base("Email Exists but required verification") { }

    public ExistsButNotVerifiedException(string message) : base(message) { }
}
