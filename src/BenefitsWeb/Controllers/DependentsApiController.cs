using Benefits.Infrastructure.Commands;
using Benefits.QueryBiz;
using BenefitsWeb.Filters;
using BenefitsWeb.Models;
using System.Linq;
using System.Web.Http;

namespace BenefitsWeb.Controllers
{
    /// <summary>
    /// Exposes methods for reading and writing dependent information.
    /// </summary>
    [HandleException]
    public class DependentsApiController : ApiController
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="queries">IQueries implementation to use</param>
        /// <param name="commandBus">ICommandBus implementation to use</param>
        public DependentsApiController(IQueries queries, ICommandBus commandBus)
        {
            _queries = queries;
            _commandBus = commandBus;
        }

        //internal state
        readonly IQueries _queries;
        readonly ICommandBus _commandBus;

        /// <summary>
        /// Gets a dependent by their unique id.
        /// </summary>
        /// <param name="id">Id to query with</param>
        /// <returns>Corresponding DependentViewModel instance or null if not found</returns>
        public DependentViewModel Get(string id)
        {
            return DependentViewModel.FromQueryModel(_queries.GetDependentById(id));
        }

        /// <summary>
        /// Attempts to create a new dependent in the data store.
        /// </summary>
        /// <param name="value">DependentViewModel instance reprenting the dependent to create</param>
        /// <returns>Created depenent</returns>
        public DependentViewModel Post([FromBody]DependentViewModel value)
        {
            var empVm = EmployeeDetailViewModel.FromQueryModel(_queries.GetEmployeeById(value.EmployeeId));

            empVm.Dependents = empVm.Dependents.Concat(new DependentViewModel[] { value });

            _commandBus.Execute(new EmployeeEditCommand() { Arg = EmployeeDetailViewModel.ToDomainModel(empVm) });

            return value;
        }

        /// <summary>
        /// Attempts update a dependent in the data store.
        /// </summary>
        /// <param name="id">id of the dependent</param>
        /// <param name="value">DependentViewModel instance reprenting the dependent to update</param>
        public void Put(string id, [FromBody]DependentViewModel value)
        {
            var empVm = EmployeeDetailViewModel.FromQueryModel(_queries.GetEmployeeById(value.EmployeeId));

            empVm.Version = value.EmployeeVersion;
            empVm.Dependents = empVm.Dependents.Where(i => i.Id != value.Id).Concat(new DependentViewModel[] { value });

            _commandBus.Execute(new EmployeeEditCommand() { Arg = EmployeeDetailViewModel.ToDomainModel(empVm) });
        }

        /// <summary>
        /// Attempts to delete a dependent from the data store.
        /// </summary>
        /// <param name="id">id of the dependent</param>
        /// <param name="version">version of the employee this change is based on</param>
        public void Delete(string id, short version)
        {
            var empVm = EmployeeDetailViewModel.FromQueryModel(_queries.GetDependentById(id).EmployeeDetail);

            empVm.Version = version;
            empVm.Dependents = empVm.Dependents.Where(i => i.Id != id);

            _commandBus.Execute(new EmployeeEditCommand() { Arg = EmployeeDetailViewModel.ToDomainModel(empVm) });
        }
    }
}
