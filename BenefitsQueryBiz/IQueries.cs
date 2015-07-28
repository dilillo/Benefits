using Benefits.QueryData;
using System.Collections.Generic;

namespace Benefits.QueryBiz
{
    /// <summary>
    /// Defines a component that can query the write side of the data store for employee information.
    /// </summary>
    public interface IQueries
    {
        /// <summary>
        /// Gets all non-deleted employees.
        /// </summary>
        /// <returns>All non-deleted employees</returns>
        IEnumerable<EmployeeDetail> GetAllEmployees();

        /// <summary>
        /// Gets all Key Performance Indicators.
        /// </summary>
        /// <returns>All key performance indicators</returns>
        IEnumerable<Kpi> GetAllKpis();

        /// <summary>
        /// Gets a dependent and the associated employee by the id of the dependent.
        /// </summary>
        /// <param name="id">id of the dependent</param>
        /// <returns>Corresponding dependent details or null if not found</returns>
        DependentDetail GetDependentById(string id);

        /// <summary>
        /// Gets an employee by id.
        /// </summary>
        /// <param name="id">id of the employee</param>
        /// <returns>Corresponding employee details or null if not found</returns>
        EmployeeDetail GetEmployeeById(string id);
    }
}
