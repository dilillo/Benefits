using Benefits.Infrastructure.Events;
using Benefits.QueryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Benefits.QueryBiz
{
    /// <summary>
    /// Services queries for read side employee data from cache.
    /// </summary>
    /// <remarks>
    /// This is only an example of how CQRS and the eventing model in play can be used to add features without impacting
    /// the way in which commands are executed by the write side logic.  In a non-demo scenario we'd likely just update the in memory
    /// version of the objects when the events are received rather than clearing the cache outright.
    /// </remarks>
    public class Queries : IQueries,
        IEventHandler<EmployeeCreatedEvent>,
        IEventHandler<EmployeeEditedEvent>,
        IEventHandler<EmployeeDeletedEvent>
        
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="dataModel">Instance of the write side datamodel to use</param>
        public Queries(IQueryDataModel dataModel)
        {
            _dataModel = dataModel;
            _cache = MemoryCache.Default;
        }

        // internal state
        readonly IQueryDataModel _dataModel;
        readonly MemoryCache _cache;

        // known constants (cache keys)
        const string EmployeesCacheKey = "Employees";
        const string KpisCacheKey = "Kpis";

        /// <summary>
        /// Gets all non-deleted employees.
        /// </summary>
        /// <returns>All non-deleted employees</returns>
        public IEnumerable<EmployeeDetail> GetAllEmployees()
        {
            var results = _cache.Get(EmployeesCacheKey) as IEnumerable<EmployeeDetail>;

            if (results == null)
            {
                //if not found in the cache get it from the read side data store
                results = _dataModel.EmployeeDetails.Include("DependentDetails")
                    .Where(i => !i.IsDeleted)
                    .OrderBy(i => i.Name).ToArray();

                _cache.Add(EmployeesCacheKey, results, new CacheItemPolicy() { SlidingExpiration = new TimeSpan(1, 0, 0) });
            }

            return results;
        }

        /// <summary>
        /// Gets all Key Performance Indicators.
        /// </summary>
        /// <returns>All key performance indicators</returns>
        public IEnumerable<Kpi> GetAllKpis()
        {
            var results = _cache.Get(KpisCacheKey) as IEnumerable<Kpi>;

            if (results == null)
            {
                //if not found in the cache get it from the read side data store
                results = _dataModel.Kpis.ToArray();

                _cache.Add(KpisCacheKey, results, new CacheItemPolicy() { SlidingExpiration = new TimeSpan(1, 0, 0) });
            }

            return results;
        }

        /// <summary>
        /// Gets an employee by id.
        /// </summary>
        /// <param name="id">id of the employee</param>
        /// <returns>Corresponding employee details or null if not found</returns>
        public EmployeeDetail GetEmployeeById(string id)
        {
            return GetAllEmployees().SingleOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// Gets a dependent and the associated employee by the id of the dependent.
        /// </summary>
        /// <param name="id">id of the dependent</param>
        /// <returns>Corresponding dependent details or null if not found</returns>
        public DependentDetail GetDependentById(string id)
        {
            return GetAllEmployees().SelectMany(i => i.DependentDetails).SingleOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// Handles EmployeeCreatedEvent event.
        /// </summary>
        /// <param name="event">EmployeeCreatedEvent to handle</param>
        public void Handle(EmployeeCreatedEvent @event)
        {
            ExpireCachedItems();
        }

        /// <summary>
        /// Handles EmployeeEditedEvent event.
        /// </summary>
        /// <param name="event">EmployeeEditedEvent to handle</param>
        public void Handle(EmployeeEditedEvent @event)
        {
            ExpireCachedItems();
        }

        /// <summary>
        /// Handles EmployeeDeletedEvent event.
        /// </summary>
        /// <param name="event">EmployeeDeletedEvent to handle</param>
        public void Handle(EmployeeDeletedEvent @event)
        {
            ExpireCachedItems();
        }

        /// <summary>
        /// Clears stale data from the cache.
        /// </summary>
        void ExpireCachedItems()
        {
            _cache.Remove(EmployeesCacheKey);
            _cache.Remove(KpisCacheKey);
        }
    }
}
