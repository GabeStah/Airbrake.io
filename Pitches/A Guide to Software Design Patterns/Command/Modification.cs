// Modification.cs
using System;
using System.Reflection;
using Command.Statistics;
using Utility;

namespace Command
{
    internal enum Status
    {
        ExecuteFailed,
        ExecuteSucceeded,
        Queued,
        RevertFailed,
        RevertSucceeded
    }

    /// <summary>
    /// Defines all the fundamental properties and methods of modifications.
    /// 
    /// Acts as 'Command' within Command pattern.
    /// </summary>
    internal interface IModification
    {
        void Execute();
        Guid Id { get; set; }
        void Revert();
        Status Status { get; set; }
    }

    /// <summary>
    /// Base Modification class, used to alter Character Statistic values.
    /// 
    /// Acts as a 'ConcreteCommand' within Command pattern.
    /// </summary>
    internal class Modification : IModification
    {
        private readonly Character _character;
        private readonly StatisticType _statisticType;

        public Guid Id { get; set; } = Guid.NewGuid();
        public Status Status { get; set; } = Status.Queued;
        public readonly decimal Value;

        /// <summary>
        /// Get character statistic object.
        /// </summary>
        internal IStatistic CharacterStatistic => (IStatistic) 
            _character
            .GetType()
            .GetProperty(_statisticType.ToString())?
            .GetValue(_character);
        
        /// <summary>
        /// Get character statistic value property.
        /// </summary>
        internal PropertyInfo CharacterStatisticValueProperty =>
            CharacterStatistic?.GetType().GetProperty("Value");

        public Modification(Character character, StatisticType statisticType, decimal value)
        {
            _character = character;
            _statisticType = statisticType;
            Value = value;
        }

        /// <summary>
        /// Execute this modification.
        /// </summary>
        public void Execute()
        {
            Status = UpdateValue() ? Status.ExecuteSucceeded : Status.ExecuteFailed;

            // Output message.
            Logging.Log($"{Status} for modification {this}.");
        }

        /// <summary>
        /// Revert this modification.
        /// </summary>
        public void Revert()
        {
            Status = UpdateValue(true) ? Status.RevertSucceeded : Status.RevertFailed;

            // Output message.
            Logging.Log($"{Status} for modification {this}.");
        }

        /// <summary>
        /// Updates the value of the underlying Character Statistic property.
        /// </summary>
        /// <param name="isReversion">Indicates if this is a reversion command.</param>
        /// <returns>Indicates if update was successful.</returns>
        internal bool UpdateValue(bool isReversion = false)
        {
            try
            {
                // Return if property not set.
                if (CharacterStatisticValueProperty == null) return false;

                // Assign original and new values.
                var originalValue = CharacterStatistic.Value;
                var newValue = 0m;
                // Add values normally, but subtract if reversion.
                newValue = isReversion ? CharacterStatistic.Value - Value : CharacterStatistic.Value + Value;

                // Set modified value.
                CharacterStatisticValueProperty.SetValue(CharacterStatistic, newValue);

                // Output confirmation message.
                Logging.Log($"[{_character}] - '{CharacterStatistic.GetType().Name}' {(isReversion ? "reverted" : "modified")} from {originalValue} to {newValue}.");
            }
            catch (Exception)
            {
                return false;
            }
            // Return successful result.
            return true;
        }

        public override string ToString()
        {
            return $"[Id: {Id}, Statistic: {_statisticType}, Value: {Value}]";
        }
    }
}
