# Structural Design Patterns: Decorator

Next up in our continued journey through common `Structural` design patterns, as part of our extensive [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series, we'll be exploring the `decorator` design pattern.  The `decorator` pattern allows the behavior of an individual instance of an object to be changed without impacting the behavior of other instance objects of the same type.

In this article we'll take a closer look at the `decorator` pattern with both a real world illustration and a fully-functional `C#` example showing how to use `decorators` in your own code.  Let's get this adventure going!

## In the Real World

One of the most common real world examples of the `decorator` pattern, and one which everyone reading this article can relate to, is the basic concept of your personal computer or smart phone; specifically how its behavior changes as you add and remove applications.

For example, if you're builing a new PC these days you'll likely start with an empty SSD on which you'll place your most critical applications.  At the outset your computer won't have anything on it and will just be a clean slate, but the first step is to add an operating system.  Once the operating system is installed suddenly the behavior and potential capabilities of your computer improve _dramatically_, allowing you to install all manner of additional applications.  If you're a developer you may want to to begin working using your new system, so you might install a framework or a code editor like Visual Studio (so you can create code like that we'll see later in this article!)

Lo and behold, your computer is a base component on which you've applied a handful of `decorators` (application installers), all of which have modified the behavior of your computer.  However, these installed applications are specific to _your computer_.  While these same applications can be (and often are) installed on other computers, we're showing a simple `decorator` pattern example here because your decision to install a particular application doesn't impact the behavior of _other_ computers you may have within your home or office.

## How It Works In Code

To illustrate the `decorator` pattern in code we're going to use a slightly different example than the real world one give above.  Instead of computers and application installation we're going to look at pizza and the act of adding ingredients to see how `decoration` works in code.  The reason for using pizza is not only because it's delicious, but also because the `decorator` pattern shares many similarities with the `builder` pattern, and we used pizza as our `builder` code example in [an article we published previously](https://airbrake.io/blog/design-patterns/builder-method).  Since we used pizza construction in the `builder` pattern article we're going to use pizza again for the `decorator`, allowing us to compare the two methods and see which is better.

As usual we'll start with the full example source code below and then take the time to examine what's going on in more detail afterward:  

```cs
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
            // Use the Builder pattern to compare it to Decorator.
            // Set Large size (default), add Sauce, add Mozzarella cheese (default),
            // add Olives, add Mushrooms, add Pepperoni, then build it.
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
            Logging.Log(pizza);

            // Decorate the base pizza by adding olives.
            var olives = new OlivesDecorator(pizza);
            Logging.Log(olives);

            // Add mushrooms.
            var mushrooms = new MushroomsDecorator(olives);
            Logging.Log(mushrooms);

            // Add pepperoni.
            var pepperoni = new PepperoniDecorator(mushrooms);
            Logging.Log(pepperoni);

            // Add pineapple.
            var pineapple = new PineappleDecorator(pepperoni);
            Logging.Log(pineapple);
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
```

Our `decorator` code example above also uses the `Builder` namespace which is seen below.  We won't go over this code again but if you are interested in further explanation check out our [`Builder` design pattern article](https://airbrake.io/blog/design-patterns/builder-method).

```cs
namespace Builder
{
    public enum Cheese { Cheddar, Mozzarella, Parmesan, Provolone }
    public enum Size { Small, Medium, Large, XLarge }

    public class Pizza
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

    public class PizzaBuilder
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
```

As for our `decorator` example we need to establish four basic types of objects:

- `Base Interface`: The base interface used by our `concrete components`.
- `Base Component`: The base object upon with `decorators` can be used to modify behavior.
- `Base Decorator`: The base interface used by our `concrete decorators`.
- `Concrete Decorator`: The additional functionality that will be applied to our `components`.

Thus, our code begins with the `base interface`, which we've defined here as the `IPizza` interface.  `IPizza` defines the basic properties or methods our `component` and `decorators` will use:

```cs
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
```

Next we have our `base component` of `PizzaBase`.  Here we use this component to specify all the necessary properties, giving many of them default values and basic logic.  Of particular note is the `Description` property, which outputs information about the toppings and cost of our current pizza instance.  We'll use `Description` later to verify our decorated pizzas are being modified.

```cs
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
```

The third part of our `decorator` pattern we need to add is the `base decorator`, which we've defined here as the `abstract PizzaDecorator` class:

```cs
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
```

This `base decorator` is where the full power of the `decorator` pattern begins to take shape.  The pattern takes advantage of two fundamental features of this object.  First, it's critical that the `constructor` of our `base decorator` accepts an instance of our `base interface` (`IPizza` in this case) as an argument.  This passed object instance is stored as a private variable for use elsewhere in the decorator.

Secondly, our `base decorator` should perform any basic logic which _merges_ behaviors or values from our `base component` (`PizzaBase`) and subsequent decorators.  In this case, we've specified the logic of `Cost.get()` to combine the `Cost` of the passed in `IPizza` object instance with the private `_cost` of our inherited `base component` (`PizzaBase`).  Similar logic is performed for the list of toppings in the `Toppings` property.  This ensures that any modifications to these two properties within inherited decorators will _propogate_ those values up through the chain, so we can keep track of all the changes that future decorations may provide.

The fourth and final component within our `decorator` pattern is the series of `concrete decorators`.  These are the independent classes which can be used to modify the behavior of our object however we see fit.  For this example we have four `concrete decorators`, all of which are used to add a particular ingredient to our pizza:

```cs
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
```

While not necessary, you may also notice that we snuck another layer of encapsulation in there with the `ToppingDecorator` class, which inherits from our `base decorator` of `PizzaDecorator`, and then is used to extend our four actual toppings `decorators`.  This extra level isn't necessary by any means, but, in this case, it allows our code to programmatically grab the ingredient name from our child classes without including the `"Decorator"` portion of the names.  This means our ingredient decorator classes, like `PepperoniDecorator`, only need to concern themselves with modifying one property directly, which is the extra `_cost` that ingredient adds to the total.

Obviously, this isn't a fully fleshed-out example -- in real world code we'd likely adjust the cost of a particular ingredient based on the `Size` of the pizza -- but for our purposes this serves well enough to illustrate how the `decorator` pattern works.

With our code ready to go we implement it in our `Program.Main()` method:

```cs
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
```

We start with an instance of our `base component` (`PizzaBase`), output that object to the log, and then pass it to one of our `concrete decorators` to create a new, _decorated_ instance.  In this case we add olives, then mushrooms, and so forth, each time outputting the modified object's `Description` to confirm each additional `decorator` has properly been applied.  If we look at the log output we can confirm the results:

```
A Large, 0-topping Mozzarella pizza for $7.50!
A Large, 1-topping Mozzarella pizza with Olives for $9.00!
A Large, 2-topping Mozzarella pizza with Olives, Mushrooms for $11.00!
A Large, 3-topping Mozzarella pizza with Olives, Mushrooms, Pepperoni for $15.00!
A Large, 4-topping Mozzarella pizza with Olives, Mushrooms, Pepperoni, Pineapple for $17.50!
```

By using the `decorator` pattern here we can modify a `base component` (or any other object that inherits from it) with as many `decorators` as we wish, and the changes will always propogate through the chain since we're always adding on or "modifying" the underlying object.  This is why the `Toppings` list and `Cost` properties adjust themselves with each new addition.

---

We previously mentioned that `decorator` and `builder` patterns are similar, so let's also briefly look at how we might create similar pizzas using the `builder` pattern:

```cs
// Set Large size (default), add Sauce, add Mozzarella cheese (default), add Olives, add Mushrooms, add Pepperoni, then build it.
var builderPizza = new PizzaBuilder()
                   .AddSauce()
                   .AddCheese()
                   .AddOlives()
                   .AddMushrooms()
                   .AddPepperoni()
                   .Build();
Logging.Log(builderPizza);
```

The log output is not formatted quite the same but we can see that each call to the appropriate `AddIngredient()` method, each of which is chained onto the `PizzaBuilder` class constructor, has produced the appropriate change to our pizza's ingredients:

```
{Builder.Pizza(HashCode:46104728)}
  Size: Large
  Cheese: Mozzarella
  Sauce: True
  Pepperoni: True
  Ham: False
  Olives: True
  Mushrooms: True
```

While both patterns work in this case, the fundamental difference between the `builder` and `decorator` patterns is that `builder` is aimed at modifying an object _during_ construction, while `decorator` is aimed at modifying an object _after_ construction.  In the above example we see that every chained method call made on the `PizzaBuilder` class happens during the construction phase of the object.  Once our `builderPizza` instance is created we can _no longer modify it_ with any of the `builder` functionality we have in place.

On the other hand, the `decorator` pattern has no means of specifying the entire list of ingredients during construction.  Instead, we must create a base instance and then add decorators one at a time on top of that base instance to get to the final pizza we're after.

From this we can conclude that neither pattern is better than the other, they're simply used in different ways and the best choice depends on the specific needs or design requirements of your application. 

---

__META DESCRIPTION__

Part 10 of our Software Design Pattern series in which examine the Decorator design pattern using fully-functional C# code examples.