using System;

namespace Benefits.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception to be thrown when a validtion problem is detected.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base (message)
        {
        }
    }
}
