using System;
using System.Collections.Generic;
using System.Linq;

namespace Benefits.Infrastructure.Events
{
    /// <summary>
    /// Component that can publish event messages to a set of registered handler types.
    /// </summary>
    /// /// <remarks>
    /// This implementation is simply for demo purposes.  A more robust setup would probably feature a combination of a durable service bus and async support.
    /// </remarks>
    public class DefaultEventBus : IEventBus
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DefaultEventBus()
        {
            _handlers = new List<Tuple<string, Type>>();
        }

        //internal state
        readonly List<Tuple<string, Type>> _handlers;

        /// <summary>
        /// Registers a type as an event handler.
        /// </summary>
        /// <typeparam name="TEventHandlerType">Type to register</typeparam>
        public void RegisterHandler<TEventHandlerType>()
        {
            var type = typeof(TEventHandlerType);

            //figure out which events this type can handle
            var handlesEventTypeNames = type.GetInterfaces()
                .Where(i => i.Name.StartsWith(typeof(IEventHandler<>).Name))
                .Select(i => i.GenericTypeArguments.First().Name);

            //load em up in our handlers list
            foreach (var handlesEventTypeName in handlesEventTypeNames)
                _handlers.Add(new Tuple<string,Type>(handlesEventTypeName, type));
        }

        /// <summary>
        /// Publishes an event out to its registered handlers.
        /// </summary>
        /// <param name="event">Event to publish</param>
        public void Publish(MessageBase @event)
        {
            //identify the handlers
            var typeName = @event.GetType().Name;
            var handlers = _handlers.Where(i => i.Item1 == typeName);

            //send each one the event
            foreach (var handler in handlers)
            {
                var handleMethod = handler.Item2.GetMethod("Handle", new Type[] { @event.GetType() });
                var handlerInstance = CreateHandler(handler.Item2);

                handleMethod.Invoke(handlerInstance, new object[] { @event });
            }
        }

        /// <summary>
        /// Creates an instance of the requested type.
        /// </summary>
        /// <param name="type">Type to create</param>
        /// <returns>Instance of the type requested</returns>
        /// <remarks>
        /// Again this is the simple implementation. Likely this would be overrident in a derrived version of the class to 
        /// incorporate something like dependency injection.
        /// </remarks>
        protected virtual object CreateHandler(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
