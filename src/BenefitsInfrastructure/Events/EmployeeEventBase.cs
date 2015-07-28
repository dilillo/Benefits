using Benefits.Infrastructure.Models;

namespace Benefits.Infrastructure.Events
{
    /// <summary>
    /// Represents an event with an EmployeeModel payload.
    /// </summary>
    public abstract class EmployeeEventBase : MessageBase
    {
        public EmployeeModel Data { get; set; }
    }
}
