using System.Collections.Generic;

namespace Benefits.Infrastructure.Commands
{
    /// <summary>
    /// Defintes a component that can handle a command of a given type.
    /// </summary>
    /// <typeparam name="TCommandType">Type of command that the component will handle</typeparam>
    public interface ICommandHandler<TCommandType> where TCommandType : MessageBase
    {
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <returns>Set of messages (events) resulting from the handling of the command</returns>
        IEnumerable<MessageBase> Handle(TCommandType command);
    }
}
