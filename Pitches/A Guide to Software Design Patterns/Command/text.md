# Behavioral Design Patterns: Command

Not to be rude, but I hereby _command_ you to check out today's article in our ongoing [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series, in which we dive into the extremely useful `command design pattern` in all its glory!  As another `behavioral` pattern, the command design pattern makes it easy to add work to a queue, which can be processed by a client at any moment in time.  Furthermore, the client need not be aware of the underlying functionality or implementation of the queued work.

In this article we'll closely examine the `command design pattern`, exploring a real world example of the pattern in action, along with a fully-functional C# code sample intended to illustrate how the pattern can be used in your own coding endeavors.  Let's get this party started!

## In the Real World

To explain a real world example of the `command pattern`, we should first outline the basic components that make up the bulk of its logic.

- `Receiver` - Receives actions via `Commands`.
- `Command` - Binds an action to a `Receiver`.
- `Invoker` - Handles a collection of `Commands` and determines when `Commands` are executed.
- `Client` - Manages interactions between `Receiver / Command` and `Command / Invoker`.

While that's all very high level and abstract, it may begin to make sense when you consider that the **postal service** as a modern, real world example of the `command pattern` in action.  A recipient waiting to get some mail is a `receiver`.  Letters would be considered `commands`, each awaiting their time to be "executed," by being delivered to the appropriate recipient.  A mailman/mailwoman is obviously the `invoker`, handling the collection of letters (`commands`) and determining when they are delivered (executed).  The postoffice itself is, therefore, the `client`, as it determines which letters (`commands`) are assigned to which mailpersons (`invokers`).

Moreover, the `command pattern` is intended to make it easy to revert or undo a previous `command` action, if necessary.  As we all know, this also applies to postal deliveries, since a refused letter or package can easily be picked up and returned to sender, thereby reverting the initial `command` action.

## How It Works In Code

As with other `behavioral` pattern code samples, the `command pattern` example is somewhat complex at first glance.   We'll completely break down our sample below, so you can easily understand everything that's going on in the code.  As an avid gamer myself, I decided to implement a basic `Character` modification system for our `command design pattern` example.  Here's the basic rundown of the logic:

- `Character` class - Our `receiver`, which contains a few standard statistics (`Agility`, `Charisma`, and `Strength`) that we want to manipulate.
- `Modification` class - Our `command` class, which allows us to _modify_ a statistic for the associated `Character` instance.
- `ModificationManager` class - Our `invoker` class, which holds the `Modifications` queue and determines when queued `commands` should be invoked.
- `Program.Main(string[])` method - Our simple `client` class, which creates two `Characters`, a number of `Modifications`, then proceeds with executing and reverting them to see how `Character` statistics change.

That's the big picture of what we're going for here, so now we'll start with the full code sample for easy copy-pasting.  Afterward, we'll start digging into it to see exactly what's going on:

```cs
// <Statistics>/Statistic.cs
namespace Command.Statistics
{
    internal enum StatisticType
    {
        Agility,
        Charisma,
        Strength
    }

    internal interface IStatistic
    {
        decimal Value { get; set; }
    }

    internal class Strength : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    internal class Agility : IStatistic
    {
        public decimal Value { get; set; } = 0;
    }

    internal class Charisma : IStatistic
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
    internal class Character
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
            return Name;
        }
    }
}

// Modification.cs
using System;
using System.Reflection;
using Command.Statistics;
using Utility;

namespace Command
{
    internal enum Status
    {
        ExecuteFailed,
        ExecuteSucceeded,
        Queued,
        RevertFailed,
        RevertSucceeded
    }

    /// <summary>
    /// Defines all the fundamental properties and methods of modifications.
    /// 
    /// Acts as 'Command' within Command pattern.
    /// </summary>
    internal interface IModification
    {
        void Execute();
        Guid Id { get; set; }
        void Revert();
        Status Status { get; set; }
    }

    /// <summary>
    /// Base Modification class, used to alter Character Statistic values.
    /// 
    /// Acts as a 'ConcreteCommand' within Command pattern.
    /// </summary>
    internal class Modification : IModification
    {
        private readonly Character _character;
        private readonly StatisticType _statisticType;

        public Guid Id { get; set; } = Guid.NewGuid();
        public Status Status { get; set; } = Status.Queued;
        public readonly decimal Value;

        /// <summary>
        /// Get character statistic object.
        /// </summary>
        internal IStatistic CharacterStatistic => (IStatistic) 
            _character
            .GetType()
            .GetProperty(_statisticType.ToString())?
            .GetValue(_character);
        
        /// <summary>
        /// Get character statistic value property.
        /// </summary>
        internal PropertyInfo CharacterStatisticValueProperty =>
            CharacterStatistic?.GetType().GetProperty("Value");

        public Modification(Character character, StatisticType statisticType, decimal value)
        {
            _character = character;
            _statisticType = statisticType;
            Value = value;
        }

        /// <summary>
        /// Execute this modification.
        /// </summary>
        public void Execute()
        {
            Status = UpdateValue() ? Status.ExecuteSucceeded : Status.ExecuteFailed;

            // Output message.
            Logging.Log($"{Status} for modification {this}.");
        }

        /// <summary>
        /// Revert this modification.
        /// </summary>
        public void Revert()
        {
            Status = UpdateValue(true) ? Status.RevertSucceeded : Status.RevertFailed;

            // Output message.
            Logging.Log($"{Status} for modification {this}.");
        }

        /// <summary>
        /// Updates the value of the underlying Character Statistic property.
        /// </summary>
        /// <param name="isReversion">Indicates if this is a reversion command.</param>
        /// <returns>Indicates if update was successful.</returns>
        internal bool UpdateValue(bool isReversion = false)
        {
            try
            {
                // Return if property not set.
                if (CharacterStatisticValueProperty == null) return false;

                // Assign original and new values.
                var originalValue = CharacterStatistic.Value;
                var newValue = 0m;
                // Add values normally, but subtract if reversion.
                newValue = isReversion ? CharacterStatistic.Value - Value : CharacterStatistic.Value + Value;

                // Set modified value.
                CharacterStatisticValueProperty.SetValue(CharacterStatistic, newValue);

                // Output confirmation message.
                Logging.Log($"[{_character}] - '{CharacterStatistic.GetType().Name}' {(isReversion ? "reverted" : "modified")} from {originalValue} to {newValue}.");
            }
            catch (Exception)
            {
                return false;
            }
            // Return successful result.
            return true;
        }

        public override string ToString()
        {
            return $"[Id: {Id}, Statistic: {_statisticType}, Value: {Value}]";
        }
    }
}

// ModificationManager.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace Command
{
    /// <summary>
    /// Manages the modification queue and actions.
    /// 
    /// Acts as 'Invoker' within Command pattern.
    /// </summary>
    internal class ModificationManager
    {
        private readonly List<IModification> _queue = new List<IModification>();

        /// <summary>
        /// Checks if any modifications are queued.
        /// </summary>
        public bool HasQueue => _queue.Any(x =>
            x.Status == Status.Queued ||
            x.Status == Status.ExecuteFailed ||
            x.Status == Status.RevertFailed);

        /// <summary>
        /// Add modification to queue.
        /// </summary>
        /// <param name="modification"></param>
        public void AddModification(IModification modification)
        {
            _queue.Add(modification);
        }

        /// <summary>
        /// Process all outstanding modifications.
        /// </summary>
        public void ProcessQueue()
        {
            // Execute modifications that are queued or failed.
            foreach (var modification in _queue.Where(x =>
                x.Status == Status.Queued ||
                x.Status == Status.ExecuteFailed))
            {
                modification.Execute();
            }

            // Revert modifications that failed.
            foreach (var modification in _queue.Where(x =>
                x.Status == Status.RevertFailed))
            {
                modification.Revert();
            }
        }

        /// <summary>
        /// Revert passed modification, if found in queue.
        /// </summary>
        /// <param name="modification">Modification to revert.</param>
        public void RevertModification(IModification modification)
        {
            // Find match.
            var match = _queue.FirstOrDefault(x => x == modification);

            // Can't revert a modification not in the queue.
            if (match == null)
            {
                throw new ArgumentException($"Modification [{modification}] not found, cannot revert.");
            }

            // Can't revert unless execution already took place.
            if (match.Status != Status.ExecuteSucceeded)
            {
                throw new ArgumentException($"Modification [{modification}] 'Status' must be Status.ExecuteSucceeded to revert.");
            }

            // Revert modification.
            match.Revert();

            // Update status and remove from queue.
            if (match.Status == Status.RevertSucceeded)
            {
                _queue.Remove(match);
            }
        }

        /// <summary>
        /// Get modification by Id and pass to primary RevertModification method.
        /// </summary>
        /// <param name="id">Id of modification to revert.</param>
        public void RevertModification(Guid id)
        {
            RevertModification(_queue.FirstOrDefault(x => x.Id == id));
        }
    }
}

// Program.cs
using Command.Statistics;
using Xunit;

namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a manager.
            var manager = new ModificationManager();

            // Create a character with initial stats.
            var alice = new Character("Alice", 10, 14, 12);
            // Create another character with default stats.
            var bob = new Character("Bob");

            // Create some modifications for Alice.
            var agilityAlice = new Modification(alice, StatisticType.Agility, 8);
            var charismaAlice = new Modification(alice, StatisticType.Charisma, -4);
            var strengthAlice = new Modification(alice, StatisticType.Strength, 0.75m);

            // Create modifications for Bob.
            var agilityBob = new Modification(bob, StatisticType.Agility, 99.99m);
            var charismaBob = new Modification(bob, StatisticType.Charisma, -42);

            // Add modifications to queue.
            manager.AddModification(agilityAlice);
            manager.AddModification(strengthAlice);
            manager.AddModification(agilityBob);
            manager.AddModification(charismaBob);
            manager.AddModification(charismaAlice);

            // Process queue.
            manager.ProcessQueue();

            // Revert agility modification.
            manager.RevertModification(agilityAlice);

            // Confirm that we can revert in any order, regardless of queue order.
            Assert.Equal(bob.Charisma.Value, charismaBob.Value);
            manager.RevertModification(charismaBob);
            Assert.Equal(bob.Charisma.Value, 0);

            // Confirm that passing by Id also works.
            Assert.Equal(alice.Strength.Value, 12 + strengthAlice.Value);
            manager.RevertModification(strengthAlice.Id);
            Assert.Equal(alice.Strength.Value, 12);
        }
    }
}

// <Utility>/Logging.cs
using System;
using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
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
    }
}
```

---

The `Command.Statistics` namespace contains just a few statistic classes (`Strength`, `Agility`, and `Charisma`), each of which merely contain a `Value` property. They're not a fundamental part of the `command pattern`, so we won't go into anymore detail on those.  Instead, let's take a look at the `Character` class:

```cs
// Character.cs
using Command.Statistics;

namespace Command
{
    /// <summary>
    /// Stores basic character information, including statistics.
    /// 
    /// Acts as 'Receiver' within Command pattern.
    /// </summary>
    internal class Character
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
            return Name;
        }
    }
}
```

This is our core `Receiver` class, which is basically the object we'll be **acting upon** via `Modifications`.  As you can see, each `Character` just has a `Name` and a set of statistic properties, each of which have `Value` properties of their own that default to `0`.

Next, we move onto the `Modification` class.  However, before we do so, let's look at the `IModification` interface that `Modification` implements:

```cs
// Modification.cs
using System;
using System.Reflection;
using Command.Statistics;
using Utility;

namespace Command
{
    internal enum Status
    {
        ExecuteFailed,
        ExecuteSucceeded,
        Queued,
        RevertFailed,
        RevertSucceeded
    }

    /// <summary>
    /// Defines all the fundamental properties and methods of modifications.
    /// 
    /// Acts as 'Command' within Command pattern.
    /// </summary>
    internal interface IModification
    {
        void Execute();
        Guid Id { get; set; }
        void Revert();
        Status Status { get; set; }
    }

    // ...
}
```

One of the goals of our `command pattern` example is the ability to _revert_ modifications that have already been made.  This effectively allows the system to freely perform any modifications, in any order, and even to roll them back in any order as well, without confusing the logic or incorrectly modifying the underlying `Character` statistic values.  To that end, we'll be using the `Command.Status` enumeration values throughout our logic to assign and check the current stage of each `Modification`.

The `IModification` interface is really the _actual_ `Command` object we'll be using.  However, since it's an interface, we refer to classes that implement said interface as `ConcreteCommands`.  The fundamental methods we need are `Execute()` to trigger an action and `Revert()` to roll an action back.  We also store the `Status` and unique `Id`, in case we need to refer to a `Modification` elsewhere in code.

Now we get to the `Modification` class, which is ultimately a `Command`, although we could have multiple `Command` classes to implement the `IModification` interface, so calling `Modification` a `ConcreteCommand` is a bit more accurate in this context.

```cs
/// <summary>
/// Base Modification class, used to alter Character Statistic values.
/// 
/// Acts as a 'ConcreteCommand' within Command pattern.
/// </summary>
internal class Modification : IModification
{
    private readonly Character _character;
    private readonly StatisticType _statisticType;

    public Guid Id { get; set; } = Guid.NewGuid();
    public Status Status { get; set; } = Status.Queued;
    public readonly decimal Value;

    /// <summary>
    /// Get character statistic object.
    /// </summary>
    internal IStatistic CharacterStatistic => (IStatistic) 
        _character
        .GetType()
        .GetProperty(_statisticType.ToString())?
        .GetValue(_character);
    
    /// <summary>
    /// Get character statistic value property.
    /// </summary>
    internal PropertyInfo CharacterStatisticValueProperty =>
        CharacterStatistic?.GetType().GetProperty("Value");

    public Modification(Character character, StatisticType statisticType, decimal value)
    {
        _character = character;
        _statisticType = statisticType;
        Value = value;
    }

    /// <summary>
    /// Execute this modification.
    /// </summary>
    public void Execute()
    {
        Status = UpdateValue() ? Status.ExecuteSucceeded : Status.ExecuteFailed;

        // Output message.
        Logging.Log($"{Status} for modification {this}.");
    }

    /// <summary>
    /// Revert this modification.
    /// </summary>
    public void Revert()
    {
        Status = UpdateValue(true) ? Status.RevertSucceeded : Status.RevertFailed;

        // Output message.
        Logging.Log($"{Status} for modification {this}.");
    }

    /// <summary>
    /// Updates the value of the underlying Character Statistic property.
    /// </summary>
    /// <param name="isReversion">Indicates if this is a reversion command.</param>
    /// <returns>Indicates if update was successful.</returns>
    internal bool UpdateValue(bool isReversion = false)
    {
        try
        {
            // Return if property not set.
            if (CharacterStatisticValueProperty == null) return false;

            // Assign original and new values.
            var originalValue = CharacterStatistic.Value;
            var newValue = 0m;
            // Add values normally, but subtract if reversion.
            newValue = isReversion ? CharacterStatistic.Value - Value : CharacterStatistic.Value + Value;

            // Set modified value.
            CharacterStatisticValueProperty.SetValue(CharacterStatistic, newValue);

            // Output confirmation message.
            Logging.Log($"[{_character}] - '{CharacterStatistic.GetType().Name}' {(isReversion ? "reverted" : "modified")} from {originalValue} to {newValue}.");
        }
        catch (Exception)
        {
            return false;
        }
        // Return successful result.
        return true;
    }

    public override string ToString()
    {
        return $"[Id: {Id}, Statistic: {_statisticType}, Value: {Value}]";
    }
}
```

The `Modification` constructor method accepts a `Character`, a `StatisticType` (which is just an easy way to refer to `Agility`, `Charisma`, or `Strength` statistics), and a decimal `value`.  The `value` parameter represents the potential change in value for the passed `StatisticType` associated with the passed `Character` instance.  Fundamentally, that's all that a `Modification` does -- it changes one of the statistic property values of the passed `Character`.

This value change is performed in the `Execute()` method, which passes most of its logical behavior to the `UpdateValue(bool)` method.  This method uses the `CharacterStatisticValueProperty` and `CharacterStatistic` properties -- both of which use a bit of reflection magic to obtain their underlying represented object values -- to calculate and update that `new` underlying `Value`.  The `bool isReversion` parameter allows us to use `UpdateValue(bool)` for both `Execute()` and `Revert()` calls, with almost no logical difference between the two methods.

Next let's look at the `ModificationManager`, which is our `invoker` class.  It handles the `Modification` queue and determines when `Modifications` need to be executed, reverted, or ignored:

```cs
// ModificationManager.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace Command
{
    /// <summary>
    /// Manages the modification queue and actions.
    /// 
    /// Acts as 'Invoker' within Command pattern.
    /// </summary>
    internal class ModificationManager
    {
        private readonly List<IModification> _queue = new List<IModification>();

        /// <summary>
        /// Checks if any modifications are queued.
        /// </summary>
        public bool HasQueue => _queue.Any(x =>
            x.Status == Status.Queued ||
            x.Status == Status.ExecuteFailed ||
            x.Status == Status.RevertFailed);

        /// <summary>
        /// Add modification to queue.
        /// </summary>
        /// <param name="modification"></param>
        public void AddModification(IModification modification)
        {
            _queue.Add(modification);
        }

        /// <summary>
        /// Process all outstanding modifications.
        /// </summary>
        public void ProcessQueue()
        {
            // Execute modifications that are queued or failed.
            foreach (var modification in _queue.Where(x =>
                x.Status == Status.Queued ||
                x.Status == Status.ExecuteFailed))
            {
                modification.Execute();
            }

            // Revert modifications that failed.
            foreach (var modification in _queue.Where(x =>
                x.Status == Status.RevertFailed))
            {
                modification.Revert();
            }
        }

        /// <summary>
        /// Revert passed modification, if found in queue.
        /// </summary>
        /// <param name="modification">Modification to revert.</param>
        public void RevertModification(IModification modification)
        {
            // Find match.
            var match = _queue.FirstOrDefault(x => x == modification);

            // Can't revert a modification not in the queue.
            if (match == null)
            {
                throw new ArgumentException($"Modification [{modification}] not found, cannot revert.");
            }

            // Can't revert unless execution already took place.
            if (match.Status != Status.ExecuteSucceeded)
            {
                throw new ArgumentException($"Modification [{modification}] 'Status' must be Status.ExecuteSucceeded to revert.");
            }

            // Revert modification.
            match.Revert();

            // Update status and remove from queue.
            if (match.Status == Status.RevertSucceeded)
            {
                _queue.Remove(match);
            }
        }

        /// <summary>
        /// Get modification by Id and pass to primary RevertModification method.
        /// </summary>
        /// <param name="id">Id of modification to revert.</param>
        public void RevertModification(Guid id)
        {
            RevertModification(_queue.FirstOrDefault(x => x.Id == id));
        }
    }
}
```

The most important component of the `ModificationManager` is the `List<IModification> _queue` property.  This collection is used by every method within the class.  For example, the `HasQueue` property uses LINQ to check if any queued objects have either a failing `Status` or are `Queued` (indicating they haven't been processed yet).

The `AddModification(IModification)` method allows us to add `Modifications` to the queue.  Similarly, `RevertModification(IModification)` attempts to find the passed `Modification` in the queue.  If it's found and has recently been successfully executed, the `Revert()` method is called on that `Modification`, which rolls back the value of the `Character` statistic it's associated with.  The `RevertModification(Guid)` overload allows us to perform reversion based on `Id` instead of an actual `Modification` instance argument.

Lastly, the `ProcessQueue()` method will be used to perform the majority of logical processing of the queue.  This can be safely executed at any time, and simply finds any queued objects that recently failed or have never been executed.  This is why a distinction between the `Status.RevertFailed` and `Status.ExecuteFailed` enumeration values is important, since we can use the `Status` value to determine which action to take on the failed `Modification`.

Lastly, we get to actually _using_ our `command pattern` in some way, which is where the `Program.Main(string[])` method comes in.  This is effectively our `client` object:

```cs
// Program.cs
using Command.Statistics;
using Xunit;

namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a manager.
            var manager = new ModificationManager();

            // Create a character with initial stats.
            var alice = new Character("Alice", 10, 14, 12);
            // Create another character with default stats.
            var bob = new Character("Bob");

            // Create some modifications for Alice.
            var agilityAlice = new Modification(alice, StatisticType.Agility, 8);
            var charismaAlice = new Modification(alice, StatisticType.Charisma, -4);
            var strengthAlice = new Modification(alice, StatisticType.Strength, 0.75m);

            // Create modifications for Bob.
            var agilityBob = new Modification(bob, StatisticType.Agility, 99.99m);
            var charismaBob = new Modification(bob, StatisticType.Charisma, -42);

            // Add modifications to queue.
            manager.AddModification(agilityAlice);
            manager.AddModification(strengthAlice);
            manager.AddModification(agilityBob);
            manager.AddModification(charismaBob);
            manager.AddModification(charismaAlice);

            // Process queue.
            manager.ProcessQueue();

            // Revert agility modification.
            manager.RevertModification(agilityAlice);

            // Confirm that we can revert in any order, regardless of queue order.
            Assert.Equal(bob.Charisma.Value, charismaBob.Value);
            manager.RevertModification(charismaBob);
            Assert.Equal(bob.Charisma.Value, 0);

            // Confirm that passing by Id also works.
            Assert.Equal(alice.Strength.Value, 12 + strengthAlice.Value);
            manager.RevertModification(strengthAlice.Id);
            Assert.Equal(alice.Strength.Value, 12);
        }
    }
}
```

Nothing too crazy going on here.  We start by creating a `ModificationManager` instance, along with a couple `Characters`, and a number of `Modification` instances for assorted statistics and values.  Then, we add the `Modifications`, in an irrelevant order,  to the `manager` queue, before calling `manager.ProcessQueue()` to apply all the modifications.  The output, up to this point, shows everything working as expected:

```
[Alice] - 'Agility' modified from 10 to 18.
ExecuteSucceeded for modification [Id: 6a65df10-db5e-49aa-9aa3-abe3110af714, Statistic: Agility, Value: 8].

[Alice] - 'Strength' modified from 12 to 12.75.
ExecuteSucceeded for modification [Id: 4a926ef9-68a8-4f76-a10d-0541c74cf150, Statistic: Strength, Value: 0.75].

[Bob] - 'Agility' modified from 0 to 99.99.
ExecuteSucceeded for modification [Id: 0c7f10d5-d0b2-41f7-9042-caf162a79b54, Statistic: Agility, Value: 99.99].

[Bob] - 'Charisma' modified from 0 to -42.
ExecuteSucceeded for modification [Id: fada200a-fb2b-4b0d-8067-47460c5de055, Statistic: Charisma, Value: -42].

[Alice] - 'Charisma' modified from 14 to 10.
ExecuteSucceeded for modification [Id: 18f42f5e-f8be-419b-b2cb-21185db4c08b, Statistic: Charisma, Value: -4].
```

Now that the queue contains some `Modifications` we can revert any of these changes we need to, at any time.  In this case, our code does so right away, but our assertions verify that the appropriate statistic `Values` are reverting as expected.  We also confirm that we can call `RevertModification(Guid)` using an `Id` rather than a `Modification` instance still results in proper behavior.  Sure enough, the output (and passing tests) confirm everything is working:


```
[Alice] - 'Agility' reverted from 18 to 10.
RevertSucceeded for modification [Id: 6a65df10-db5e-49aa-9aa3-abe3110af714, Statistic: Agility, Value: 8].
[Bob] - 'Charisma' reverted from -42 to 0.
RevertSucceeded for modification [Id: fada200a-fb2b-4b0d-8067-47460c5de055, Statistic: Charisma, Value: -42].
[Alice] - 'Strength' reverted from 12.75 to 12.00.
RevertSucceeded for modification [Id: 4a926ef9-68a8-4f76-a10d-0541c74cf150, Statistic: Strength, Value: 0.75].
```

---

So there you have it!  A small but relatively robust example of the command design pattern in action.  For more information on all the other popular design patterns, head on over to our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 15 of our Software Design Pattern series in which examine the command design pattern using fully-functional C# example code.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Command_patternn