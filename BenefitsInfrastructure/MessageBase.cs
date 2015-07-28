using System;

namespace Benefits.Infrastructure
{
    /// <summary>
    /// Base class defining the common properties of a message in the Benefits system's CQRS/Event Streaming architecture.
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MessageBase()
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
            Version = 1;
        }

        /// <summary>
        /// Unique id of the message.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Friendly name of the message.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date and Time on which the message was sent.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Version of the message.
        /// </summary>
        public byte Version { get; set; }
    }
}
