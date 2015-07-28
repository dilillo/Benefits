namespace Benefits.QueryData
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a key performance indicator in the read side data store.
    /// </summary>
    public partial class Kpi
    {
        [StringLength(50)]
        public string Id { get; set; }

        public int Value { get; set; }
    }
}
