using System;

namespace URLshortner.Domain.Exceptions;
public class GoogleAuthFailedException : Exception
{
    public GoogleAuthFailedException() : base("Google authentication failed.")
    {
    }

    public GoogleAuthFailedException(string message) : base(message)
    {
    }
}
