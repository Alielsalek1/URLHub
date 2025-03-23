﻿namespace URLshortner.Exceptions;

public class InvalidInputException : Exception
{
    public InvalidInputException() : base("Invalid Credentials") { }

    public InvalidInputException(string message) : base(message) { }

    public InvalidInputException(string message, Exception innerException)
        : base(message, innerException) { }
}
