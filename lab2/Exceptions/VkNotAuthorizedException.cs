using System;

namespace lab2.Exceptions
{
    public class VkNotAuthorizedException : Exception
    {
        public VkNotAuthorizedException()
        {
        }

        public VkNotAuthorizedException(string? message) : base(message)
        {
        }

        public VkNotAuthorizedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
