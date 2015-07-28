namespace Benefits.QueryData
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a dependent in the read side data store.
    /// </summary>
    public partial class DependentDetail
    {
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual EmployeeDetail EmployeeDetail { get; set; }
    }
}
