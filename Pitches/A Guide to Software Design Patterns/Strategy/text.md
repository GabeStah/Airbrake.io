# Behavioral Design Patterns: Strategy

Next up in our in-depth [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series we'll dig into the **strategy design pattern**.  The `strategy pattern` is ideal when code should programmatically determine which algorithm (or function, method, or so forth) should be executed at _runtime_.

In this article we'll look into a real world example of the `strategy design pattern`, along with a fully-functional C# code sample illustrating how it can be implemented, so let's get to it!

## In the Real World

The `strategy pattern`, which is sometimes called a `policy pattern`, consists of three basic components:

- `Strategy` - A interfaced implementation of the core algorithm.
- `Concrete Strategy` - An actual implementation of the core algorithm, to be passed to the `Client`.
- `Client` - Stores a local `Strategy` instance, which is used to perform the core algorithm of that strategy.

The goal of the `strategy design pattern` is to allow the `Client` to perform the core algorithm, based on the locally-selected `Strategy`.  In so doing, this allows different objects or data to use different strategies, independently of one another.

To better explain, let's consider the real world example of a postal service packaging and mailing out various objects.  The characteristics of a given object will determine what packing materials are necessary to safely ship it.  That is to say, the packaging required to send a letter will differ dramatically from the packaging necessary to send a watermelon or a saxophone.

These different forms of packaging up objects can be thought of as unique `packaging strategies`.  The strategy for most paper mail is typically envelopes and stamps, while the strategy for packaging perishable food may require a box, packing foam, and possibly even dry ice.  The power of the `strategy design pattern` in this scenario is that the post office can look at each individual object, and implement the most suitable strategy to package it safely and efficiently.

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
// Program.cs
using Utility;

namespace Strategy
{
    internal class Program
    {
        private static void Main()
        {
            var bear = new Package("A teddy bear");
            var defaultPackager = new Packager(new DefaultStrategy());
            defaultPackager.Pack(bear);
            Logging.Log(bear.ToString());

            var monitor = new Package("A computer monitor");
            var packager = new Packager(new FragileStrategy());
            packager.Pack(monitor);
            Logging.Log(monitor.ToString());

            var fish = new Package("Some salmon filets");
            packager.Pack(fish, new PerishableStrategy());
            Logging.Log(fish.ToString());

            var massiveBear = new Package("A MASSIVE teddy bear");
            packager.Pack(massiveBear, new OversizedStrategy());
            Logging.Log(massiveBear.ToString());
        }
    }
}
```

```cs
// Package.cs
using System.Collections.Generic;

namespace Strategy
{
    /// <summary>
    /// Basic packing materials, which are used within packaging strategies.
    /// </summary>
    internal enum PackingMaterials
    {
        Box,
        BubbleWrap,
        DryIce,
        Envelope,
        Foam,
        LargeBox,
        Tape
    }

    /// <summary>
    /// Defines a basic package to be shipped.
    /// 
    /// Contains package content and packing materials used.
    /// </summary>
    internal class Package
    {
        internal string Content { get; set; }
        internal List<PackingMaterials> Packaging { get; set; } = new List<PackingMaterials>();

        internal Package(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Outputs package Content and comma-delimited list of packing materials.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString()
        {
            return $"{Content} was packaged using {string.Join(", ", Packaging.ToArray())}.";
        }
    }
}
```

```cs
// PackagingStrategy.cs
namespace Strategy
{
    /// <summary>
    /// Strategy interface used to pack all packages.
    /// </summary>
    internal interface IPackagingStrategy
    {
        void Pack(Package package);
    }

    /// <summary>
    /// Default packaging strategy.
    /// 
    /// Abstract class forces this class to be inherited.
    /// </summary>
    internal abstract class PackagingStrategy : IPackagingStrategy
    {
        public virtual void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.BubbleWrap);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }
}
```

```cs
// PackagingStrategies.cs
namespace Strategy
{
    /// <summary>
    /// Default packaging strategy.
    /// 
    /// Uses box, bubble wrap, and tape.
    /// </summary>
    internal class DefaultStrategy : PackagingStrategy { }

    /// <summary>
    /// Strategy for flat objects, such as letters.
    /// 
    /// Uses envelope.
    /// </summary>
    internal class FlatStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Envelope);
        }
    }

    /// <summary>
    /// Strategy for fragile objects, such as glassware.
    /// 
    /// Uses box, bubble wrap, foam, and tape.
    /// </summary>
    internal class FragileStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.BubbleWrap);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for perishables, such as food.
    /// 
    /// Uses box, dry ice, foam, and tape.
    /// </summary>
    internal class PerishableStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.DryIce);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for pliable objects, such as clothing.
    /// 
    /// Uses envelope and tape.
    /// </summary>
    internal class PliableStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Envelope);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for oversized objects, such as furniture.
    /// 
    /// Uses large box, foam, and tape.
    /// </summary>
    internal class OversizedStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.LargeBox);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }
}
```

```cs
// Packager.cs
using Utility;

namespace Strategy
{
    /// <summary>
    /// Client that routes all packages to the passed packaging strategy.
    /// </summary>
    internal class Packager
    {
        protected IPackagingStrategy Strategy;

        public Packager(IPackagingStrategy strategy)
        {
            Strategy = strategy;
        }

        /// <summary>
        /// Packs the passed Package, using the existing strategy.
        /// </summary>
        /// <param name="package"></param>
        public void Pack(Package package)
        {
            // Output the current strategy to the log.
            Logging.LineSeparator(Strategy.GetType().Name);

            // Pack the package using current strategy.
            Strategy.Pack(package);
        }

        /// <summary>
        /// Packs the passed Package, using the passed strategy.
        /// </summary>
        /// <param name="package">Package to pack.</param>
        /// <param name="strategy">Strategy to use.</param>
        public void Pack(Package package, IPackagingStrategy strategy)
        {
            // Assign to local strategy.
            Strategy = strategy;

            // Pass to default Pack method.
            Pack(package);
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

The code sample continues with the packaging analogy and illustrates how code might be used to differentiate between various packaging strategies, which are used based on the type of object that is being shipped.

We begin with the basic `Package` class, which is not a key component to the `strategy pattern`, but merely allows us to represent individual packages as they work their way through the system.  A `Package` is our primary form of data.

```cs
/// <summary>
/// Basic packing materials, which are used within packaging strategies.
/// </summary>
internal enum PackingMaterials
{
    Box,
    BubbleWrap,
    DryIce,
    Envelope,
    Foam,
    LargeBox,
    Tape
}

/// <summary>
/// Defines a basic package to be shipped.
/// 
/// Contains package content and packing materials used.
/// </summary>
internal class Package
{
    internal string Content { get; set; }
    internal List<PackingMaterials> Packaging { get; set; } = new List<PackingMaterials>();

    internal Package(string content)
    {
        Content = content;
    }

    /// <summary>
    /// Outputs package Content and comma-delimited list of packing materials.
    /// </summary>
    /// <returns>Formatted string.</returns>
    public override string ToString()
    {
        return $"{Content} was packaged using {string.Join(", ", Packaging.ToArray())}.";
    }
}
```

We also use the `PackingMaterials` to store some common materials that we'll use within our packaging strategies.  The ultimate goal is to assign a list of `PackingMaterials` to each unique `Package`, as determined by the packaging strategy that is implemented.

Speaking of which, now let's take a look at the `IPackagingStrategy` interface, and the abstract `PackagingStrategy` class that implements it:

```cs
/// <summary>
/// Strategy interface used to pack all packages.
/// </summary>
internal interface IPackagingStrategy
{
    void Pack(Package package);
}

/// <summary>
/// Default packaging strategy.
/// 
/// Abstract class forces this class to be inherited.
/// </summary>
internal abstract class PackagingStrategy : IPackagingStrategy
{
    public virtual void Pack(Package package)
    {
        package.Packaging.Add(PackingMaterials.Box);
        package.Packaging.Add(PackingMaterials.BubbleWrap);
        package.Packaging.Add(PackingMaterials.Tape);
    }
}
```

These are the bread and butter components of the `strategy pattern`.  `IPackagingStrategy` is the `Strategy` and defines the core algorithm (the `Pack(Package package)` method, in this case)  that we'll be using on our `Packages`.  From that interface we create the `PackagingStrategy` class, which is effectively a `Concrete Strategy` object.  It's `abstract` (with a `virtual` `Pack(Package package)` method) in this case just to provide a default form of a concrete strategy, from which all our real strategies can inherit.  Regardless, as you can see, the `Pack(Package package)` simply adds any relevant `PackingMaterials` to the `package.Packaging` property list.

Now, let's look at all the aforementioned strategies that are inheriting `PackagingStrategy`:

```cs
// PackagingStrategies.cs
namespace Strategy
{
    /// <summary>
    /// Default packaging strategy.
    /// 
    /// Uses box, bubble wrap, and tape.
    /// </summary>
    internal class DefaultStrategy : PackagingStrategy { }

    /// <summary>
    /// Strategy for flat objects, such as letters.
    /// 
    /// Uses envelope.
    /// </summary>
    internal class FlatStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Envelope);
        }
    }

    /// <summary>
    /// Strategy for fragile objects, such as glassware.
    /// 
    /// Uses box, bubble wrap, foam, and tape.
    /// </summary>
    internal class FragileStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.BubbleWrap);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for perishables, such as food.
    /// 
    /// Uses box, dry ice, foam, and tape.
    /// </summary>
    internal class PerishableStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.DryIce);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for pliable objects, such as clothing.
    /// 
    /// Uses envelope and tape.
    /// </summary>
    internal class PliableStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Envelope);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for oversized objects, such as furniture.
    /// 
    /// Uses large box, foam, and tape.
    /// </summary>
    internal class OversizedStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.LargeBox);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }
}
```

We won't go through an explanation of each, since the comments and code are fairly explanatory, but you can see that `DefaultStrategy` doesn't need to implement or override the `Pack(Package package)` method, since we want to use the default values.  Other classes add the appropriate set of materials based on the strategy they are trying to implement.  For example, a fragile package needs a lot of protection, so the `FragileStrategy` uses a box, foam, bubble wrap, and tape to properly secure the package.

The last component is our `Client` class, which we've named `Packager` to better fit the example:

```cs
/// <summary>
/// Client that routes all packages to the passed packaging strategy.
/// </summary>
internal class Packager
{
    protected IPackagingStrategy Strategy;

    public Packager(IPackagingStrategy strategy)
    {
        Strategy = strategy;
    }

    /// <summary>
    /// Packs the passed Package, using the existing strategy.
    /// </summary>
    /// <param name="package"></param>
    public void Pack(Package package)
    {
        // Output the current strategy to the log.
        Logging.LineSeparator(Strategy.GetType().Name);

        // Pack the package using current strategy.
        Strategy.Pack(package);
    }

    /// <summary>
    /// Packs the passed Package, using the passed strategy.
    /// </summary>
    /// <param name="package">Package to pack.</param>
    /// <param name="strategy">Strategy to use.</param>
    public void Pack(Package package, IPackagingStrategy strategy)
    {
        // Assign to local strategy.
        Strategy = strategy;

        // Pass to default Pack method.
        Pack(package);
    }
}
```

Within the `strategy design pattern`, the `Client` is what connects the data to the strategy.  Typically, this is done by assigning a specific, singular `Concrete Strategy` instance to a local variable within the `Client` instance.  Thus, our code does this within the constructor itself.  As we'll see later, we can also "reuse" an existing `Packager` by passing a new `IPackagingStrategy` instance to the `Pack(Package package, IPackagingStrategy strategy)` method signature, which assigns the passed `strategy` to the local `Strategy` property, then proceeds as normal.  In effect, the `Packager` (`Client`) invokes the core algorithm (`Pack(Package package)`) of the local `Strategy`, _using_ the passed data (`Package`) the main argument.

Alright, let's put this all together and see how we might actually use the `strategy pattern` here to send some packages.  We have four different objects we'd like to send, so we'll split each into a singular example:

```cs
var bear = new Package("A teddy bear");
var defaultPackager = new Packager(new DefaultStrategy());
defaultPackager.Pack(bear);
Logging.Log(bear.ToString());
```

We start by creating a new `Package`, the contents of which will be a teddy bear.  Nothing too abnormal going on here, so we create a new `Packager` instance and pass in the `DefaultStrategy` that we want to use.  Once the `Packager` instance is created, we can call the core algorithm (`Pack(Package package)` method) and pass in the data (`Package` instance).  The result should be our teddy bear being packaged up using the default strategy, which is confirmed by the log output:

```
----------- DefaultStrategy ------------
A teddy bear was packaged using Box, BubbleWrap, Tape.
```

Now, let's try something a bit more awkward and fragile than a teddy bear -- a computer monitor:

```cs
var monitor = new Package("A computer monitor");
var packager = new Packager(new FragileStrategy());
packager.Pack(monitor);
Logging.Log(monitor.ToString());
```

Since we need to ensure the security of this package during shipment, we'll use the `FragileStrategy` for this package, but everything else is the same as before.  Our output shows that the strategy worked, making sure both bubble wrap and foam were used:

```
----------- FragileStrategy ------------
A computer monitor was packaged using Box, BubbleWrap, Foam, Tape.
```

While many implementations of the `strategy pattern` like to make new instances of `Clients` every time a new `Strategy` is implemented, our third example shows how easy it is to create a pattern that reuses existing `Clients`:

```cs
var fish = new Package("Some salmon filets");
packager.Pack(fish, new PerishableStrategy());
Logging.Log(fish.ToString());
```

To ship a few salmon filets we have _reused_ the existing `Packager` instance named `packager`, but implemented a specific strategy by passing `PerishableStrategy` as the second `Pack(...)` argument.  Since this item is perishable and must be kept cold, our packaging includes dry ice:

```
---------- PerishableStrategy ----------
Some salmon filets was packaged using Box, DryIce, Foam, Tape.
```

Lastly, a normal teddy bear is fine and dandy, but what about a **MASSIVE** teddy bear?  We'd need an `OversizedStrategy` for particularly large objects, so that's what we use:

```cs
var massiveBear = new Package("A MASSIVE teddy bear");
packager.Pack(massiveBear, new OversizedStrategy());
Logging.Log(massiveBear.ToString());
```

```
---------- OversizedStrategy -----------
A MASSIVE teddy bear was packaged using LargeBox, Foam, Tape.
```

---

There we have it!  I hope this article gave you a bit more information on what the `strategy design pattern` is, and how it can be easily implemented in your own code.  For more information on all the other popular design patterns, head on over to our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 21 of our Software Design Pattern series in which examine the strategy design pattern using fully-functional C# example code.
