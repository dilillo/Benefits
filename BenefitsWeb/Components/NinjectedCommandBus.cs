using Benefits.CommandBiz;
using Benefits.Infrastructure.Commands;
using Benefits.Infrastructure.Events;
using Ninject;
using System;

namespace BenefitsWeb.Components
{
    /// <summary>
    /// CommandBus implementation that uses Ninject to create the command handlers.
    /// </summary>
    public class NinjectedCommandBus : DefaultCommandBus
    {
        public NinjectedCommandBus(IEventBus eventBus, IKernel kernel) : base(eventBus)
        {
            _kernel = kernel;

            RegisterHandler<EmployeeAggregate>();
        }

        IKernel _kernel;

        protected override object CreateHandler(Type type)
        {
            return _kernel.Get(type);
        }
    }
}