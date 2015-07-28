using Benefits.Infrastructure.Commands;
using Benefits.QueryBiz;
using BenefitsWeb.Filters;
using BenefitsWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BenefitsWeb.Controllers
{
    /// <summary>
    /// Exposes methods for reading and writing employee information.
    /// </summary>
    [HandleException]
    public class EmployeesApiController : ApiController
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="queries">IQueries implementation to use</param>
        /// <param name="commandBus">ICommandBus implementation to use</param>
        public EmployeesApiController(IQueries queries, ICommandBus commandBus)
        {
            _queries = queries;
            _commandBus = commandBus;
        }

        //internal state
        readonly IQueries _queries;
        readonly ICommandBus _commandBus;
        
        /// <summary>
        /// Gets all of the non-deleted employees in the system.
        /// </summary>
        /// <returns>Set of employees</returns>
        public IEnumerable<EmployeeSummaryViewModel> Get()
        {
            return _queries.GetAllEmployees().Select(i => EmployeeSummaryViewModel.FromData(i));
        }

        /// <summary>
        /// Gets a employee by their unique id.
        /// </summary>
        /// <param name="id">Id to query with</param>
        /// <returns>Corresponding EmployeeDetailViewModel instance or null if not found</returns>
        public EmployeeDetailViewModel Get(string id)
        {
            return EmployeeDetailViewModel.FromQueryModel(_queries.GetEmployeeById(id));
        }

        /// <summary>
        /// Attempts to create a new employee in the data store.
        /// </summary>
        /// <param name="value">EmployeeDetailViewModel instance reprenting the employee to create</param>
        /// <returns>Created employee</returns>
        public EmployeeDetailViewModel Post([FromBody]EmployeeDetailViewModel value)
        {
            var cmd = new EmployeeCreateCommand() { Arg = EmployeeDetailViewModel.ToDomainModel(value) };

            _commandBus.Execute(cmd);

            value.Id = cmd.Arg.Id;
            value.Version = cmd.Arg.Version;

            return value;
        }

        /// <summary>
        /// Attempts update a employee in the data store.
        /// </summary>
        /// <param name="id">id of the employee</param>
        /// <param name="value">EmployeeDetailViewModel instance reprenting the employee to update</param>
        public void Put(string id, [FromBody]EmployeeDetailViewModel value)
        {
            var empVm = EmployeeDetailViewModel.FromQueryModel(_queries.GetEmployeeById(value.Id));

            empVm.Name = value.Name;
            empVm.Version = value.Version;

            _commandBus.Execute(new EmployeeEditCommand() { Arg = EmployeeDetailViewModel.ToDomainModel(empVm) });
        }

        /// <summary>
        /// Attempts to delete a employee from the data store.
        /// </summary>
        /// <param name="id">id of the employee</param>
        /// <param name="version">version of the employee this change is based on</param>
        public void Delete(string id)
        {
            var empVm = EmployeeDetailViewModel.FromQueryModel(_queries.GetEmployeeById(id));

            _commandBus.Execute(new EmployeeDeleteCommand() { Arg = EmployeeDetailViewModel.ToDomainModel(empVm) });
        }
    }
}
