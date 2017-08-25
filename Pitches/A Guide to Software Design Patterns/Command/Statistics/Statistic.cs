// <Statistics>/Statistic.cs
namespace Command.Statistics
{
    public enum StatisticType
    {
        Agility,
        Charisma,
        Strength
    }

    internal interface IStatistic
    {
        decimal Value { get; set; }
    }

    public class Strength : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    public class Agility : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    public class Charisma : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }
}
