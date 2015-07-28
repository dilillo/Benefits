
namespace Benefits.Infrastructure.Commands
{
    /// <summary>
    /// Represents a command that should result in a deleted employee.
    /// </summary>
    public class EmployeeDeleteCommand : EmployeeCommandBase
    {
        public EmployeeDeleteCommand()
        {
            Name = "EmployeeDelete";
        }
    }
}
