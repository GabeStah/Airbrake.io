using System;
using System.Collections.Generic;
using System.Linq;

namespace Command
{
    /// <summary>
    /// Manages the modification queue and actions.
    /// 
    /// Acts as 'Invoker' within Command pattern.
    /// </summary>
    internal class ModificationManager
    {
        private readonly List<IModification> _queue = new List<IModification>();

        /// <summary>
        /// Checks if any modifications are queued.
        /// </summary>
        public bool HasQueue => _queue.Any(x =>
            x.Status == Status.Queued ||
            x.Status == Status.ExecuteFailed ||
            x.Status == Status.RevertFailed);

        /// <summary>
        /// Add modification to queue.
        /// </summary>
        /// <param name="modification"></param>
        public void AddModification(IModification modification)
        {
            _queue.Add(modification);
        }

        /// <summary>
        /// Process all outstanding modifications.
        /// </summary>
        public void ProcessQueue()
        {
            // Execute modifications that are queued or failed.
            foreach (var modification in _queue.Where(x =>
                x.Status == Status.Queued ||
                x.Status == Status.ExecuteFailed))
            {
                modification.Execute();
            }

            // Revert modifications that failed.
            foreach (var modification in _queue.Where(x =>
                x.Status == Status.RevertFailed))
            {
                modification.Revert();
            }
        }

        /// <summary>
        /// Revert passed modification, if found in queue.
        /// </summary>
        /// <param name="modification">Modification to revert.</param>
        public void RevertModification(IModification modification)
        {
            // Find match.
            var match = _queue.FirstOrDefault(x => x == modification);

            // Can't revert a modification not in the queue.
            if (match == null)
            {
                throw new ArgumentException($"Modification [{modification}] not found, cannot revert.");
            }

            // Can't revert unless execution already took place.
            if (match.Status != Status.ExecuteSucceeded)
            {
                throw new ArgumentException($"Modification [{modification}] 'Status' must be Status.ExecuteSucceeded to revert.");
            }

            // Revert modification.
            match.Revert();

            // Update status and remove from queue.
            if (match.Status == Status.RevertSucceeded)
            {
                _queue.Remove(match);
            }
        }

        /// <summary>
        /// Get modification by Id and pass to primary RevertModification method.
        /// </summary>
        /// <param name="id">Id of modification to revert.</param>
        public void RevertModification(Guid id)
        {
            RevertModification(_queue.FirstOrDefault(x => x.Id == id));
        }
    }
}