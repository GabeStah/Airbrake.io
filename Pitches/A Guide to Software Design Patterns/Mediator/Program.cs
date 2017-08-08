// Program.cs
namespace Mediator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var crier = new Crier();

            // Create some Characters and assign carrier.
            var arya = new Person("Arya Stark", crier);
            var cersei = new Person("Cersei Lannister", crier);
            var daenerys = new Person("Daenerys Targaryen", crier);
            // Without a tongue it's difficult to speak, so Ilyn remains silent and listens.
            var ilyn = new Person("Ilyn Payne", crier);
            var tyrion = new Person("Tyrion Lannister", crier);

            // Send messages from respective characters.
            arya.Say("Valar morghulis.");
            tyrion.Say("Never forget what you are, for surely the world will not.");
            daenerys.Say("Men are mad and gods are madder.");
            cersei.Say("When you play the game of thrones, you win or you die. There is no middle ground.");
        }
    }
}
