namespace Benefits.CommandData
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a single processing event for a given employee.
    /// </summary>
    public partial class EmployeeEvent
    {
        [StringLength(50)]
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        public byte Version { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeId { get; set; }

        [Required]
        public string Data { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
