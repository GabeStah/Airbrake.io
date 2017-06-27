using Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Decorator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set Large size (default), add Sauce, add Mozzarella cheese (default), add Olives, add Mushrooms, add Pepperoni, then build it.
            var builderPizza = new PizzaBuilder()
                               .AddSauce()
                               .AddCheese()
                               .AddOlives()
                               .AddMushrooms()
                               .AddPepperoni()
                               .Build();
            Logging.Log(builderPizza);

            Logging.LineSeparator();

            // Create a basic pizza.
            var pizza = new PizzaBase();
            Logging.Log(pizza.Description);

            // Decorate the base pizza by adding olives.
            var olives = new OlivesDecorator(pizza);
            Logging.Log(olives.Description);

            // Add mushrooms.
            var mushrooms = new MushroomsDecorator(olives);
            Logging.Log(mushrooms.Description);

            // Add pepperoni.
            var pepperoni = new PepperoniDecorator(mushrooms);
            Logging.Log(pepperoni.Description);

            // Add pineapple.
            var pineapple = new PineappleDecorator(pepperoni);
            Logging.Log(pineapple.Description);
        }
    }

    public enum Cheese { Cheddar, Mozzarella, Parmesan, Provolone }
    public enum Size { Small, Medium, Large, XLarge }

    /// <summary>
    /// Basic interface defining the core properties.
    /// </summary>
    public interface IPizza
    {
        Cheese Cheese { get; }
        decimal Cost { get; }
        string Description { get; }
        Size Size { get; }
        List<string> Toppings { get; }
    }

    /// <summary>
    /// Acts as a concrete component upon which all decorators will rely.
    /// Defines default values and logic for most properties.
    /// </summary>
    public class PizzaBase : IPizza
    {
        // Set default cost to $7.50.
        private decimal _cost = 7.5M;
        private List<string> _toppings = new List<string>();

        // Set default cheese type to Mozzarella.
        public Cheese Cheese { get; } = Cheese.Mozzarella;
        // Allow Cost property to be overridden.
        public virtual decimal Cost { get { return _cost; } }
        // Set description to include cost, size, cheese, topping count, and topping list, as applicable.
        public string Description
        {
            get
            {
                // Get list of toppings as formatted string.
                var toppingsList = Toppings.IsAny() ? $" with {Toppings.Aggregate((index, topping) => index + ", " + topping)}" : "";
                return $"A {Size.ToString()}, {Toppings.Count()}-topping {Cheese.ToString()} pizza{toppingsList} for {String.Format("{0:C2}", Cost)}!";
            }
        }
        // Set default size to Large.
        public Size Size { get; } = Size.Large;
        // Allow Toppings property to be overriden.
        public virtual List<string> Toppings { get { return _toppings; } }

    }

    /// <summary>
    /// The base decorator on which others will be based.
    /// Ensures that all decorators can use and accept the IPizza interface.
    /// This is where logic used to *combine* decorator functionality takes place.
    /// </summary>
    public abstract class PizzaDecorator : PizzaBase
    {
        // Base interface component which is used to modify properties elsewhere.
        private IPizza _pizza;

        // Protected flag ensures the value can be modified in inherited decorators.
        protected decimal _cost;
        protected List<string> _toppings = new List<string>();

        // Cost if current instance added to passed instance cost.
        public override decimal Cost
        {
            get
            {
                return _cost + _pizza.Cost;
            }
        }

        // Combine current instance toppings with passed instance toppings.
        public override List<string> Toppings
        {
            get
            {
                return _pizza.Toppings.Concat(_toppings).ToList();
            }
        }

        // Constructor must set the private interface component to match the passed one, 
        // allowing future propert modifications to continue along the chain.
        protected PizzaDecorator(IPizza pizza)
        {
            _pizza = pizza;
        }
    }

    /// <summary>
    /// Base decorator for toppings.
    /// Handles setting the appropriate topping name
    /// by excluding "Decorator" portion of class string.
    /// </summary>
    public class ToppingDecorator : PizzaDecorator
    {
        public ToppingDecorator(IPizza pizza) : base(pizza)
        {
            // Get the name of the current class type while removing "Decorator"
            // and then adding that to the list of current toppings.
            _toppings.Add(GetType().Name.Replace("Decorator", null));
        }
    }

    public class MushroomsDecorator : ToppingDecorator
    {
        public MushroomsDecorator(IPizza pizza) : base(pizza)
        {
            // Set the topping cost.
            _cost = 2;
        }
    }

    public class OlivesDecorator : ToppingDecorator
    {
        public OlivesDecorator(IPizza pizza) : base(pizza)
        {
            // Set the topping cost.
            _cost = 1.5M;
        }
    }

    public class PepperoniDecorator : ToppingDecorator
    {
        public PepperoniDecorator(IPizza pizza) : base(pizza)
        {
            // Set the topping cost.
            _cost = 4;
        }
    }

    public class PineappleDecorator : ToppingDecorator
    {
        public PineappleDecorator(IPizza pizza) : base(pizza)
        {
            // Set the topping cost.
            _cost = 2.5M;
        }
    }
}
