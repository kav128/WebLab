using System;

namespace lab2.Exceptions
{
    public class VkApiException : Exception
    {
        public VkApiException()
        {
        }

        public VkApiException(string? message) : base(message)
        {
        }

        public VkApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
