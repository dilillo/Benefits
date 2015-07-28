
namespace Benefits.Infrastructure.Events
{
    /// <summary>
    /// Represents an event fired when an Employee is created.
    /// </summary>
    public class EmployeeCreatedEvent : EmployeeEventBase
    {
        public EmployeeCreatedEvent()
            : base()
        {
            Name = EmployeeCreatedEventName;
        }

        public const string EmployeeCreatedEventName = "EmployeeCreated";
    }
}
