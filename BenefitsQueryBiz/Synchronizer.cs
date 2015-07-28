using Benefits.Infrastructure.Events;
using Benefits.QueryData;
using System.Collections.Generic;
using System.Linq;

namespace Benefits.QueryBiz
{
    /// <summary>
    /// Updates the read side of the data store based on events received from the write side business logic.
    /// </summary>
    public class Synchronizer : 
        IEventHandler<EmployeeCreatedEvent>,
        IEventHandler<EmployeeEditedEvent>,
        IEventHandler<EmployeeDeletedEvent>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="dataModel">Instance of the data model to use to update the read side datastore</param>
        public Synchronizer(IQueryDataModel dataModel)
        {
            _dataModel = dataModel;
        }

        // internal state
        readonly IQueryDataModel _dataModel;

        IEnumerable<Kpi> _kpis;

        /// <summary>
        /// Gets a KPI by id and creates it if it doesn't exist.
        /// </summary>
        /// <param name="id">Unique identifier of the KPI</param>
        /// <returns>Instance of the KPI</returns>
        Kpi GetKpiById(string id)
        {
            if (_kpis == null)
                _kpis = _dataModel.Kpis.ToArray();

            var kpi = _kpis.SingleOrDefault(i => i.Id == id);

            if (kpi == null)
            {
                //if it doesnt exist add it
                kpi = new Kpi() { Id = id, Value = 0 };

                _dataModel.Kpis.Add(kpi);
            }

            return kpi;
        }

        /// <summary>
        /// Adjusts the KPIs.
        /// </summary>
        /// <param name="grossPayDif">Difference in Gross Pay to be applied</param>
        /// <param name="benefitsDif">Difference in Benefits to be applied</param>
        /// <param name="netPayDif">Difference in Net Pay to be applied</param>
        /// <param name="employees">Difference in Employees to be applied</param>
        void AdjustKpis(decimal grossPayDif, decimal benefitsDif, decimal netPayDif, int employees = 0)
        {
            GetKpiById("GrossPay").Value += (int)grossPayDif;
            GetKpiById("Benefits").Value += (int)benefitsDif;
            GetKpiById("NetPay").Value += (int)netPayDif;
            GetKpiById("Employees").Value += employees;
        }

        /// <summary>
        /// Handles the EmployeeCreatedEvent and updates the read side datastore accordingly.
        /// </summary>
        /// <param name="event">Instance of EmployeeCreatedEvent to handle</param>
        public void Handle(EmployeeCreatedEvent @event)
        {
            var empDetail = @event.Data.ToEmployeeDetail();

            foreach (var depModel in @event.Data.Dependents)
                empDetail.DependentDetails.Add(depModel.ToDependentDetail());

            _dataModel.EmployeeDetails.Add(empDetail);

            AdjustKpis(empDetail.GrossPay, empDetail.Benefits, empDetail.NetPay, 1);

            _dataModel.SaveChanges();
        }

        /// <summary>
        /// Handles the EmployeeEditedEvent and updates the read side datastore accordingly.
        /// </summary>
        /// <param name="event">Instance of EmployeeEditedEvent to handle</param>
        public void Handle(EmployeeEditedEvent @event)
        {
            var empDetail = _dataModel.EmployeeDetails.Include("DependentDetails").Single(i => i.Id == @event.Data.Id);

            AdjustKpis(@event.Data.GrossPay - empDetail.GrossPay, @event.Data.Benefits - empDetail.Benefits, @event.Data.NetPay - empDetail.NetPay);

            //removed the old version of the dependents (this is a shortcut im taking given the time constraints)
            foreach (var depDetail in empDetail.DependentDetails.ToArray())
                _dataModel.DependentDetails.Remove(depDetail);

            empDetail = @event.Data.ToEmployeeDetail(empDetail);

            //add new version of dependents 
            foreach (var depModel in @event.Data.Dependents)
                empDetail.DependentDetails.Add(depModel.ToDependentDetail());

            _dataModel.SaveChanges();
        }

        /// <summary>
        /// Handles the EmployeeDeletedEvent and updates the read side datastore accordingly.
        /// </summary>
        /// <param name="event">Instance of EmployeeDeletedEvent to handle</param>
        public void Handle(EmployeeDeletedEvent @event)
        {
            var empDetail = _dataModel.EmployeeDetails.Single(i => i.Id == @event.Data.Id);

            empDetail.IsDeleted = true;
            empDetail.Version = @event.Version;

            AdjustKpis(-1 * empDetail.GrossPay, -1 * empDetail.Benefits, -1 * empDetail.NetPay, -1);

            _dataModel.SaveChanges();
        }
    }
}
