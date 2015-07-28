using Benefits.Infrastructure.Events;
using Benefits.QueryBiz;
using Ninject;
using System;

namespace BenefitsWeb.Components
{
    /// <summary>
    /// EventBus implementation that uses Ninject to create the event handlers.
    /// </summary>
    public class NinjectedEventBus : DefaultEventBus
    {
        public NinjectedEventBus(IKernel kernel) 
        {
            _kernel = kernel;

            RegisterHandler<Synchronizer>();
            RegisterHandler<Queries>();
        }

        IKernel _kernel;

        protected override object CreateHandler(Type type)
        {
            return _kernel.Get(type);
        }
    }
}