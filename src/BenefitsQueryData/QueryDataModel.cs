namespace Benefits.QueryData
{
    using System.Data.Entity;

    /// <summary>
    /// DbContext derrived class for interacting with the tables in the read side of the data store.
    /// </summary>
    public partial class QueryDataModel : DbContext, IQueryDataModel
    {
        public QueryDataModel()
            : base("name=QueryDataModel")
        {
        }

        public virtual DbSet<DependentDetail> DependentDetails { get; set; }
        public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual DbSet<Kpi> Kpis { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DependentDetail>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<DependentDetail>()
                .Property(e => e.EmployeeId)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeDetail>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeDetail>()
                .Property(e => e.GrossPay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EmployeeDetail>()
                .Property(e => e.Benefits)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EmployeeDetail>()
                .Property(e => e.NetPay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EmployeeDetail>()
                .HasMany(e => e.DependentDetails)
                .WithRequired(e => e.EmployeeDetail)
                .HasForeignKey(e => e.EmployeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kpi>()
                .Property(e => e.Id)
                .IsUnicode(false);
        }
    }
}
