using System;

namespace TemplateMethod
{
    internal class BroadcastSlot
    {
        public enum SlotType
        {
            Streaming,
            TimeSlot
        }

        private DayOfWeek Day { get; }
        private int Hour { get; }
        private int Minute { get; }

        /// <summary>
        /// Gets the string version of broadcast time.
        /// </summary>
        public string BroadcastTime => DateTime.Parse($"{Hour}:{Minute}:00").ToLongTimeString();

        public SlotType Type { get; }
        
        /// <summary>
        /// Creates a new BroadcastSlot with a default type of Streaming.
        /// </summary>
        public BroadcastSlot()
        {
            Type = SlotType.Streaming;
        }

        /// <summary>
        /// Specifies a specific day and timeslot for broadcast.
        /// </summary>
        /// <param name="day">Day of the week.</param>
        /// <param name="hour">Hour of the day.</param>
        /// <param name="minute">Minute of the hour.</param>
        public BroadcastSlot(DayOfWeek day, int hour, int minute)
        {
            Day = day;
            Hour = hour;
            Minute = minute;
            Type = SlotType.TimeSlot;
        }

        /// <summary>
        /// Gets BroadcastSlot as formatted string suitable for output.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString()
        {
            // If TimeSlot, output time and day, otherwise output streaming info.
            return Type == SlotType.TimeSlot ? $"{BroadcastTime} on {Day}" : $"all times [{Type}]";
        }
    }
}