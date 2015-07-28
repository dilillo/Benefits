using Benefits.CommandData;
using Benefits.Infrastructure.Events;
using Newtonsoft.Json;

namespace Benefits.CommandBiz
{
    /// <summary>
    /// Container for EmployeeEventBase extension methods.
    /// </summary>
    public static class EmployeeEventBaseExtensionMethods
    {
        /// <summary>
        /// Converts an instance of EmployeeEventBase to an instance of EmployeeEvent.
        /// </summary>
        /// <param name="value">EmployeeEventBase instance to convert</param>
        /// <returns>EmployeeEvent instance corresponding to the EmployeeEventBase instance passed in</returns>
        public static EmployeeEvent ToEmployeeEvent(this EmployeeEventBase value)
        {
            return new EmployeeEvent()
            {
                Id = value.Id,
                Data = JsonConvert.SerializeObject(value.Data),
                Name = value.Name,
                EmployeeId = value.Data.Id,
                Timestamp = value.Timestamp,
                Version = value.Version
            };
        }
    }
}
