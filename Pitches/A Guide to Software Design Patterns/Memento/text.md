# Behavioral Design Patterns: Memento

Moving along through our detailed [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series, today we land on the island of the **memento design pattern**.  The `memento pattern` is typically used to store the historical `state` of an object and, therefore, allow previous states to be restored at any time.  The pattern shares many similarities with the `command pattern`, which we explored in our previous [`Behavioral Design Patterns: Command Design Pattern`](https://airbrake.io/blog/design-patterns/behavioral-command-design-pattern) post.

Throughout today's article we'll explore the `memento design pattern` by looking at both a real world example and fully-functional C# code samples.  This code will illustrate how a `memento pattern` can be setup fairly easily, ideally providing you with some ideas for your own projects, so let's get going!

## In the Real World

The `memento design pattern` can be found in the real world almost everywhere we look.  In fact, essentially any object you interact with that can have a _singular state_ at a given moment in time, and which allows that state to be adjusted at will, is effectively an example of the `memento` pattern.

For example, consider the volume control of your television.  No matter whether you have an old CRT display or a state-of-the-art smart OLED display, the fundamental behavior of changing the volume works the same.  The television (or receiver, speakers, etc) has a saved `volume state`.  If the volume is set to `3`, and you can't quite hear Morgan Freeman's soft, dulcet tones, you'll probably turn the knob or press your remote to crank it up to `11`!  The act of changing that `volume state` from `3` to `11` is a simple form of the `memento pattern`, in which we're changing the _state_ of an object (the volume, in this case) to a new value.  Critically, we can also _revert_ that change at any time, going back to any `volume state` value we had previously.

For older televisions this process will be incremental, meaning we go from `3` to `4` to `5`, and so on.  Yet, on some newer televisions, we can explicitly choose a value to jump to immediately, making the change from `3` to `11` in a single step.  Regardless, both of these techniques are commonly used in `memento design pattern` implementations.  In some cases, undo and redo only occur in incremental fashion, using the most recent state as the fallback position.  In other cases, like the code implementation we'll look at below, reverting to previous states can occur in any order, allowing us to jump backwards in the revision history as many jumps as required.

## How It Works In Code

The `memento design pattern` has three key components:

- `Memento` - Simple object that contains basic `state` storage and retrieval capabilities.
- `Originator` - Gets and sets values of Mementoes.  Also, creates new Mementoes and assigns current values to them.
- `Caretaker` - Holds a collection that contains all previous Mementoes.  Can also store and retrieve Mementoes.

As discussed in the introduction, there is a clear similarity between the `memento` and `command` patterns.  The `command pattern` allows tasks to be assigned to a `queue`, which can then be processed by a client at any moment in time.  This structure makes the `command pattern` particularly well-suited for providing `undo` and `redo` functionality, since tasks can be reverted when desired.

The `memento design pattern` is far more abstract, but it also allows tasks to be reverted when necessary.  The distinct difference between the two patterns, however, is that the `memento pattern` is intended to change the _singular_ `state` of just one object.  On the other hand, the `command pattern` effectively handles multiple states simultaneously -- the logic and behavior of which is handled by a `Manager` class that orchestrates everything.  Check out our [`Command Design Pattern`](https://airbrake.io/blog/design-patterns/behavioral-command-design-pattern) article for more details.

Alright, let's get into the good stuff!  We'll start with the full code sample below, after which we'll break it down in more detail to see exactly how everything is put together:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using Command;
using Utility;

namespace Memento
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an originator.
            var originator = new Originator<Character>();
            // Create a caretaker with passed originator instance.
            var caretaker = new Caretaker<Character>(originator);

            // Create Alice character with default stats.
            var alice = new Character("Alice");
            // Create others characters with initial stats.
            var bob = new Character("Bob", 12, 10, 11);
            var christine = new Character("Christine", 25, -4, 0);

            // Set state to Alice.
            originator.SetState(alice);
            // Save state.
            caretaker.Save();

            // Set state to Bob.
            originator.SetState(bob);
            var bobMemento = caretaker.Save();

            // Set state to Christine.
            originator.SetState(christine);
            caretaker.Save();

            // Restore state back to Bob.
            caretaker.Restore(bobMemento);
        }
    }

    /// <summary>
    /// Simple object that allows us to store basic state value.
    /// </summary>
    /// <typeparam name="T">Type of State object.</typeparam>
    internal class Memento<T>
    {
        public T State { get; set; }

        public Memento() { }

        public Memento(T state)
        {
            State = state;
        }

        public override string ToString()
        {
            return State.ToString();
        }
    }

    /// <summary>
    /// Basic event args used to pass a Memento instance when event fires.
    /// </summary>
    /// <typeparam name="T">Object type of Memento.</typeparam>
    internal class MementoChangedEventArgs<T> : EventArgs
    {
        internal Memento<T> Memento { get; set; }
    }

    /// <summary>
    /// Gets/sets values of Mementoes.
    /// Creates new Mementoes and assigns current values to them.
    /// </summary>
    /// <typeparam name="T">Type of State object.</typeparam>
    internal class Originator<T>
    {
        private T _state;

        /// <summary>
        /// Creates a new Memento instance using _state.
        /// </summary>
        /// <returns>Newly generated Memento instance.</returns>
        public Memento<T> CreateMemento()
        {
            // Create memento and set state to current state.
            var memento = new Memento<T>(_state);

            // Invoke event handler.
            OnMementoChanged(new MementoChangedEventArgs<T>
            {
                Memento = memento,
            });

            return memento;
        }

        /// <summary>
        /// Set current state based on passed Memento.State property.
        /// </summary>
        /// <param name="memento">Memento from which to get State property.</param>
        public void SetMemento(Memento<T> memento)
        {
            _state = memento.State;

            // Invoke event handler.
            OnMementoChanged(new MementoChangedEventArgs<T>
            {
                Memento = memento,
            });
        }

        /// <summary>
        /// Explicitly set the state property.
        /// </summary>
        /// <param name="state">State object to set.</param>
        public void SetState(T state)
        {
            _state = state;
        }

        /// <summary>
        /// Handler for event when Memento is changed.
        /// </summary>
        public event EventHandler<MementoChangedEventArgs<T>> MementoChanged;

        /// <summary>
        /// Fires when Memento is changed.
        /// </summary>
        /// <param name="e">Event args containing Memento instance.</param>
        protected virtual void OnMementoChanged(MementoChangedEventArgs<T> e)
        {
            // Invoke event.
            MementoChanged?.Invoke(this, e);

            // Output log message with changed Memento.
            Logging.LineSeparator("MEMENTO CHANGED");
            Logging.Log(e.Memento.ToString());
        }
    }

    /// <summary>
    /// Holds a collection that contains all previous versions of the Memento.
    /// Can store and retrieve Mementos.
    /// </summary>
    /// <typeparam name="T">Type of Memento object.</typeparam>
    internal class Caretaker<T>
    {
        private static readonly List<Memento<T>> MementoList = new List<Memento<T>>();
        private Originator<T> Originator { get; set; }

        public Caretaker(Originator<T> originator)
        {
            Originator = originator;
        }

        /// <summary>
        /// Save Memento of Originator by creating new 
        /// Memento and adding to list.
        /// </summary>
        /// <returns>Created Memento instance.</returns>
        public Memento<T> Save()
        {
            var memento = Originator.CreateMemento();
            MementoList.Add(memento);
            return memento;
        }

        /// <summary>
        /// Restore Originator to Memento via passed list index.
        /// </summary>
        /// <param name="index">Index of Memento instance.</param>
        public void Restore(int index)
        {
            // Find match.
            var match = MementoList[index];

            // Can't restore if not in the list.
            if (match == null)
            {
                throw new ArgumentException($"Memento at index [{index}] not found, cannot restore.");
            }

            // Restore Memento.
            Originator.SetMemento(match);
        }

        /// <summary>
        /// Restore Originator to passed Memento state, if exists.
        /// </summary>
        /// <param name="memento">Memento to be restored to</param>
        public void Restore(Memento<T> memento)
        {
            // Find match.
            var match = MementoList.FirstOrDefault(x => x == memento);

            // Can't restore if not in the list.
            if (match == null)
            {
                throw new ArgumentException($"Memento [{memento}] not found, cannot restore.");
            }
            
            // Restore Memento.
            Originator.SetMemento(match);
        }
    }
}

// Character.cs
using Command.Statistics;

namespace Command
{
    /// <summary>
    /// Stores basic character information, including statistics.
    /// 
    /// Acts as 'Receiver' within Command pattern.
    /// </summary>
    public class Character
    {
        public string Name { get; set; }

        public Agility Agility { get; set; } = new Agility();
        public Charisma Charisma { get; set; } = new Charisma();
        public Strength Strength { get; set; } = new Strength();

        public Character(string name)
        {
            Name = name;
        }

        public Character(string name, decimal agility, decimal charisma, decimal strength)
        {
            Name = name;

            Agility.Value = agility;
            Charisma.Value = charisma;
            Strength.Value = strength;
        }

        public override string ToString()
        {
            return $"Character: {Name} [Agility: {Agility.Value}, Charisma: {Charisma.Value}, Strength: {Strength.Value}]";
        }
    }
}

// <Statistics>/Statistic.cs
namespace Command.Statistics
{
    public enum StatisticType
    {
        Agility,
        Charisma,
        Strength
    }

    internal interface IStatistic
    {
        decimal Value { get; set; }
    }

    public class Strength : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    public class Agility : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    public class Charisma : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }
}

// <Utility>/Logging.cs
using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        private const char SeparatorCharacterDefault = '-';
        private const int SeparatorLengthDefault = 40;

        /// <summary>
        /// Determines type of output to be generated.
        /// </summary>
        public enum OutputType
        {
            /// <summary>
            /// Default output.
            /// </summary>
            Default,
            /// <summary>
            /// Output includes timestamp prefix.
            /// </summary>
            Timestamp
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(string value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }
        
        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(Exception exception, bool expected = true, OutputType outputType = OutputType.Default)
        {
            var value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception}: {exception.Message}";

            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(Object)"/>.
        /// 
        /// ObjectDumper: http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object&amp;lt;/cref
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(object value, OutputType outputType = OutputType.Default)
        {
            if (value is IXmlSerializable)
            {
                Debug.WriteLine(value);
            }
            else
            {
                Debug.WriteLine(outputType == OutputType.Timestamp
                    ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                    : ObjectDumper.Dump(value));
            }
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            Debug.WriteLine(new string(@char, length));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>,
        /// with inserted text centered in the middle.
        /// </summary>
        /// <param name="insert">Inserted text to be centered.</param>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(string insert, int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            // Default output to insert.
            var output = insert;

            if (insert.Length < length)
            {
                // Update length based on insert length, less a space for margin.
                length -= insert.Length + 2;
                // Halve the length and floor left side.
                var left = (int) Math.Floor((decimal) (length / 2));
                var right = left;
                // If odd number, add dropped remainder to right side.
                if (length % 2 != 0) right += 1;

                // Surround insert with separators.
                output = $"{new string(@char, left)} {insert} {new string(@char, right)}";
            }
            
            // Output.
            Debug.WriteLine(output);
        }
    }
}
```

---

The example we're using here is based on some of the classes we saw in our [`command pattern`](https://airbrake.io/blog/design-patterns/behavioral-command-design-pattern) article, namely the `Character` class.  A `Character` might be used as a basic entity in a video game, containing both a `Name` and a collection of basic attributes: `Agility`, `Charism`, and `Strength`.  We'll be defining _generic_ `Memento`, `Originator`, and `Caretaker` classes that will all accept a generic object type `<T>`, which we can then specify as `Characters` we'll be creating.  The real world purpose of this `memento pattern` might be a game you're creating in which a single `Character` can be selected or active at a time, but you want to retain the history of all the `Characters` that have been used previously, and allow the player to revert to any previous `Character` at will.

Our goal here is to generalize the `memento pattern` implementation as much as possible, so we can reuse it for future implementations if desired.  Thus, we start with the `Memento<T>` class:

```cs
/// <summary>
/// Simple object that allows us to store basic state value.
/// </summary>
/// <typeparam name="T">Type of State object.</typeparam>
internal class Memento<T>
{
    public T State { get; set; }

    public Memento() { }

    public Memento(T state)
    {
        State = state;
    }

    public override string ToString()
    {
        return State.ToString();
    }
}
```

Nothing fancy going on here.  All a `Memento<T>` object really needs is some form of `state`, so we've defined the `T State` property.  Along with the constructor, this allows a new `Memento<T>` instance of any `type` to be created quite easily (e.g. `new Memento<Character>(new Character())`).

Next we have the `Originator<T>` class, which stores a local `T _state` property and updates this property based on the currently manipulated `Memento<T>` instance.  For extensibility, we also have a simple `OnMementoChanged` event handler, which allows us to fire some basic logic whenever the `Originator<T>` instance updates a `Memento<T>` in some way:

```cs
/// <summary>
/// Gets/sets values of Mementoes.
/// Creates new Mementoes and assigns current values to them.
/// </summary>
/// <typeparam name="T">Type of State object.</typeparam>
internal class Originator<T>
{
    private T _state;

    /// <summary>
    /// Creates a new Memento instance using _state.
    /// </summary>
    /// <returns>Newly generated Memento instance.</returns>
    public Memento<T> CreateMemento()
    {
        // Create memento and set state to current state.
        var memento = new Memento<T>(_state);

        // Invoke event handler.
        OnMementoChanged(new MementoChangedEventArgs<T>
        {
            Memento = memento,
        });

        return memento;
    }

    /// <summary>
    /// Set current state based on passed Memento.State property.
    /// </summary>
    /// <param name="memento">Memento from which to get State property.</param>
    public void SetMemento(Memento<T> memento)
    {
        _state = memento.State;

        // Invoke event handler.
        OnMementoChanged(new MementoChangedEventArgs<T>
        {
            Memento = memento,
        });
    }

    /// <summary>
    /// Explicitly set the state property.
    /// </summary>
    /// <param name="state">State object to set.</param>
    public void SetState(T state)
    {
        _state = state;
    }

    /// <summary>
    /// Handler for event when Memento is changed.
    /// </summary>
    public event EventHandler<MementoChangedEventArgs<T>> MementoChanged;

    /// <summary>
    /// Fires when Memento is changed.
    /// </summary>
    /// <param name="e">Event args containing Memento instance.</param>
    protected virtual void OnMementoChanged(MementoChangedEventArgs<T> e)
    {
        // Invoke event.
        MementoChanged?.Invoke(this, e);

        // Output log message with changed Memento.
        Logging.LineSeparator("MEMENTO CHANGED");
        Logging.Log(e.Memento.ToString());
    }
}

/// <summary>
/// Basic event args used to pass a Memento instance when event fires.
/// </summary>
/// <typeparam name="T">Object type of Memento.</typeparam>
internal class MementoChangedEventArgs<T> : EventArgs
{
    internal Memento<T> Memento { get; set; }
}
```

The important methods of `Originator<T>` are `CreateMemento()` and `SetMemento(Memento<T> memento)`.  The former creates a new `Memento<T>` instance based on the current `_state` property, while the latter updates the `_state` property based on the passed `Memento<T>` argument.

The final piece in the `memento pattern` trifecta is the `Caretaker<T>` class:

```cs
/// <summary>
/// Holds a collection that contains all previous versions of the Memento.
/// Can store and retrieve Mementos.
/// </summary>
/// <typeparam name="T">Type of Memento object.</typeparam>
internal class Caretaker<T>
{
    private static readonly List<Memento<T>> MementoList = new List<Memento<T>>();
    private Originator<T> Originator { get; set; }

    public Caretaker(Originator<T> originator)
    {
        Originator = originator;
    }

    /// <summary>
    /// Save Memento of Originator by creating new 
    /// Memento and adding to list.
    /// </summary>
    /// <returns>Created Memento instance.</returns>
    public Memento<T> Save()
    {
        var memento = Originator.CreateMemento();
        MementoList.Add(memento);
        return memento;
    }

    /// <summary>
    /// Restore Originator to Memento via passed list index.
    /// </summary>
    /// <param name="index">Index of Memento instance.</param>
    public void Restore(int index)
    {
        // Find match.
        var match = MementoList[index];

        // Can't restore if not in the list.
        if (match == null)
        {
            throw new ArgumentException($"Memento at index [{index}] not found, cannot restore.");
        }

        // Restore Memento.
        Originator.SetMemento(match);
    }

    /// <summary>
    /// Restore Originator to passed Memento state, if exists.
    /// </summary>
    /// <param name="memento">Memento to be restored.</param>
    public void Restore(Memento<T> memento)
    {
        // Find match.
        var match = MementoList.FirstOrDefault(x => x == memento);

        // Can't restore if not in the list.
        if (match == null)
        {
            throw new ArgumentException($"Memento [{memento}] not found, cannot restore.");
        }
        
        // Restore Memento.
        Originator.SetMemento(match);
    }
}
```

The primary purpose of the caretaker object is to store the historical list of mementoes that have been created, effectively containing all the `state` values that have ever existed.  The `Save()` method handles the creation of a new `Memento<T>` instance within the `Originator` property value.  Conversely, `Restore(Memento<T> memento)` searches for the passed `memento` instance in the full list and sets the current memento (i.e. `state`) of `Originator` to that value, if its found in the list.

That's the whole of the `memento pattern` setup.  Now, for implementation we use the aforementioned `Character` class, along with the statistic classes implementing the `IStatistic` interface that it uses:

```cs
// <Statistics>/Statistic.cs
namespace Command.Statistics
{
    public enum StatisticType
    {
        Agility,
        Charisma,
        Strength
    }

    internal interface IStatistic
    {
        decimal Value { get; set; }
    }

    public class Strength : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    public class Agility : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    public class Charisma : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }
}

// Character.cs
using Command.Statistics;

namespace Command
{
    /// <summary>
    /// Stores basic character information, including statistics.
    /// 
    /// Acts as 'Receiver' within Command pattern.
    /// </summary>
    public class Character
    {
        public string Name { get; set; }

        public Agility Agility { get; set; } = new Agility();
        public Charisma Charisma { get; set; } = new Charisma();
        public Strength Strength { get; set; } = new Strength();

        public Character(string name)
        {
            Name = name;
        }

        public Character(string name, decimal agility, decimal charisma, decimal strength)
        {
            Name = name;

            Agility.Value = agility;
            Charisma.Value = charisma;
            Strength.Value = strength;
        }

        public override string ToString()
        {
            return $"Character: {Name} [Agility: {Agility.Value}, Charisma: {Charisma.Value}, Strength: {Strength.Value}]";
        }
    }
}
```

The `Program.Main(string[] args)` method actually tests all this code out for us:

```cs
static void Main(string[] args)
{
    // Create an originator.
    var originator = new Originator<Character>();
    // Create a caretaker with passed originator instance.
    var caretaker = new Caretaker<Character>(originator);

    // Create Alice character with default stats.
    var alice = new Character("Alice");
    // Create others characters with initial stats.
    var bob = new Character("Bob", 12, 10, 11);
    var christine = new Character("Christine", 25, -4, 0);

    // Set state to Alice.
    originator.SetState(alice);
    // Save state.
    caretaker.Save();

    // Set state to Bob.
    originator.SetState(bob);
    var bobMemento = caretaker.Save();

    // Set state to Christine.
    originator.SetState(christine);
    caretaker.Save();

    // Restore state back to Bob.
    caretaker.Restore(bobMemento);
}
```

As indicated by the comments, the above code starts by creating a new `Originator<Character>` instance, then passes that to be used by the new `Caretaker<Character>` instance.  Next, we create a few `Character` instances to represent our three different characters, giving each unique names and attribute scores.

Now comes the `memento pattern` usage.  We take our `originator` instance and call `SetState(Memento<T> memento)`, passing in our first `Character` instance.  Behind the scenes, this sets the `state` of the `originator` instance to be equal to that passed in `alice` `Character` value.  Then we call `caretaker.Save()`, which adds the `alice` memento to the stored `MementoList` property.  This also triggers our `OnMementoChanged` event, so we produce a little bit of output to confirm what is going on.  We repeat this process a couple more times, changing the saved `state` first to `bob`, then to `christine`.

Finally, we call `caretaker.Restore(bobMemento)`, which effectively checks if `bob` can be found in the underlying `MementoList` property.  If so, it _restores_ the `state` back to the state of when `bob` was added, then fires our `OnMementoChanged` event again to provide some output.

The result of all this code is that we should see three state changes as each `Character` instance is saved as the `state`, then a final reversion back to the `state` of the second `bob` `Character`.  Sure enough, that's exactly what we find in the output:

```
----------- MEMENTO CHANGED ------------
Character: Alice [Agility: 0, Charisma: 0, Strength: 0]
----------- MEMENTO CHANGED ------------
Character: Bob [Agility: 12, Charisma: 10, Strength: 11]
----------- MEMENTO CHANGED ------------
Character: Christine [Agility: 25, Charisma: -4, Strength: 0]
----------- MEMENTO CHANGED ------------
Character: Bob [Agility: 12, Charisma: 10, Strength: 11]
```

---

There you have it!  I hope this provided a bit of insight into the potential power of the memento design pattern, and gave you some basic tools you can use in your own projects to implement it where it seems most appropriate.  For more information on all the other popular design patterns, head on over to our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 18 of our Software Design Pattern series in which examine the memento design pattern using fully-functional C# example code.