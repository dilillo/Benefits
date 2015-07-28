
namespace Benefits.Infrastructure.Commands
{
    /// <summary>
    /// Represents a command that should result in an updated employee.
    /// </summary>
    public class EmployeeEditCommand : EmployeeCommandBase
    {
        public EmployeeEditCommand()
        {
            Name = "EmployeeEdit";
        }
    }
}
