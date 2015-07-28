using System.Data.Entity;
using System.Threading.Tasks;

namespace Benefits.CommandData
{
    /// <summary>
    /// Defines a component that can query and persist data in the write data store.
    /// </summary>
    public interface ICommandDataModel
    {
        DbSet<EmployeeEvent> EmployeeEvents { get; }

        DbSet<Employee> Employees { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
