using System.Data.Entity;
using System.Threading.Tasks;

namespace Benefits.QueryData
{
    /// <summary>
    /// Defines a component used to read from and write to the read side data store.
    /// </summary>
    public interface IQueryDataModel
    {
        DbSet<DependentDetail> DependentDetails { get; }

        DbSet<EmployeeDetail> EmployeeDetails { get; }

        DbSet<Kpi> Kpis { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
