
namespace Benefits.Infrastructure.Events
{
    /// <summary>
    /// Defines a component capable of receiving a published event.
    /// </summary>
    /// <typeparam name="TEventType">Type of event handled by the component</typeparam>
    public interface IEventHandler<TEventType> where TEventType : MessageBase
    {
        /// <summary>
        /// Handles the published event.
        /// </summary>
        /// <param name="event">Event to handle</param>
        void Handle(TEventType @event);
    }
}
