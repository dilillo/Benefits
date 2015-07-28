
namespace Benefits.Infrastructure.Commands
{
    /// <summary>
    /// Represents a command that should result in a created employee.
    /// </summary>
    public class EmployeeCreateCommand : EmployeeCommandBase
    {
        public EmployeeCreateCommand()
        {
            Name = "EmployeeCreate";
        }
    }
}
