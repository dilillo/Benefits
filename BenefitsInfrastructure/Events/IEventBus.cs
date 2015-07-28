
namespace Benefits.Infrastructure.Events
{
    /// <summary>
    /// Defines a component that can publish events to a set of registerd event handler types.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Registers an event handler type.
        /// </summary>
        /// <typeparam name="TEventHandlerType">Type of the event handler.</typeparam>
        void RegisterHandler<TEventHandlerType>();

        /// <summary>
        /// Publishes an event to the set of interested event handlers.
        /// </summary>
        /// <param name="event">Event to publish</param>
        void Publish(MessageBase @event);
    }
}
