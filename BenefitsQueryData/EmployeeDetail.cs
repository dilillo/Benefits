namespace Benefits.QueryData
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents an employee in the read side data store.
    /// </summary>
    public partial class EmployeeDetail
    {
        public EmployeeDetail()
        {
            DependentDetails = new HashSet<DependentDetail>();
        }

        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public short Version { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName = "money")]
        public decimal GrossPay { get; set; }

        [Column(TypeName = "money")]
        public decimal Benefits { get; set; }

        [Column(TypeName = "money")]
        public decimal NetPay { get; set; }

        public byte Dependents { get; set; }

        public virtual ICollection<DependentDetail> DependentDetails { get; set; }
    }
}
