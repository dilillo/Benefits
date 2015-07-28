namespace Benefits.CommandData
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents information required to validate employee commands can be performed.
    /// </summary>
    public partial class Employee
    {
        public Employee()
        {
            EmployeeEvents = new HashSet<EmployeeEvent>();
        }

        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<EmployeeEvent> EmployeeEvents { get; set; }
    }
}
