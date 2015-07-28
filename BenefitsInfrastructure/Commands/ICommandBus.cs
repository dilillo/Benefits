
namespace Benefits.Infrastructure.Commands
{
    /// <summary>
    /// Defines a component that can execute commands against a set of registered handlers.
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Registers a command handler type.
        /// </summary>
        /// <typeparam name="TCommandHandlerType">Type to register</typeparam>
        void RegisterHandler<TCommandHandlerType>();

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="command">Command to execute</param>
        void Execute(MessageBase command);
    }
}
