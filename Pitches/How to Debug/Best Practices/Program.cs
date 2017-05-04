using Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Best_Practices
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set Medium size, add Sauce, add Provolone cheese, add Pepperoni, add Olives, then build.
            var pizza = new PizzaBuilder(Size.Medium)
                                 .AddSauce()
                                 .AddCheese(Cheese.Provolone)
                                 .AddPepperoni()
                                 .AddOlives()
                                 .Build();
            Logging.Log(pizza);

            DateTime birthday = new DateTime(2000, 1, 1);
            int daysOld = (DateTime.Today - birthday).Days;
            Logging.Log($"Age is {daysOld} days old.");
        }
    }
}

namespace Builder
{
    public enum Cheese { Cheddar, Mozzarella, Parmesan, Provolone }
    public enum Size { Small, Medium, Large, XLarge }

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
