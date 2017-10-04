# Behavioral Design Patterns: Template Method

Today we'll take a look at the **template method design pattern**, which is the final design pattern introduced in the well-known book 1994, [_Design Patterns: Elements of Reusable Object-Oriented Software_](https://en.wikipedia.org/wiki/Design_Patterns).  We've covered all other patterns thus far in our detailed [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series, with both real world and fully functional C# code samples of each, illustrating how the patterns can be used and implemented in your own code.

The `template method design pattern` is intended to outline the basic structure or "skeleton" of an algorithm, without explicitly defining (or advertising) the logic of each step within the overall structure to the client.  This pattern is ideal for complex algorithms that must be shared and executed by multiple classes, where each class must define their own specific implementation.

Let's get to exploring the `template method pattern` with both a real world example, as well as a functional C# sample!

## In the Real World

In terms of components, the `template method design pattern` is one of the simplest patterns we've examined, as it includes only two parts:

- `AbstractClass` - Defines a series of `primitive operations`, many of which inheriting `ConcreteClasses` must implement.  These operations are executed within the `TemplateMethod`, which is the primary method of the `AbstractClass` that is invoked by each instance of the `ConcreteClass`.  This `TemplateMethod` performs all the necessary operations (`primitive operations`) that makeup the main algorithm.
- `ConcreteClass` - Each `ConcreteClass` implements all the `primitive operation` methods that were declared in the `AbstractClass`.  When the client creates a new `ConcreteClass` instance, the `TemplateMethod` is invoked, which executes all `primitive operations` specific to this `ConcreteClass` implementation.

Since the entire purpose of the `template method` is to define a `TemplateMethod` that invokes a particular algorithm (i.e. a series of method calls), this pattern is frequently used in the real world, where a step-by-step process is required to complete a complex task.  For example, let's consider what might be required to get a new TV show up and running on a broadcast network or streaming service.  I'm no expert, but from a layman's perspective we might assume that broadcasting a new show requires these steps, in roughly the following order:

1. Write a script.
2. Pitch the script to a network.
3. Cast some actors.
4. Shoot the pilot.
5. Determine the best broadcast time slot.
6. Broadcast!

Now, undoubtedly some of these steps can occur at different points in the process, but, generally speaking, most of the time pitching will occur after a script as written, as will casting actors.  Certainly, the pilot episode cannot be shot if there are no actors nor a network to help with production.

Given that the process of creating and broadcasting a show is more or less an ordered "algorithm", it's a perfect example of the `template method design pattern` in real life.  The above steps are fairly generic and ordered, but the exact implementation of them will vary, depending on factors like the type of show, the network involved, how it is broadcast, and so forth.  Thus, each particular network acts as a unique `ConcreteClass`, implementing all the `primitive operation` steps to produce and broadcast a show that we see above, but doing so in their own way.

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using Utility;

namespace TemplateMethod
{
    internal class Program
    {
        private static void Main()
        {
            Logging.LineSeparator("THE OFFICE");
            var theOffice = new NBCShow("The Office", DayOfWeek.Thursday, 21);
            theOffice.Broadcast();

            Logging.LineSeparator("COMPUTERPHILE");
            var computerphile = new YouTubeShow("Computerphile");
            computerphile.Broadcast();

            Logging.LineSeparator("STRANGER THINGS");
            var strangerThings = new NetflixShow("Stranger Things");
            strangerThings.Broadcast();
        }
    }
}
```

```cs
using System;

namespace TemplateMethod
{
    internal class BroadcastSlot
    {
        public enum SlotType
        {
            Streaming,
            TimeSlot
        }

        private DayOfWeek Day { get; }
        private int Hour { get; }
        private int Minute { get; }

        /// <summary>
        /// Gets the string version of broadcast time.
        /// </summary>
        public string BroadcastTime => DateTime.Parse($"{Hour}:{Minute}:00").ToLongTimeString();

        public SlotType Type { get; }
        
        /// <summary>
        /// Creates a new BroadcastSlot with a default type of Streaming.
        /// </summary>
        public BroadcastSlot()
        {
            Type = SlotType.Streaming;
        }

        /// <summary>
        /// Specifies a specific day and timeslot for broadcast.
        /// </summary>
        /// <param name="day">Day of the week.</param>
        /// <param name="hour">Hour of the day.</param>
        /// <param name="minute">Minute of the hour.</param>
        public BroadcastSlot(DayOfWeek day, int hour, int minute)
        {
            Day = day;
            Hour = hour;
            Minute = minute;
            Type = SlotType.TimeSlot;
        }

        /// <summary>
        /// Gets BroadcastSlot as formatted string suitable for output.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString()
        {
            // If TimeSlot, output time and day, otherwise output streaming info.
            return Type == SlotType.TimeSlot ? $"{BroadcastTime} on {Day}" : $"all times [{Type}]";
        }
    }
}
```

```cs
using Utility;

namespace TemplateMethod
{
    internal abstract class Show
    {
        public BroadcastSlot BroadcastSlot { get; set; }
        public string Name { get; set; }
        public string Network { get; set; }

#region Abstract methods.
        // These methods must be overriden.
        public abstract void AssignBroadcastSlot();
        public abstract void FindNetwork();
#endregion

#region Default methods.
        // These methods can be left as their default implementations.
        public virtual void CastActors() => Logging.Log($"Casting actors for {Name}.");
        public virtual void ShootPilot() => Logging.Log($"Shooting pilot for {Name}.");
        public virtual void WriteScript() => Logging.Log($"Writing script for {Name}.");
#endregion

        protected Show(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Broadcasts show according to assigned properties.
        /// 
        /// Acts as 'Template Method' in Template Method pattern.
        /// </summary>
        public void Broadcast()
        {
            WriteScript();
            FindNetwork();
            CastActors();
            ShootPilot();
            AssignBroadcastSlot();

            // Output broadcasting message.
            Logging.Log($"Broadcasting {this}.");
        }

        /// <summary>
        /// Get formatted string representation of Show.
        /// </summary>
        /// <returns>Formatted Show information.</returns>
        public override string ToString()
        {
            return $"'{Name}' on {Network} at {BroadcastSlot}";
        }
    }
}
```

```cs
using System;
using Utility;

namespace TemplateMethod
{
    /// ReSharper disable once InconsistentNaming
    internal class NBCShow : Show
    {
        private BroadcastSlot Slot { get; }

        public override void AssignBroadcastSlot()
        {
            // Assign private Slot to public BroadcastSlot property.
            BroadcastSlot = Slot;
            Logging.Log($"{Name} broadcast slot set to {Slot}.");
        }

        public override void FindNetwork()
        {
            Network = "NBC";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create a new NBC show, broadcast on specified day and timeslot.
        /// </summary>
        /// <param name="name">Name of the show.</param>
        /// <param name="day">Day of broadcast.</param>
        /// <param name="hour">Hour of day.</param>
        /// <param name="minute">Minute of hour.</param>
        public NBCShow(string name, DayOfWeek day, int hour, int minute = 0) : base(name)
        {
            Slot = new BroadcastSlot(day, hour, minute);
        }
    }
}
```

```cs
using Utility;

namespace TemplateMethod
{
    internal class NetflixShow : Show
    {
        public override void AssignBroadcastSlot()
        {
            // Assign a default BroadcastSlot, 
            // which ensures the show is always available.
            BroadcastSlot = new BroadcastSlot();
            Logging.Log($"{Name} is a {BroadcastSlot.Type} broadcast.");
        }

        public override void FindNetwork()
        {
            Network = "Netflix";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create new Netflix show.
        /// </summary>
        /// <param name="name">Name of show.</param>
        public NetflixShow(string name) : base(name)
        {
        }
    }
}
```

```cs
using Utility;

namespace TemplateMethod
{
    internal class YouTubeShow : Show
    {
        public override void AssignBroadcastSlot()
        {
            // Assign a default BroadcastSlot, 
            // which ensures the show is always available.
            BroadcastSlot = new BroadcastSlot();
            Logging.Log($"{Name} is a {BroadcastSlot.Type} broadcast.");
        }

        public override void FindNetwork()
        {
            Network = "YouTube";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create new YouTube show.
        /// </summary>
        /// <param name="name">Name of show.</param>
        public YouTubeShow(string name) : base(name)
        {
        }
    }
}
```

```cs
// <Utility/>Logging.cs
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
            Output(value, outputType);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        public static void Log(string value, object arg0)
        {
            Debug.WriteLine(value, arg0);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public static void Log(string value, object arg0, object arg1)
        {
            Debug.WriteLine(value, arg0, arg1);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Log(string value, object arg0, object arg1, object arg2)
        {
            Debug.WriteLine(value, arg0, arg1, arg2);
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

            Output(value, outputType);
        }

        private static void Output(string value, OutputType outputType = OutputType.Default)
        {
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

## How It Works In Code

Our sample code continues with the television show example.  Specifically, we want to be able to broadcast a `Show` (`AbstractClass`), but doing so will depend on the particular `NetworkShow` (`ConcreteClass`) that inherits from `Show`.  The `Show` class has a single `Broadcast()` method, which acts as the `TemplateMethod` that invokes all the various `primitive operations` required to actually get the show created and broadcast.

Thus, we begin with the `Show` (`AbstractClass`):

```cs
using Utility;

namespace TemplateMethod
{
    internal abstract class Show
    {
        public BroadcastSlot BroadcastSlot { get; set; }
        public string Name { get; set; }
        public string Network { get; set; }

#region Abstract methods.
        // These methods must be overriden.
        public abstract void AssignBroadcastSlot();
        public abstract void FindNetwork();
#endregion

#region Default methods.
        // These methods can be left as their default implementations.
        public virtual void CastActors() => Logging.Log($"Casting actors for {Name}.");
        public virtual void ShootPilot() => Logging.Log($"Shooting pilot for {Name}.");
        public virtual void WriteScript() => Logging.Log($"Writing script for {Name}.");
#endregion

        protected Show(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Broadcasts show according to assigned properties.
        /// 
        /// Acts as 'Template Method' in Template Method pattern.
        /// </summary>
        public void Broadcast()
        {
            WriteScript();
            FindNetwork();
            CastActors();
            ShootPilot();
            AssignBroadcastSlot();

            // Output broadcasting message.
            Logging.Log($"Broadcasting {this}.");
        }

        /// <summary>
        /// Get formatted string representation of Show.
        /// </summary>
        /// <returns>Formatted Show information.</returns>
        public override string ToString()
        {
            return $"'{Name}' on {Network} at {BroadcastSlot}";
        }
    }
}
```

As mentioned, the `Broadcast()` method is the `TemplateMethod` that will be invoked by all object instances, so we see that it invokes a series of methods to accomplish this -- everything from `WriteScript()` to `AssignBroadcastSlot()`.  Once complete, it outputs a confirmation with formatted details of the `Show` that is being `Broadcast()`.

It's also worth noting that a few `#regions` are defined, just to illustrate the different method types we can use.  For this example, we've only chosen to explicitly implement the `AssignBroadcastSlot()` and `FindNetwork()` methods within all subclasses, so setting these to `abstract` will force this behavior.  Meanwhile, any methods we want to allow a default behavior for -- such as `CastActors()`, `ShootPilot()`, and `WriteScript()` -- are `virtual` methods, so they have their own base logic, which can be overriden if necessary.

To help handle the day of the week and time slot requirements for `Shows` that may need it, we've also created the helper `BroadcastSlot` class:

```cs
using System;

namespace TemplateMethod
{
    internal class BroadcastSlot
    {
        public enum SlotType
        {
            Streaming,
            TimeSlot
        }

        private DayOfWeek Day { get; }
        private int Hour { get; }
        private int Minute { get; }

        /// <summary>
        /// Gets the string version of broadcast time.
        /// </summary>
        public string BroadcastTime => DateTime.Parse($"{Hour}:{Minute}:00").ToLongTimeString();

        public SlotType Type { get; }
        
        /// <summary>
        /// Creates a new BroadcastSlot with a default type of Streaming.
        /// </summary>
        public BroadcastSlot()
        {
            Type = SlotType.Streaming;
        }

        /// <summary>
        /// Specifies a specific day and timeslot for broadcast.
        /// </summary>
        /// <param name="day">Day of the week.</param>
        /// <param name="hour">Hour of the day.</param>
        /// <param name="minute">Minute of the hour.</param>
        public BroadcastSlot(DayOfWeek day, int hour, int minute)
        {
            Day = day;
            Hour = hour;
            Minute = minute;
            Type = SlotType.TimeSlot;
        }

        /// <summary>
        /// Gets BroadcastSlot as formatted string suitable for output.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString()
        {
            // If TimeSlot, output time and day, otherwise output streaming info.
            return Type == SlotType.TimeSlot ? $"{BroadcastTime} on {Day}" : $"all times [{Type}]";
        }
    }
}
```

The comments outline most of the functionality here, but the basic purpose is to allow each `Show` instance to specify whether it's broadcast at a particular time on a specific day of the week, like most network shows, _or_ whether it's always available on a streaming service (such as YouTube or Netflix).  As we'll see in a moment, a new instance of `BroadcastSlot` is created within each `NetworkShow` (`ConcreteClass`) implementation's `AssignBroadcastSlot()` method, and then assigned to the `Show.BroadcastSlot` property.

Speaking of `ConcreteClasses`, now let's take a look at the three different `NetworkShow` implementations we've defined for this example, covering the networks of `NBC`, `Netflix`, and `YouTube`:

```cs
using System;
using Utility;

namespace TemplateMethod
{
    /// ReSharper disable once InconsistentNaming
    internal class NBCShow : Show
    {
        private BroadcastSlot Slot { get; }

        public override void AssignBroadcastSlot()
        {
            // Assign private Slot to public BroadcastSlot property.
            BroadcastSlot = Slot;
            Logging.Log($"{Name} broadcast slot set to {Slot}.");
        }

        public override void FindNetwork()
        {
            Network = "NBC";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create a new NBC show, broadcast on specified day and timeslot.
        /// </summary>
        /// <param name="name">Name of the show.</param>
        /// <param name="day">Day of broadcast.</param>
        /// <param name="hour">Hour of day.</param>
        /// <param name="minute">Minute of hour.</param>
        public NBCShow(string name, DayOfWeek day, int hour, int minute = 0) : base(name)
        {
            Slot = new BroadcastSlot(day, hour, minute);
        }
    }
}

using Utility;

namespace TemplateMethod
{
    internal class NetflixShow : Show
    {
        public override void AssignBroadcastSlot()
        {
            // Assign a default BroadcastSlot, 
            // which ensures the show is always available.
            BroadcastSlot = new BroadcastSlot();
            Logging.Log($"{Name} is a {BroadcastSlot.Type} broadcast.");
        }

        public override void FindNetwork()
        {
            Network = "Netflix";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create new Netflix show.
        /// </summary>
        /// <param name="name">Name of show.</param>
        public NetflixShow(string name) : base(name)
        {
        }
    }
}

using Utility;

namespace TemplateMethod
{
    internal class YouTubeShow : Show
    {
        public override void AssignBroadcastSlot()
        {
            // Assign a default BroadcastSlot, 
            // which ensures the show is always available.
            BroadcastSlot = new BroadcastSlot();
            Logging.Log($"{Name} is a {BroadcastSlot.Type} broadcast.");
        }

        public override void FindNetwork()
        {
            Network = "YouTube";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create new YouTube show.
        /// </summary>
        /// <param name="name">Name of show.</param>
        public YouTubeShow(string name) : base(name)
        {
        }
    }
}
```

As previously discussed, we've chosen to only require that two of our `primitive operation` methods be overriden, `AssignBroadcastSlot()` and `FindNetwork()`, since these operations will significantly change depending if the show is on a regular broadcast network or on an online streaming service.

Of particular note is the `NBCShow(string name, DayOfWeek day, int hour, int minute = 0)` constructor method, which forces the client to specify a broadcast day and time slot when creating a new `NBCShow`.  On the other hand, the `YouTubeShow(string name)` and `NetflixShow(string name)` constructor methods only require the `Show.Name` property to be specified, since the `BroadcastSlot` for both streaming services has a `Type` property set to `SlotType.Streaming`.  This ensures that the `Show` has no specific day of the week or time slot for broadcast, since such shows can be watched anytime.  Other than that, the `FindNetwork()` method just assigns the `Show.Network` property.  

Obviously, further refinement of this example might include refactoring the current default methods like `CastActors()`, but the above should properly illustrate the purpose and setup of the `template method design pattern`.

Now, to test all this out with client code we're creating just a handful of shows, one for each `NetworkShow` (`ConcreteClass`) implementation:

```cs
using System;
using Utility;

namespace TemplateMethod
{
    internal class Program
    {
        private static void Main()
        {
            Logging.LineSeparator("THE OFFICE");
            var theOffice = new NBCShow("The Office", DayOfWeek.Thursday, 21);
            theOffice.Broadcast();

            Logging.LineSeparator("COMPUTERPHILE");
            var computerphile = new YouTubeShow("Computerphile");
            computerphile.Broadcast();

            Logging.LineSeparator("STRANGER THINGS");
            var strangerThings = new NetflixShow("Stranger Things");
            strangerThings.Broadcast();
        }
    }
}
```

We start with _The Office_, which was on NBC and (mostly) aired on Thursdays at 9PM.  As we saw above, the `NBCShow` class requires that a day of the week,  hour, and optional minute be specified in its constructor, so that's where we pass the data indicating when the show will broadcast.  Once a new instance is created, we simply call `theOffice.Broadcast()`, which is the main `TemplateMethod` of all `Shows`.  This invokes all underlying `primitive operations` and gets our show on the air!  The output this produces can be seen below:

```
-------------- THE OFFICE --------------
Writing script for The Office.
Network (NBC) found for The Office.
Casting actors for The Office.
Shooting pilot for The Office.
The Office broadcast slot set to 9:00:00 PM on Thursday.
Broadcasting 'The Office' on NBC at 9:00:00 PM on Thursday.
```

As intended, we see an output message for each `primitive operation` method that was invoked by `Broadcast()`, along with a final message from `Broadcast()` itself that summarizes the show, the network, and when it broadcasts.

We're also creating and broadcasting [_Computerphile_](https://www.youtube.com/user/Computerphile) on YouTube, and _Stranger Things_ on Netflix.  Both of these networks are streaming services, so we don't need to specify a time slot.  This is confirmed with the output from executing both of these show creations:

```
------------ COMPUTERPHILE -------------
Writing script for Computerphile.
Network (YouTube) found for Computerphile.
Casting actors for Computerphile.
Shooting pilot for Computerphile.
Computerphile is a Streaming broadcast.
Broadcasting 'Computerphile' on YouTube at all times [Streaming].

----------- STRANGER THINGS ------------
Writing script for Stranger Things.
Network (Netflix) found for Stranger Things.
Casting actors for Stranger Things.
Shooting pilot for Stranger Things.
Stranger Things is a Streaming broadcast.
Broadcasting 'Stranger Things' on Netflix at all times [Streaming].
```

There we have it!  Hopefully this article provided a bit of insight into the purpose and implementation of the `template method design pattern`.  With that, our series comes to a close, but, for more information on all the other popular design patterns, head on over to our [detailed design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 23 of our Software Design Pattern series in which examine the template method design pattern using fully-functional C# example code.
