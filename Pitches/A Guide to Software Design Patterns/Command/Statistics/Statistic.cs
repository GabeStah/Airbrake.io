// <Statistics>/Statistic.cs
namespace Command.Statistics
{
    internal enum StatisticType
    {
        Agility,
        Charisma,
        Strength
    }

    internal interface IStatistic
    {
        decimal Value { get; set; }
    }

    internal class Strength : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    internal class Agility : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    internal class Charisma : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }
}
