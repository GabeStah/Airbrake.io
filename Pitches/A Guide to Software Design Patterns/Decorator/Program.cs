using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Builder;
using Utility;

namespace Decorator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set Large size (default), add Sauce, add Mozzarella cheese (default), add Olives, add Mushrooms, then build it.
            var pizzaOlivesMushrooms = new PizzaBuilder()
                                          .AddSauce()
                                          .AddCheese()
                                          .AddOlives()
                                          .AddMushrooms()
                                          .Build();
            Logging.Log(pizzaOlivesMushrooms);

            Logging.LineSeparator();

            var pizza = new Pizza();
            Logging.Log(pizza);
            //pizza = new Pepperoni(pizza);
            //Logging.Log(pizza);
            var mushrooms = new Mushrooms(pizza);
            Logging.Log(mushrooms);
            var pinneapple = new Pineapple(mushrooms);
            Logging.Log(pinneapple);
        }
    }

    public enum Cheese { Cheddar, Mozzarella, Parmesan, Provolone }
    public enum Size { Small, Medium, Large, XLarge }

    public interface IPizza
    {
        Cheese Cheese { get; }
        decimal Cost { get; set; }
        string Description { get; }
        Size Size { get; }
        List<string> Toppings { get; set; }
    }

    public class Pizza : IPizza
    {
        public Cheese Cheese { get; } = Cheese.Mozzarella;
        public decimal Cost { get; set; } = 7.5M;
        public string Description
        {
            get
            {
                if (Toppings.IsAny())
                    return Toppings.Aggregate((index, topping) => index + ", " + topping);
                return null;
            }
        }
        public Size Size { get; } = Size.Large;
        public List<string> Toppings { get; set; }
        public Pizza()
        {
            if (!Toppings.IsAny())
                Toppings = new List<string>();
        }
    }

    public abstract class PizzaDecorator : IPizza
    {
        protected IPizza Pizza { get; }
        public Cheese Cheese { get; }
        public decimal Cost { get; set; }
        public string Description
        {
            get
            {
                return Pizza.Toppings.Aggregate((index, topping) => index + ", " + topping);
            }
        }
        public Size Size { get; }
        public List<string> Toppings { get; set; } = new List<string>();
        public PizzaDecorator(IPizza pizza)
        {
            Pizza = pizza;
            if (!Pizza.Toppings.IsAny())
                Pizza.Toppings = new List<string>();
        }
    }


    public class Mushrooms : PizzaDecorator
    {
        public Mushrooms(IPizza pizza) : base(pizza)
        {
            Toppings.Add(this.GetType().Name.ToString());
            Cost += 2;
        }
    }

    public class Pineapple : PizzaDecorator
    {
        public Pineapple(IPizza pizza) : base(pizza)
        {
            Toppings.Add(this.GetType().Name.ToString());
            Cost += 3.5M;
        }
    }


    //public class Pepperoni : IPizza
    //{
    //    protected IPizza Pizza { get; }
    //    public Cheese Cheese { get; }
    //    public decimal Cost
    //    {
    //        get
    //        {
    //            return Pizza.Cost + 2;
    //        }
    //    }
    //    public string Description { get; }
    //    public Size Size { get; }
    //    public List<string> Toppings { get; }
    //    public Pepperoni(IPizza pizza)
    //    {
    //        Pizza = pizza;
    //        Pizza.Toppings.Add(this.GetType().Name.ToString());
    //    }
    //}

    //public class Olives : IPizza
    //{
    //    protected IPizza Pizza { get; }
    //    public override decimal Cost
    //    {
    //        get
    //        {
    //            return Pizza.Cost + 1;
    //        }
    //    }
    //    public Olives(IPizza pizza)
    //    {
    //        Pizza = pizza;
    //        Pizza.Toppings.Add(this.GetType().Name.ToString());
    //    }
    //}

    interface ICoffee
    {
        int Cost { get; }
        string Description { get; }
    }

    class SimpleCoffee : ICoffee
    {
        public int Cost { get; } = 10;
        public string Description { get; } = "Simple coffee";
    }

    class MilkCoffee : ICoffee
    {
        protected ICoffee Coffee { get; }

        public int Cost { get { return Coffee.Cost + 2; } }

        public string Description { get { return Coffee.Description + ", milk"; } }

        public MilkCoffee(ICoffee coffee)
        {
            Coffee = coffee;
        }
    }

    class WhipCoffee : ICoffee
    {
        protected ICoffee Coffee { get; }

        public int Cost { get { return Coffee.Cost + 5; } }

        public string Description { get { return Coffee.Description + ", whip"; } }

        public WhipCoffee(ICoffee coffee)
        {
            Coffee = coffee;
        }
    }

    class VanillaCoffee : ICoffee
    {
        protected ICoffee Coffee { get; }

        public int Cost { get { return Coffee.Cost + 3; } }

        public string Description { get { return Coffee.Description + ", vanilla"; } }

        public VanillaCoffee(ICoffee coffee)
        {
            Coffee = coffee;
        }
    }
}
