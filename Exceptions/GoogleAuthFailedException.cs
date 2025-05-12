using System;

namespace URLshortner.Exceptions;
public class GoogleAuthFailedException : Exception
{
    public GoogleAuthFailedException() : base("Google authentication failed.")
    {
    }

    public GoogleAuthFailedException(string message) : base(message)
    {
    }
}
