using System;
using Utility;

namespace TemplateMethod
{
    /// ReSharper disable once InconsistentNaming
    internal class NBCShow : Show
    {
        private BroadcastSlot Slot { get; }

        public override void AssignBroadcastSlot()
        {
            // Assign private Slot to public BroadcastSlot property.
            BroadcastSlot = Slot;
            Logging.Log($"{Name} broadcast slot set to {Slot}.");
        }

        public override void FindNetwork()
        {
            Network = "NBC";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create a new NBC show, broadcast on specified day and timeslot.
        /// </summary>
        /// <param name="name">Name of the show.</param>
        /// <param name="day">Day of broadcast.</param>
        /// <param name="hour">Hour of day.</param>
        /// <param name="minute">Minute of hour.</param>
        public NBCShow(string name, DayOfWeek day, int hour, int minute = 0) : base(name)
        {
            Slot = new BroadcastSlot(day, hour, minute);
        }
    }
}