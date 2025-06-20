namespace URLshortner.Domain.Exceptions;

public class FailedToSendEmailException : Exception
{
    public FailedToSendEmailException() : base("Failed To send Email") { }

    public FailedToSendEmailException(string message) : base(message) { }
}
