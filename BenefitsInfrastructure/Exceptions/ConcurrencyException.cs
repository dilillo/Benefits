using System;

namespace Benefits.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception to be thrown when a concurrency problem is detected.
    /// </summary>
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException() : base("Oops!  Looks like someone beat you to it.  Please refresh and try again.")
        {
        }
    }
}
