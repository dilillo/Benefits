
namespace Benefits.Infrastructure.Events
{
    /// <summary>
    /// Represents an event fired when an Employee is edited.
    /// </summary>
    public class EmployeeEditedEvent : EmployeeEventBase
    {
        public EmployeeEditedEvent()
            : base()
        {
            Name = EmployeeEditedEventName;
        }

        public const string EmployeeEditedEventName = "EmployeeEdited";
    }
}
