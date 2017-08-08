// Crier.cs
using Utility;

namespace Mediator
{
    /// <summary>
    /// Declares members for Crier.
    /// </summary>
    internal interface ICrier
    {
        void Shout(string message, Person source);
    }

    /// <summary>
    /// Event handler for MessageReceived event.
    /// </summary>
    /// <param name="message">Received message.</param>
    /// <param name="source">Person who sent the message.</param>
    internal delegate void MessageReceivedEventHandler(string message, Person source);
    
    /// <summary>
    /// Communicates messages between all Persons.
    /// 
    /// Acts as 'Mediator' within mediator pattern.
    /// </summary>
    internal class Crier : ICrier
    {
        internal event MessageReceivedEventHandler MessageReceived;

        /// <summary>
        /// Shout a message from passed Person source.
        /// </summary>
        /// <param name="message">Message to be shouted.</param>
        /// <param name="source">Person message originated from.</param>
        public void Shout(string message, Person source)
        {
            // Extend separator beyond name and message length.
            var separatorLength = source.ToString().Length + message.Length + 30;
            // Create separator with message and source
            Logging.LineSeparator($"'{message}' from {source}", separatorLength, '=');
            // If handler isn't null, invoke MessageReceived event with message and source.
            MessageReceived?.Invoke(message, source);
        }
    }
}