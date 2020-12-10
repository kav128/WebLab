using System;

namespace lab2.Exceptions
{
    public class UnexpectedErrorException : Exception
    {
        public UnexpectedErrorException()
        {
        }

        public UnexpectedErrorException(string? message) : base(message)
        {
        }

        public UnexpectedErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
