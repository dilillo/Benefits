using Benefits.CommandData;
using Benefits.Infrastructure;
using Benefits.Infrastructure.Commands;
using Benefits.Infrastructure.Events;
using Benefits.Infrastructure.Exceptions;
using Benefits.Infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benefits.CommandBiz
{
    /// <summary>
    /// Component responsible for business operations related to employees and dependents.
    /// </summary>
    /// <remarks>
    /// This class agregates employee events and handles employee related commands.
    /// </remarks>
    public class EmployeeAggregate :
        ICommandHandler<EmployeeCreateCommand>,
        ICommandHandler<EmployeeEditCommand>,
        ICommandHandler<EmployeeDeleteCommand>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="dataModel">Instance of ICommandDataModel to be used to persist data</param>
        public EmployeeAggregate(ICommandDataModel dataModel)
        {
            _dataModel = dataModel;
        }

        //known constants
        const decimal PerPayBenefitsCost = 1000 / 26;
        const decimal PerPayDependentBenefitsCost = 500 / 26;
        const decimal NameStartsWithADiscount = 0.9M;
        const decimal GrossPay = 2000M;

        //internal state
        ICommandDataModel _dataModel;

        string _name;
        string _id;
        short _version;
        bool _isDeleted;
        bool _isDefined;
        decimal _grossPay;
        decimal _benefits;
        decimal _netPay;
        
        List<DependentModel> _depdendents;

        List<EmployeeEvent> _events;

        /// <summary>
        /// Initializes the event sourced state of the aggregate.
        /// </summary>
        /// <param name="id">id of employee who's events should be aggregated</param>
        void Initialize(string id = null)
        {
            //make sure this only happens once
            if (_events != null)
                return;

            _id = id;
            _events = new List<EmployeeEvent>();

            //if not new then get previously applied events from data store and apply them (existing employees only)
            if (!string.IsNullOrEmpty(_id))
                foreach (var @event in _dataModel.EmployeeEvents.Where(i => i.EmployeeId == _id).OrderBy(i => i.Timestamp))
                    ApplyEvent(@event);
        }

        /// <summary>
        /// Updates internal state from the state associated to the event.
        /// </summary>
        /// <param name="event">EmployeeEvent instance to pull state values from</param>
        void ApplyEvent(EmployeeEvent @event)
        {
            switch (@event.Name)
            {
                case EmployeeCreatedEvent.EmployeeCreatedEventName:
                case EmployeeEditedEvent.EmployeeEditedEventName:

                    var model = JsonConvert.DeserializeObject<EmployeeModel>(@event.Data);

                    _name = model.Name;
                    _version = model.Version;
                    _grossPay = model.GrossPay;
                    _benefits = model.Benefits;
                    _netPay = model.NetPay;
                    _depdendents = model.Dependents.ToList();
                    _isDefined = true;

                    break;

                case EmployeeDeletedEvent.EmployeeDeletedEventName:

                    _isDeleted = true;

                    break;
            }

            _events.Add(@event);
        }

        /// <summary>
        /// Calculates the cost of benefits for an employee given their name and the names of their dependents.
        /// </summary>
        /// <param name="name">Name of the employee</param>
        /// <param name="dependentNames">Names of the employee's dependents</param>
        /// <returns>Cost of benefits</returns>
        static decimal CalculateBenefits(string name, IEnumerable<string> dependentNames)
        {
            return ApplyNameStartsWithADiscount(name, PerPayBenefitsCost) +
                dependentNames.Sum(i => ApplyNameStartsWithADiscount(name, PerPayDependentBenefitsCost));
        }

        /// <summary>
        /// Applies a discount to the cost of an employee or dependent's benifits based on their name.
        /// </summary>
        /// <param name="name">Name of employee or dependent</param>
        /// <param name="benefits">Unadjusted benefits cost</param>
        /// <returns>Adjusted benefits cost</returns>
        static decimal ApplyNameStartsWithADiscount(string name, decimal benefits)
        {
            return name.StartsWith("A", StringComparison.CurrentCultureIgnoreCase) ? benefits * NameStartsWithADiscount : benefits;
        }

        /// <summary>
        /// Calculates an employee's net pay given the cost of their benifits.
        /// </summary>
        /// <param name="benefits">Cost of employee's benefits</param>
        /// <returns>Employee's net pay</returns>
        static decimal CalculateNetPay(decimal benefits)
        {
            return GrossPay - benefits;
        }
        
        /// <summary>
        /// Calculates the gross, benefits cost, and net pay values for an employee.
        /// </summary>
        /// <param name="model">EmployeeModel instance to update</param>
        static void CalculatePayValues(EmployeeModel model)
        {
            model.GrossPay = GrossPay;
            model.Benefits = CalculateBenefits(model.Name, model.Dependents.Select(i => i.Name));
            model.NetPay = CalculateNetPay(model.Benefits);
        }

        /// <summary>
        /// Validates the employee has not been deleted.
        /// </summary>
        void ValidateEmployeeNotDeleted()
        {
            if (_isDeleted)
                throw new ValidationException("Employee has been deleted.");
        }

        /// <summary>
        /// Validates the employee has been defined.
        /// </summary>
        void ValidateEmployeeDefined()
        {
            if (!_isDefined)
                throw new ValidationException("Employee does not exist.");
        }

        /// <summary>
        /// Ensures employee dependents have been propery associated and assigned an id value.
        /// </summary>
        /// <param name="model">EmployeeModel to check</param>
        static void NormalizeDependents(EmployeeModel model)
        {
            foreach (var dependent in model.Dependents)
            {
                dependent.Id = dependent.Id ?? Guid.NewGuid().ToString();
                dependent.EmployeeId = model.Id;
            }
        }

        /// <summary>
        /// Handles an employee create command.
        /// </summary>
        /// <param name="command">EmployeeCreateCommand instance representing the work to be done</param>
        /// <returns>EmployeeCreatedEvent if successful</returns>
        public IEnumerable<MessageBase> Handle(EmployeeCreateCommand command)
        {
            var arg = command.Arg;

            //load state from events
            Initialize();

            //validate name does not exist
            if (_dataModel.Employees.Any(i => i.Name == arg.Name))
                throw new ValidationException("Employee name already exists.");

            //gen new id value
            arg.Id = _id = Guid.NewGuid().ToString();

            //set version
            arg.Version = 1;

            //ensure dependents correctly associated
            NormalizeDependents(arg);

            //calculate new pay values
            CalculatePayValues(arg);

            var empCreatedEvent = new EmployeeCreatedEvent() { Data = command.Arg };
            var empEvent = empCreatedEvent.ToEmployeeEvent();

            //persist data
            var emp = new Employee() 
            { 
                Id = arg.Id, 
                Name = arg.Name 
            };

            emp.EmployeeEvents.Add(empEvent);

            _dataModel.Employees.Add(emp);
            _dataModel.SaveChanges();

            //update internal state
            ApplyEvent(empEvent);

            yield return empCreatedEvent;
        }

        /// <summary>
        /// Handles an employee edit command.
        /// </summary>
        /// <param name="command">EmployeeEditCommand instance representing the work to be done</param>
        /// <returns>EmployeeEditedEvent if successful</returns>
        public IEnumerable<MessageBase> Handle(EmployeeEditCommand command)
        {
            var arg = command.Arg;

            //load state from events
            Initialize(arg.Id);

            //validate exists
            ValidateEmployeeDefined();

            //validate not deleted
            ValidateEmployeeNotDeleted();

            //validate correct version
            if (arg.Version != _version)
                throw new ConcurrencyException();

            //validate name is available if changed
            if (arg.Name != _name)
            {
                if (_dataModel.Employees.Any(i => i.Name == arg.Name && i.Id != _id && !i.IsDeleted))
                    throw new ValidationException("Employee name already exists.");

                var emp = _dataModel.Employees.Single(i => i.Id == arg.Id);

                emp.Name = arg.Name;
            }

            //increment version
            arg.Version = (short)(_version + 1);

            //ensure dependents correctly associated
            NormalizeDependents(arg);

            //calculate new pay values
            CalculatePayValues(arg);

            var empEditedEvent = new EmployeeEditedEvent() { Data = command.Arg };
            var empEvent = empEditedEvent.ToEmployeeEvent();

            //persist state to db
            _dataModel.EmployeeEvents.Add(empEvent);
            _dataModel.SaveChanges();

            //update internal state
            ApplyEvent(empEvent);

            yield return empEditedEvent;
        }

        /// <summary>
        /// Handles an employee delete command.
        /// </summary>
        /// <param name="command">EmployeeDeleteCommand instance representing the work to be done</param>
        /// <returns>EmployeeDeletedEvent if successful</returns>
        public IEnumerable<MessageBase> Handle(EmployeeDeleteCommand command)
        {
            var arg = command.Arg;

            //load state from events
            Initialize(arg.Id);

            //validate exists
            ValidateEmployeeDefined();

            //validate not deleted
            ValidateEmployeeNotDeleted();

            //increment version
            arg.Version = (short)(_version + 1);

            //ensure dependents correctly associated
            NormalizeDependents(arg);

            var emp = _dataModel.Employees.Single(i => i.Id == arg.Id);

            arg.IsDeleted = emp.IsDeleted = true;

            var empDeletedEvent = new EmployeeDeletedEvent() { Data = command.Arg };
            var empEvent = empDeletedEvent.ToEmployeeEvent();

            //persist state to db
            _dataModel.EmployeeEvents.Add(empEvent);
            _dataModel.SaveChanges();

            //update internal state
            ApplyEvent(empEvent);

            yield return empDeletedEvent;
        }
    }
}
