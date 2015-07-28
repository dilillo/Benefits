namespace Benefits.CommandData
{
    using System.Data.Entity;

    /// <summary>
    /// DbContext derrived class for interacting with the tables in the write side of the data store.
    /// </summary>
    public partial class CommandDataModel : DbContext, ICommandDataModel
    {
        public CommandDataModel()
            : base("name=CommandDataModel")
        {
        }

        public virtual DbSet<EmployeeEvent> EmployeeEvents { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeEvent>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeEvent>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeEvent>()
                .Property(e => e.EmployeeId)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeEvents)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);
        }
    }
}
