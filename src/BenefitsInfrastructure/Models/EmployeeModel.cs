
namespace Benefits.Infrastructure.Models
{
    /// <summary>
    /// Represents the state of an employee.
    /// </summary>
    public class EmployeeModel
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EmployeeModel()
        {
            Dependents = new DependentModel[] { };
        }

        /// <summary>
        /// Unique identifier of the employee.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the employee.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Version number associated with the present state of the employee in the write side data store.
        /// </summary>
        public short Version { get; set; }

        /// <summary>
        /// Indicates whether or not the employee has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Pay due to employee before cost of benefits has been subtracted.
        /// </summary>
        public decimal GrossPay { get; set; }

        /// <summary>
        /// Cost of benefits for the employee.
        /// </summary>
        public decimal Benefits { get; set; }

        /// <summary>
        /// Pay due to employee after the cost of benefits has been subtracted.
        /// </summary>
        public decimal NetPay { get; set; }

        /// <summary>
        /// Set of dependents associated with the employee.
        /// </summary>
        public DependentModel[] Dependents { get; set; }
    }
}
