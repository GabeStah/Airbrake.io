// Character.cs
using Command.Statistics;

namespace Command
{
    /// <summary>
    /// Stores basic character information, including statistics.
    /// 
    /// Acts as 'Receiver' within Command pattern.
    /// </summary>
    internal class Character
    {
        public string Name { get; set; }

        public Agility Agility { get; set; } = new Agility();
        public Charisma Charisma { get; set; } = new Charisma();
        public Strength Strength { get; set; } = new Strength();

        public Character(string name)
        {
            Name = name;
        }

        public Character(string name, decimal agility, decimal charisma, decimal strength)
        {
            Name = name;

            Agility.Value = agility;
            Charisma.Value = charisma;
            Strength.Value = strength;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}