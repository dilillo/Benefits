
namespace Benefits.Infrastructure.Events
{
    /// <summary>
    /// Represents an event fired when an Employee is deleted.
    /// </summary>
    public class EmployeeDeletedEvent : EmployeeEventBase
    {
        public EmployeeDeletedEvent()
            : base()
        {
            Name = EmployeeDeletedEventName;
        }

        public const string EmployeeDeletedEventName = "EmployeeDeleted";
    }
}
