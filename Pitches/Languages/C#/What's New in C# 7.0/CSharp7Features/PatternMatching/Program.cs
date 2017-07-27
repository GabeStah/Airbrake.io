using System;
using Utility;

namespace PatternMatching
{
    interface IOrganism
    {
        double Population { get; set; }
    }

    class Insect : IOrganism
    {
        public double Population { get; set; }

        public Insect()
        {
            // Approximately 19 quadrillion insects.
            Population = 1e19;
        }

        public Insect(double population)
        {
            Population = population;
        }
    }

    class Mammal : IOrganism
    {
        public double Population { get; set; }

        public Mammal()
        {
            // Approximately 1 trillion mammals.
            Population = 1e12;
        }

        public Mammal(double population)
        {
            Population = population;
        }
    }

    class Human : Mammal
    {
        public Human()
        {
            // Approximately 7.52 billion humans.
            Population = 7.52e9;
        }

        public Human(double population)
        {
            Population = population;
        }
    }

    class Bee : Insect
    {
        public Bee()
        {
            // Approximately 10 - 50 trillion bees.
            Population = 30e12;
        }

        public Bee(double population)
        {
            Population = population;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TypePatternExample();

            Logging.LineSeparator();

            VarPatternExample();

            Logging.LineSeparator();

            IsExpressionPatternExample();

        }

        private static void TypePatternExample()
        {
            // Pass new Human.
            GetPopulationUsingType(new Human());
            // Pass new Bee.
            GetPopulationUsingType(new Bee());
            // Pass new Mammal.
            GetPopulationUsingType(new Mammal());
            // Pass new Insect.
            GetPopulationUsingType(new Insect());

            Logging.LineSeparator();

            // Pass new Human, with low population argument.
            GetPopulationUsingType(new Human(4.2e6));
        }

        private static void VarPatternExample()
        {
            // Pass new Human.
            GetPopulationUsingVar(new Human());
            // Pass new Bee.
            GetPopulationUsingVar(new Bee());

            Logging.LineSeparator();

            // Pass new Mammal, an unknown type..
            GetPopulationUsingVar(new Mammal());
        }

        private static void IsExpressionPatternExample()
        {
            // Pass new Human.
            GetPopulationUsingIs(new Insect());
            // Pass new Bee.
            GetPopulationUsingIs(new Mammal());

            Logging.LineSeparator();

            // Pass null.
            GetPopulationUsingIs(null);
        }

        private static void GetPopulationUsingType(IOrganism organism)
        {
            double population = 0;
            // Switch on passed organism using type pattern matching.
            switch (organism)
            {
                case Bee bee:
                    population = bee.Population;
                    break;
                case Human human when (human.Population < 1e7):
                    // If a Human is passed and the population is too low, panic!
                    Logging.Log($"The human population is too low at {human.Population:n0}!  Apocalypse!");
                    return;
                case Human human:
                    // If the Human population is alright, proceed as normal.
                    population = human.Population;
                    break;
                case Insect insect:
                    population = insect.Population;
                    break;
                case Mammal mammal:
                    population = mammal.Population;
                    break;
                default:
                    // Output alert if organism type is unknown.
                    Logging.Log($"Unknown organism type ({organism.GetType().Name}), or population exceeds all known estimates.");
                    return;
            }
            // Output retrieved population estimate.
            Logging.Log($"Estimated number of {organism.GetType().Name.ToLower()}s on Earth: {population:n0}.");
        }

        private static void GetPopulationUsingVar(IOrganism organism)
        {
            double population = 0;
            // Switch on passed organism using var pattern matching.
            switch (organism)
            {
                // Assign organism to new bee variable, if population is roughly equal to 30 trillion.
                case var bee when Math.Abs(bee.Population - 30e12) <= 1:
                    population = bee.Population;
                    break;
                // Assign organism to new human variable, if object type Name is "Human."
                case var human when human.GetType().Name == "Human":
                    population = human.Population;
                    break;
                default:
                    // Output alert if organism type is unknown.
                    Logging.Log($"Unknown organism type ({organism.GetType().Name}), or population exceeds all known estimates.");
                    return;
            }
            // Output retrieved population estimate.
            Logging.Log($"Estimated number of {organism.GetType().Name.ToLower()}s on Earth: {population:n0}.");
        }

        private static void GetPopulationUsingIs(object organism)
        {
            if (organism is null)
            {
                Logging.Log($"Organism is null, cancelling.");
            }
            if (organism is IOrganism o)
            {
                // Output retrieved population estimate.
                Logging.Log($"Estimated number of {o.GetType().Name.ToLower()}s on Earth: {o.Population:n0}.");
            }
        }
    }
}
