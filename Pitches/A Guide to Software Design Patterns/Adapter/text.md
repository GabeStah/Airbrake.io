# Structural Design Patterns: Singleton

Today we've finally made it to the `Structural` pattern section of our [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series, and to celebrate we'll be exploring the `adapter` design pattern.  `Structural` patterns are used to configure how entities interact with and are related to one another.  Thus, the `adapter` pattern allows us to make an entity compatible with another entity, which it would normally be incompatible with.

In this article we'll examine the `adapter` pattern in a bit more detail, illustrating an example in the real world as well as functional `C#` code.  Let's get to it!

## In the Real World

In the real world adapters (or converters) are used all the time.  The act of talking on your phone uses an `analog-to-digital` converter of some sort to convert the analog signal of your voice against the microphone into a digital signal (and the reverse happens when producing the voice of the person on the other end).  Speaking of phones, we all probably have at least one or two devices with a `USB` port to plug into a computer or a charger.  There are now so many different types of `USB` connections out there that many adapters exist to allow for some semblance of compatibility between them.

Anyone who's an avid gamer like myself might also have tried to find a way to use gamepads or controllers designed for one console on a different console, and had to resort to a third-party adapter to get the job done.  It's often not very pretty, but it can save a fortune when compared to buying an entirely new controller for $50.

Many more examples exist, of course, but the basic idea of an adapter should be clear: It provides us with a way to use two objects which are normally incompatible with one another.

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
        Controller Controller { get; set; }
        void Play();
    }

    class Console : IConsole
    {
        private Controller _Controller;

        /// <summary>
        /// Controller field with custom set method to output controller being activated.
        /// </summary>
        public Controller Controller
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

At the most basic level we start with some controller classes to represent the unique controller types for both XBox and PlayStation.  These inherit from the `Controller` class, which itself inherits from `IController`.  For this example we don't actually need any fields, properties, or methods in these entities, so they're all empty and just represent the rudimentary objects that we'll use for now elsewhere in the example.  Obviously, in the real world we could add additional logic to these controllers if needed:

```cs
interface IController { }

class Controller : IController { }

class PlayStationController : Controller { }

class XBoxController : Controller { }
```

Next we need our adapter.  The `ControllerAdapter` class is relatively simple, but its purpose is not to be complex.  Instead, it merely acts as an `adapter` (often called a `wrapper` in many programming contexts) so we can use it to associate a `Controller` object with a `Console`.  As before, the logic is not very fleshed out here, but we could obviously add some sort of checks within the `ControllerAdapter` method (or elsewhere in the class) that check which particular `type` of `Controller` object is passed in and make adaptation adjustments as necessary to ensure it works.  Here we're just opting to output a simple message indicating that the passed controller has been adapted.

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

Next we have the `IConsole` interface which specifics a `Controller` property and a single `Play()` method:

```cs
interface IConsole
{
    Controller Controller { get; set; }
    void Play();
}
```

Our `Console` class then inherits from `IConsole`.  While not necessary, in this case we wanted to add some extra functionality to the `Controller.set()` method call by producing an output message that indicates which type of `Controller` is being plugged into the current console, so we had to use a custom private `_Controller` field (rather than default).

As with our controller classes the constructor of `Console` doesn't need to do anything to illustrate the `adapter` pattern, so we leave it blank.

Lastly, we have the `Play()` methods.  The first `Play()` definition assumes no arguments are passed and outputs a message indicating that "play" is executed using the currently assigned `Controller` on the current `Console`.  On the other hand, `Play(ControllerAdapter adapter)` is used when we want to play a game on our console using an incompatible controller.  In this case, the only way to do so is to pass an `ControllerAdapter` instance to the `Play()` method, which then assigns the `Controller` and outputs the same indication message as the previous `Play()` definition:


```cs
class Console : IConsole
{
    private Controller _Controller;

    /// <summary>
    /// Controller field with custom set method to output controller being activated.
    /// </summary>
    public Controller Controller
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

That's all the setup complete, so now we have three basic types of entities: Controllers, consoles, and an adapter.  Each controller is compatible with its matching console type (e.g. `XBox` console with `XBoxController`), but our `Console` classes don't 


---

__META DESCRIPTION__

Part 7 of our Software Design Pattern series in which examine the Adapter design pattern, along with working C# code examples.