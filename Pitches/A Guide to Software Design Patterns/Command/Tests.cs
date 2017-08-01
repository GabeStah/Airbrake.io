using Command.Statistics;
using Xunit;

namespace Command
{
    public class Tests
    {
        [Fact]
        public void Test_AllTransactionsSuccessful()
        {
            var modificationManager = new ModificationManager();

            var character = new Character("Alice", 10, 14, 12);
            var modificationAgility = new Modification(
                character, 
                StatisticType.Agility, 
                8);

            var modificationStrength = new Modification(
                character,
                StatisticType.Strength,
                -4);

            modificationManager.AddModification(modificationAgility);

            modificationManager.AddModification(modificationStrength);

            modificationManager.ProcessQueue();

            modificationManager.RevertModification(modificationAgility);

            modificationManager.RevertModification(modificationStrength.Id);
        }
    }
}
