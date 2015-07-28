
namespace Benefits.Infrastructure.Models
{
    /// <summary>
    /// Represents the state of a dependent.
    /// </summary>
    public class DependentModel
    {
        /// <summary>
        /// Unique identifier of the dependent.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique identifier of the employee to which the dependent is associated.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Name of the depedent.
        /// </summary>
        public string Name { get; set; }
    }
}
