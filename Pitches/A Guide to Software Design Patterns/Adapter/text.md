# Structural Design Patterns: Adapter

Today we've finally made it to the `Structural` pattern section of our [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series, and to celebrate we'll be exploring the `adapter` design pattern (while possibly eating some delicious cake as well).  `Structural` patterns are used to configure how entities interact with and are related to one another.  Thus, the `adapter` pattern allows us to make two normally incompatible entities compatible with one another.

In this article we'll examine the `adapter` pattern in a bit more detail, illustrating some examples in both the real world as well as functional `C#` code.  Let's get to it!

## In the Real World

Whether we realize it or not, most of us use adapters (or converters) all the time.  The act of talking on your phone uses an `analog-to-digital` converter to convert the analog signal of your voice as picked up by a microphone into a digital signal.  And, of course, the reverse happens when you're hearing the voice of the person on the other end.

Speaking of phones, we all probably have at least one or two devices with a `USB` port to plug into a computer or a charger.  There are now so many different types of `USB` connections out there that many adapters exist to allow for some semblance of compatibility between them.

Anyone who's an avid gamer like myself might also have tried to find a way to use gamepads or controllers on a console they weren't originally designed for.  In many cases these experiments lead to third-party adapters to get the job done.  While these adapters may not be very pretty or reliable, they can save you fortune when compared to buying an entirely new controller for $50 a pop.

Many more examples of adapters exist, but the basic idea should be clear: An adapter provides us with a way to integrate two objects that are normally incompatible with one another.

## How It Works In Code

To illustrate an example of the `adapter` design pattern in code we'll continue with the gaming example above, where we have an `XBox` console but we want to use a `PlayStation` controller with it.  This requires some sort of adapter that will convert the signals from the PlayStation controller into the appropriate signals that our XBox expects.  With that in mind let's start with the full code example, then we'll examine it in more detail piece by piece.

```cs
using Utility;

namespace Adapter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create new XBox console.
            var xbox = new XBox();
            // Play with default controller.
            xbox.Play();
            // Create new PlayStationController.
            var playStationController = new PlayStationController();
            // Create controller adapter for PlayStation controller.
            var adapter = new ControllerAdapter(playStationController);
            // Play with adapted controller.
            xbox.Play(adapter);

            Logging.Log("-----------------");

            // Create new PlayStation console.
            var playstation = new PlayStation();
            // Play with default controller.
            playstation.Play();
            // Create new XBoxController.
            var xboxController = new XBoxController();
            // Create controller adapter for XBox controller.
            var adapter2 = new ControllerAdapter(xboxController);
            // Play with adapted controller.
            playstation.Play(adapter2);
        }
    }

    interface IController { }

    class Controller : IController { }

    class PlayStationController : Controller { }

    class XBoxController : Controller { }

    /// <summary>
    /// Used to adapt incompatible controllers within console calls.
    /// </summary>
    class ControllerAdapter
    {
        public Controller Controller { get; set; }

        public ControllerAdapter(Controller controller)
        {
            // Assign controller to adapter.
            Controller = controller;
            Logging.Log($"Using adapter on {controller.GetType().Name}.");
        }
    }

    interface IConsole
    {
        void Play();
    }

    class Console : IConsole
    {
        private Controller _Controller;

        /// <summary>
        /// Controller field with custom set method to output controller being activated.
        /// </summary>
        protected Controller Controller
        {
            get { return _Controller; }
            set
            {
                _Controller = value;
                Logging.Log($"Plugging {Controller.GetType().Name} into {this.GetType().Name} console.");
            }
        }

        public Console() { }

        /// <summary>
        /// Invoke Play call using default controller.
        /// </summary>
        public void Play()
        {
            Logging.Log($"Playing with {Controller.GetType().Name} on {this.GetType().Name} console.");
        }

        /// <summary>
        /// Invoke Play with associated adapter controller.
        /// </summary>
        /// <param name="adapter"></param>
        public void Play(ControllerAdapter adapter)
        {
            Controller = adapter.Controller;
            Logging.Log($"Playing with {Controller.GetType().Name} on {this.GetType().Name} console.");
        }
    }

    /// <summary>
    /// Basic XBox console.
    /// </summary>
    class XBox : Console
    {
        public XBox() {
            // Associate new XBoxController as default.
            Controller = new XBoxController();
        }
    }

    /// <summary>
    /// Basic PlayStation console.
    /// </summary>
    class PlayStation : Console
    {
        public PlayStation()
        {
            // Associate new PlayStationController as default.
            Controller = new PlayStationController();
        }
    }

}
```

At the most basic level we start with some controller classes to represent the unique controller types for both XBox and PlayStation.  These inherit from the `Controller` class, which itself inherits from `IController`.  For this example we don't actually need any fields, properties, or methods in these entities, so they're all empty and just represent the rudimentary objects that we'll use elsewhere in the example.  Obviously, in the real world we could add additional logic to these controllers if needed:

```cs
interface IController { }

class Controller : IController { }

class PlayStationController : Controller { }

class XBoxController : Controller { }
```

Next we need our adapter.  The `ControllerAdapter` class is relatively simple, but its purpose is not to be complex.  Instead, it merely acts as an `adapter` (often called a `wrapper` in many programming contexts) so we can use it to associate a `Controller` object with a `Console`.  As before, the logic is not very fleshed out here, but we could add additional logic within the `ControllerAdapter` method (or elsewhere in the class) that check which particular `type` of `Controller` object is passed in and make any necessary adaptation adjustments.  Here we're just opting to output a simple message indicating that the passed controller has been adapted.

```cs
/// <summary>
/// Used to adapt incompatible controllers within console calls.
/// </summary>
class ControllerAdapter
{
    public Controller Controller { get; set; }

    public ControllerAdapter(Controller controller)
    {
        // Assign controller to adapter.
        Controller = controller;
        Logging.Log($"Using adapter on {controller.GetType().Name}.");
    }
}
```

Next we have the `IConsole` interface which specifies a single `Play()` method:

```cs
interface IConsole
{
    void Play();
}
```

Our `Console` class then inherits from `IConsole`.  While not necessary, in this case we wanted to add some extra functionality to the `Controller.set()` method call by producing an output message that indicates which type of `Controller` is being plugged into the current console, so we used a private `_Controller` field.  It's also particularly important to note that we have set the access modifier of the `Console.Controller` property to `protected`.  This ensures that it cannot be publicly accessed by other entities (or outside its own scope), but that the `Controller` property can still be used within the `Console` class along with any inherited classes as well.

As with our controller classes the constructor of `Console` doesn't need to do anything to illustrate the `adapter` pattern, so we leave it blank.

Lastly, we have the `Play()` methods.  The first `Play()` definition assumes no arguments are passed and outputs a message indicating that "play" is executed using the currently assigned `Controller` on the current `Console`.  On the other hand, `Play(ControllerAdapter adapter)` is used when we want to play a game on our console using an incompatible controller.  In this case, the only way to do so is to pass a `ControllerAdapter` instance to the `Play()` method, which then assigns the `Controller` and outputs the same indication message as the previous `Play()` definition:

```cs
class Console : IConsole
{
    private Controller _Controller;

    /// <summary>
    /// Controller field with custom set method to output controller being activated.
    /// </summary>
    protected Controller Controller
    {
        get { return _Controller; }
        set
        {
            _Controller = value;
            Logging.Log($"Plugging {Controller.GetType().Name} into {this.GetType().Name} console.");
        }
    }

    public Console() { }

    /// <summary>
    /// Invoke Play call using default controller.
    /// </summary>
    public void Play()
    {
        Logging.Log($"Playing with {Controller.GetType().Name} on {this.GetType().Name} console.");
    }

    /// <summary>
    /// Invoke Play with associated adapter controller.
    /// </summary>
    /// <param name="adapter"></param>
    public void Play(ControllerAdapter adapter)
    {
        Controller = adapter.Controller;
        Logging.Log($"Playing with {Controller.GetType().Name} on {this.GetType().Name} console.");
    }
}
```

The final two class definitions are for our unique console classes, `XBox` and `PlayStation`.  These inherit from `Console` and therefore don't need to define much themselves, except that we assume each console will use its own respective `Controller` class by default, so we set that property during construction:

```cs
/// <summary>
/// Basic XBox console.
/// </summary>
class XBox : Console
{
    public XBox() {
        // Associate new XBoxController as default.
        Controller = new XBoxController();
    }
}

/// <summary>
/// Basic PlayStation console.
/// </summary>
class PlayStation : Console
{
    public PlayStation()
    {
        // Associate new PlayStationController as default.
        Controller = new PlayStationController();
    }
}
```

That's all the setup complete, so now we have three basic types of entities: Controllers, consoles, and an adapter.  Each controller is compatible with its matching console type (e.g. `XBox` console with `XBoxController`), but because we didn't want any way to directly access the `Controller` property for our `Console` class instances, the default controller assignment is handled during initialization of the `XBox` and `PlayStation` classes.

Here's our code to see this `adapter` pattern in action:

```cs
class Program
{
    static void Main(string[] args)
    {
        // Create new XBox console.
        var xbox = new XBox();
        // Play with default controller.
        xbox.Play();
        // Create new PlayStationController.
        var playStationController = new PlayStationController();
        // Create controller adapter for PlayStation controller.
        var adapter = new ControllerAdapter(playStationController);
        // Play with adapted controller.
        xbox.Play(adapter);

        Logging.Log("-----------------");

        // Create new PlayStation console.
        var playstation = new PlayStation();
        // Play with default controller.
        playstation.Play();
        // Create new XBoxController.
        var xboxController = new XBoxController();
        // Create controller adapter for XBox controller.
        var adapter2 = new ControllerAdapter(xboxController);
        // Play with adapted controller.
        playstation.Play(adapter2);
    }
}
```

We start by creating a new instance of our `XBox` console then call `xbox.Play()` to start playing using the default controller.  These first two lines produce the following output, telling us we've plugged in the (default) `XBoxController` into our console and then we're using the `XBoxController` within our `Play()` method call:

```
Plugging XBoxController into XBox console.
Playing with XBoxController on XBox console.
```

However, we want to use a PlayStation controller instead, so we start by creating a new instance of `PlayStationController`.  Now we need our adapter.  We create a new instance of `ControllerAdapter`, which expects a `Controller` class to be passed as the only argument.  Finally, we invoke the `xbox.Play(ControllerAdapter adapter)` method call by passing in our new `adapter`, which is set to use a PlayStation controller.  As a result, our output shows that the adapter was used on the new `PlayStationController` instance, then it was plugged into our `XBox` console, before we finally were able to begin playing with it:

```
Using adapter on PlayStationController.
Plugging PlayStationController into XBox console.
Playing with PlayStationController on XBox console.
```

This is the crux of the entire `adapter` pattern: We were able to take a PlayStation controller and adapt it to be used with our XBox console.  Moreover, we did so without any direct access to the `Controller` property of our `XBox : Console` class, and instead simply used a `ControllerAdapter` instance to perform the necessary compatibility changes behind the scenes.

Just to illustrate that this works both ways, the final few lines of code reverse everything to try using an `XBoxController` via an adapter with our `PlayStation` console:

```cs
Logging.Log("-----------------");

// Create new PlayStation console.
var playstation = new PlayStation();
// Play with default controller.
playstation.Play();
// Create new XBoxController.
var xboxController = new XBoxController();
// Create controller adapter for XBox controller.
var adapter2 = new ControllerAdapter(xboxController);
// Play with adapted controller.
playstation.Play(adapter2);
```

This works just fine illustrating that, once implemented, the `adapter` pattern can easily be expanded as much as necessary to meet your own project requirements:

```
-----------------
Plugging PlayStationController into PlayStation console.
Playing with PlayStationController on PlayStation console.
Using adapter on XBoxController.
Plugging XBoxController into PlayStation console.
Playing with XBoxController on PlayStation console.
```

---

__META DESCRIPTION__

Part 7 of our Software Design Pattern series in which examine the Adapter design pattern, along with working C# code examples.