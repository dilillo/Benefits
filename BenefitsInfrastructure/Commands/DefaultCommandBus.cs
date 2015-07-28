using Benefits.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benefits.Infrastructure.Commands
{
    /// <summary>
    /// Component responsible for handling commands by forwarding them to registered handler types.
    /// </summary>
    /// <remarks>
    /// This implementation is simply for demo purposes.  A more robust setup would probably feature a combination of a durable service bus and async support.
    /// </remarks>
    public class DefaultCommandBus : ICommandBus
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="eventBus">IEventBus instance to use to forward on events published by command handlers</param>
        public DefaultCommandBus(IEventBus eventBus)
        {
            _handlers = new List<Tuple<string, Type>>();
            _eventBus = eventBus;
        }

        //internal state
        readonly List<Tuple<string, Type>> _handlers;

        readonly IEventBus _eventBus;

        /// <summary>
        /// Registers a type as being a command handler.
        /// </summary>
        /// <typeparam name="TCommandHandlerType">The command handler type</typeparam>
        public void RegisterHandler<TCommandHandlerType>()
        {
            var type = typeof(TCommandHandlerType);

            //get the types of commands this type handles
            var handlesCmdTypeNames = type.GetInterfaces()
                .Where(i => i.Name.StartsWith(typeof(ICommandHandler<>).Name))
                .Select(i => i.GenericTypeArguments.First().Name);

            //load them into the handlers list
            foreach (var handlesCmdTypeName in handlesCmdTypeNames)
                _handlers.Add(new Tuple<string, Type>(handlesCmdTypeName, type));
        }

        /// <summary>
        /// Executes a command by locating the associated handler and then calling its handle method.
        /// </summary>
        /// <param name="command">Command instance to handle</param>
        public void Execute(MessageBase command)
        {
            //get the handlers
            var typeName = command.GetType().Name;
            var handlers = _handlers.Where(i => i.Item1 == typeName);

            //run each handler (note: im not limiting the number of handlers here but that could easy be done via the register method)
            foreach (var handler in handlers)
            {
                //run the handler
                var handleMethod = handler.Item2.GetMethod("Handle", new Type[] { command.GetType() });
                var handlerInstance = CreateHandler(handler.Item2);
                var results = (IEnumerable<MessageBase>)handleMethod.Invoke(handlerInstance, new object[] { command });

                //publish the resulting events
                foreach (var result in results)
                    _eventBus.Publish(result);
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
