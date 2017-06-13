using ObjectDumper;
using System.Text;
using Utility;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var telePizzaPepOlives = new TelescopedPizza(Size.Medium, Cheese.Provolone, true, true, false, true);
            Logging.Log(telePizzaPepOlives);

            var telePizzaOlivesMushrooms = new TelescopedPizza(Size.Large, Cheese.Mozzarella, true, false, false, true, true);
            Logging.Log(telePizzaOlivesMushrooms);

            // Set Medium size, add Sauce, add Provolone cheese, add Pepperoni, add Olives, then build.
            var pizzaPepOlives = new PizzaBuilder(Size.Medium)
                                 .AddSauce()
                                 .AddCheese(Cheese.Provolone)
                                 .AddPepperoni()
                                 .AddOlives()
                                 .Build();
            Logging.Log(pizzaPepOlives);
            ObjectDumper.ObjectDumperExtensions.Dump(pizzaPepOlives);

            // Set Large size (default), add Sauce, add Mozzarella cheese (default), add Olives, add Mushrooms, then build it.
            var pizzaOlivesMushrooms = new PizzaBuilder()
                                 .AddSauce()
                                 .AddCheese()
                                 .AddOlives()
                                 .AddMushrooms()
                                 .Build();
            Logging.Log(pizzaOlivesMushrooms);
        }
    }

    public enum Cheese { Cheddar, Mozzarella, Parmesan, Provolone }
    public enum Size { Small, Medium, Large, XLarge }

    class TelescopedPizza
    {

        public Size Size { get; set; }
        public Cheese Cheese { get; set; }
        public bool Sauce { get; set; }
        public bool Pepperoni { get; set; }
        public bool Ham { get; set; }
        public bool Olives { get; set; }
        public bool Mushrooms { get; set; }

        public TelescopedPizza(Size size,
                               Cheese cheese,
                               bool sauce,
                               bool pepperoni,
                               bool ham,
                               bool olives)
        {
            Size = size;
            Cheese = cheese;
            Sauce = sauce;
            Pepperoni = pepperoni;
            Ham = ham;
            Olives = olives;
        }

        public TelescopedPizza(Size size,
                               Cheese cheese,
                               bool sauce, 
                               bool pepperoni, 
                               bool ham, 
                               bool olives, 
                               bool mushrooms)
        {
            Size = size;
            Cheese = cheese;
            Sauce = sauce;
            Pepperoni = pepperoni;
            Ham = ham;
            Olives = olives;
            Mushrooms = mushrooms;
        }
    }

    class Pizza
    {
        public Size Size { get; set; }
        public Cheese Cheese { get; set; }
        public bool Sauce { get; set; }
        public bool Pepperoni { get; set; }
        public bool Ham { get; set; }
        public bool Olives { get; set; }
        public bool Mushrooms { get; set; }

        public Pizza(PizzaBuilder builder)
        {
            Size = builder.Size;
            Cheese = builder.Cheese;
            Sauce = builder.Sauce;
            Pepperoni = builder.Pepperoni;
            Ham = builder.Ham;
            Olives = builder.Olives;
            Mushrooms = builder.Mushrooms;
        }
    }

    class PizzaBuilder
    {
        public Size Size { get; set; }
        public Cheese Cheese { get; set; }
        public bool Sauce { get; set; }
        public bool Pepperoni { get; set; }
        public bool Ham { get; set; }
        public bool Olives { get; set; }
        public bool Mushrooms { get; set; }

        public Pizza Build()
        {
            return new Pizza(this);
        }

        public PizzaBuilder(Size size = Size.Large)
        {
            Size = size;
        }

        public PizzaBuilder AddCheese(Cheese cheese = Cheese.Mozzarella)
        {
            Cheese = cheese;
            return this;
        }

        public PizzaBuilder AddSauce()
        {
            Sauce = true;
            return this;
        }

        public PizzaBuilder AddPepperoni()
        {
            Pepperoni = true;
            return this;
        }

        public PizzaBuilder AddHam()
        {
            Ham = true;
            return this;
        }

        public PizzaBuilder AddOlives()
        {
            Olives = true;
            return this;
        }

        public PizzaBuilder AddMushrooms()
        {
            Mushrooms = true;
            return this;
        }
    }
}
