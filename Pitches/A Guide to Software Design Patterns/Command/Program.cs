using Command.Statistics;
using Xunit;

namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a manager.
            var manager = new ModificationManager();

            // Create a character with initial stats.
            var alice = new Character("Alice", 10, 14, 12);
            // Create another character with default stats.
            var bob = new Character("Bob");

            // Create some modifications for Alice.
            var agilityAlice = new Modification(alice, StatisticType.Agility, 8);
            var charismaAlice = new Modification(alice, StatisticType.Charisma, -4);
            var strengthAlice = new Modification(alice, StatisticType.Strength, 0.75m);

            // Create modifications for Bob.
            var agilityBob = new Modification(bob, StatisticType.Agility, 99.99m);
            var charismaBob = new Modification(bob, StatisticType.Charisma, -42);

            // Add modifications to queue.
            manager.AddModification(agilityAlice);
            manager.AddModification(strengthAlice);
            manager.AddModification(agilityBob);
            manager.AddModification(charismaBob);
            manager.AddModification(charismaAlice);

            // Process queue.
            manager.ProcessQueue();

            // Revert agility modification.
            manager.RevertModification(agilityAlice);

            // Confirm that we can revert in any order, regardless of queue order.
            Assert.Equal(bob.Charisma.Value, charismaBob.Value);
            manager.RevertModification(charismaBob);
            Assert.Equal(bob.Charisma.Value, 0);

            // Confirm that passing by Id also works.
            Assert.Equal(alice.Strength.Value, 12 + strengthAlice.Value);
            manager.RevertModification(strengthAlice.Id);
            Assert.Equal(alice.Strength.Value, 12);
        }
    }
}
