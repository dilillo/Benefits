using Benefits.Infrastructure.Models;

namespace Benefits.Infrastructure.Commands
{
    /// <summary>
    /// Represents a command with an EmployeeModel payload.
    /// </summary>
    public abstract class EmployeeCommandBase : MessageBase
    {
        public EmployeeModel Arg { get; set; }
    }
}
