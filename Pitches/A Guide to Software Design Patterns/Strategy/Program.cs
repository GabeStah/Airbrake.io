// Program.cs
using Utility;

namespace Strategy
{
    internal class Program
    {
        private static void Main()
        {
            var bear = new Package("A teddy bear");
            var defaultPackager = new Packager(new DefaultStrategy());
            defaultPackager.Pack(bear);
            Logging.Log(bear.ToString());

            var monitor = new Package("A computer monitor");
            var packager = new Packager(new FragileStrategy());
            packager.Pack(monitor);
            Logging.Log(monitor.ToString());

            var fish = new Package("Some salmon filets");
            packager.Pack(fish, new PerishableStrategy());
            Logging.Log(fish.ToString());

            var massiveBear = new Package("A MASSIVE teddy bear");
            packager.Pack(massiveBear, new OversizedStrategy());
            Logging.Log(massiveBear.ToString());
        }
    }
}
