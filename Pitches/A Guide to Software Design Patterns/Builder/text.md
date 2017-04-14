# Creational Design Patterns: Builder Method

Today, our journey into the [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series takes us to yet another `Creational` design pattern: the `builder method`.  In this article we'll cover what the `builder method` is using both real world examples and functional `C#` code, so let's get to it!

## In the Real World

The simplest real world example of the `builder` pattern, which most of us are familiar with, is when we make (or order) a pizza.  The pizza toppings cannot be added in any random order or the whole thing is likely to come out a mess.  Instead, there is a step-by-step process that is followed.  This usually begins by deciding on the size of the pizza, and therefore how much dough to use.  Then the sauce is added, then cheese, then maybe some pepperoni and some olives, and so forth.  Yet, as any pizza-maker will tell you, not all orders are the same, and therefore, not all pizzas are built in the same way.  Each step in the process, and each topping that is added, can be thought of as a single step in the much larger `builder` pattern that is used to make that delicious pie you're about to devour.

## How It Works In Code

We'll keep the pizza analogy going with our code example for this article -- maybe we're planning on developing an online ordering application for that new local pizza joint!  In any case, to explore just how the `builder method` can help us, we must first examine a _different_, more common design pattern that is often used when working with objects that contain many `mutable` (changeable) properties.

A common technique in C# is to define our class and then define the `constructor` (the method with the same name as the class) with all the necessary parameters we might need to set properties for our class.  For example, here we have defined our `TelescopedPizza` class that we'll use to make pizzas, which has a wide range of properties including type of cheese, size, whether to include sauce or pepperoni or ham, and so forth:

```cs
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
                           bool ham)
    {
        Size = size;
        Cheese = cheese;
        Sauce = sauce;
        Pepperoni = pepperoni;
        Ham = ham;
    }
}
```

Some keen observers may notice that while our `TelescopedPizza` class has properties for `Olives` and `Mushrooms`, we haven't included a parameter for either in our `constructor`.  This is because we're trying to deal with the fundamental problem with this type of design pattern, which is indicated by our class name and is known as a `telescoping constructor`.

To see what we mean by the `telescoping constructor` pattern, here's our second `constructor` for our class:

```cs
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
```

Now we've added parameters to set both the `Olives` and `Mushrooms` properties, but immediately it becomes apparent that this practice could quickly spiral out of control.  While `C#` allows us to add as many `constructors` to our class as we'd like, so long as their `parameter signature` (the list of parameter types) is the same, if we wish to allow for all possible combinations of toppings for our pizza, it will be difficult to account for all of these with this pattern.  The list of parameters starts to expand or `telescope` outward with each new `constructor` that is defined, hence the name of this design (anti)pattern.

One common solution to this issue with `C#` is to use default values.  Here, we take our second `constructor` above, but add default values to each parameter:

```cs
public TelescopedPizza(Size size = Size.Large,
                       Cheese cheese = Cheese.Mozzarella,
                       bool sauce = true, 
                       bool pepperoni = false, 
                       bool ham = false, 
                       bool olives = false, 
                       bool mushrooms = false)
{
    Size = size;
    Cheese = cheese;
    Sauce = sauce;
    Pepperoni = pepperoni;
    Ham = ham;
    Olives = olives;
    Mushrooms = mushrooms;
}
```

This works in practice, since we can now define just about any combination when creating a new instance of our `TelescopedPizza` class, but it's rather cumbersome to use in practice.  One issue is that we have to retain (and use) the correct parameter order when creating a new instance:

```cs
// Create a Large with Mozzerella, Sauce, Pepperoni, and Olives.
var pizza = new TelescopedPizza(Size.Large, Cheese.Mozzarella, true, true, false, true)
```

In the above case, we're able to make exactly the kind of pizza we want by using the `constructor` that contains default values, but we have to remember the order and specify values even where we don't want that particular topping.  Since every parameter is default, we have to indicate that `ham` should be `false`, just to "get access to" the `olives` argument that we care about and set it to `true`.

Newer versions of C# allow us to use `named arguments`, which means we don't need to remember the positioning of parameters in the `constructor` list.  Thus, we could also perform the same instantiation like so:

```cs
// Create a Large with Mozzerella, Sauce, Pepperoni, and Olives.
var pizza = new TelescopedPizza(size: Size.Large, 
                                cheese: Cheese.Mozzarella, 
                                sauce: true, 
                                pepperoni: true, 
                                olives: true)
```

While this pattern is the most user-friendly method when using a `telescope-style` method, it's still not ideal.  For example, we might want to create our pizza object in code much the same way we'd build it in real life: step-by-step, with a bit of order.  After all, it (usually) doesn't make sense to add the sauce _after_ the cheese.  Bizarre, although still probably tasty!

This is where the `builder method` comes into play.  The `builder method` allows us to create our pizza step-by-step, adding each appropriate topping in whatever order we wish.  Best of all, as we'll see below, because each addition is separated from one another in the code, we can easily add unique logic to the step of adding cheese that is different from when we add olives, for example.

To implement the `builder method` for our pizza example, we first start with our secondary `builder` class, which we're calling `PizzaBuilder`:

```cs
public enum Cheese { Cheddar, Mozzarella, Parmesan, Provolone }
public enum Size { Small, Medium, Large, XLarge }

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
```

We're still using the same list of properties that we did before, but the magic comes in when we start to define some of our instance methods.  First, our `constructor` is very basic.  We've decided that every pizza will have a `size` of some sort, so the one and only parameter in our `constructor` is `size`, which has a default value of `Size.Large` from our `enum`:

```cs
public PizzaBuilder(Size size = Size.Large)
{
    Size = size;
}
```

Now we get to add the extra methods to our `builder`, which allows us to pick and choose which toppings we want, and in what order to add them.  Most of them are quite similar, so we'll just take a look at two, `AddCheese` and `AddSauce`:

```cs
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
```

These are quite similar to normal instance methods, but the key is that we've defined the `return type` value to be the same as the parent class, `PizzaBuilder`.  Then, after all logic is complete within the method, we `return this`, which returns the current instance of the `PizzaBuilder` class.  You may also notice that for all our properties that are merely `boolean` values, we just assume that each will default to `false` and, therefore, we only set the property value to `true` when we call the appropriate `Add...` method.

The last critical method in our `PizzaBuilder` is the `Build` method, which returns an instance of the `Pizza` class (that we'll define in a second), and which also accepts a single parameter: the current `PizzaBuilder` instance.

```cs
public Pizza Build()
{
    return new Pizza(this);
}
```

From here, defining our actual `Pizza` class is quite simple.  We just ensure the `constructor` expects a `PizzaBuilder` instance for the parameter, then we use the `properties` taken from that `PizzaBuilder` to assign values in our `Pizza` instance:

```cs
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
```

With everything defined, we can now make use of our new `PizzaBuilder` class like so:

```cs
// Set Medium size, add Sauce, add Provolone cheese, add Pepperoni, add Olives, then build.
var pizzaPepOlives = new PizzaBuilder(Size.Medium)
                         .AddSauce()
                         .AddCheese(Cheese.Provolone)
                         .AddPepperoni()
                         .AddOlives()
                         .Build();
Logging.Log(pizzaPepOlives);
```

Here we've created a pizza with a specific set of toppings, and we've added them in our desired order.  This technique of using multiple `dot method` calls in a row is called `method chaining`, and is possible in this case because of how we defined all of our `Add...` methods inside the `PizzaBuilder` class.  Since they each return the current `PizzaBuilder` instance, chaining them together like this simply executes the first call, then the next, then the next, each one using (and modifying) the current instance of the class.

Once we've added all our toppings as desired, we finally call the `Build()` method which, as we saw above, returns a new instance of `Pizza` that accepts our fully built `PizzaBuilder` and assigns all appropriate properties therein.  The end result of our log output is a `Pizza` with exactly the property values we want:

```
{Builder.Pizza}
  Size: Medium
  Cheese: Provolone
  Sauce: True
  Pepperoni: True
  Ham: False
  Olives: True
  Mushrooms: False
```

Best of all, since each `Add...` method is self-contained with its own parameters and logic, we can easily take advantage of our default values for cheese and pizza size by using the following call:

```cs
// Set Large size (default), add Sauce, add Mozzarella cheese (default), add Olives, add Mushrooms, then build it.
var pizzaOlivesMushrooms = new PizzaBuilder()
                          .AddSauce()
                          .AddCheese()
                          .AddOlives()
                          .AddMushrooms()
                          .Build();
Logging.Log(pizzaOlivesMushrooms);
```

Again, this builds a pizza just how we want it, including the default values of `Large` size and `Mozzarella` cheese:

```
{Builder.Pizza}
  Size: Large
  Cheese: Mozzarella
  Sauce: True
  Pepperoni: False
  Ham: False
  Olives: True
  Mushrooms: True
```

---

__META DESCRIPTION__

Part 4 of our Software Design Pattern series in which we cover the Builder Method design pattern, including C# example code.