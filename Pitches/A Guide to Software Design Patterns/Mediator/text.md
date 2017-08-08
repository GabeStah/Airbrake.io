# Behavioral Design Patterns: Mediator

Making our way through the detailed [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series we've been publishing, today we'll be exploring the handy mediator design pattern.  In the `mediator design pattern`, a `mediator` object encapsulates how other types of objects interact, _without_ the need for those objects to be aware the implementation of other objects.

Throughout this article we'll investigate the `mediator design pattern` using a real world example.  We'll follow that up with a fully-functional C# code sample that will illustrate how the `mediator pattern` can be quickly and easily implemented, so let's get to it!

## In the Real World

The `mediator design pattern` obviously originates from the concept of `mediation`, or `to mediate`.  Its purpose is to serve as a go-between, so other components can properly interact with one another, even in situations where doing so may be difficult.  In fact, the obvious real world example of a `mediator pattern` is in a scenario like group counseling.  A therapist may act as a mediator between spouses who are going through couples therapy.  Or, a mediator may drive discussion and encourage speakers at a local AA meeting.  Lawyer jokes aside, attorneys often act as mediators during difficult proceedings between family members or friends who find themselves in a difficult legal battle.

Outside the realm of the humanities, most of us experience some form of technological mediation multiple times a day.  Virtually every modern communication app on our phones uses some form of the `mediator pattern` to connect multiple phones (and, thereby, people) together.  When you send a text to your BFF, you aren't making a direct connection from your phone to your friend's phone.  Instead, your carrier receives your request and establishes a connection on your behalf, acting as the mediator.  The bits that makeup your message are sent out to the nearby cell tower to a switching service handled by your carrier.  The carrier then passes the message along via the fastest fiber optic route to the cell tower nearest to your friend.  From there, the message finally arrives at your friend's phone.  In some cases, satellites might even be involved, which also behave as a mediator in many ways.

## How It Works In Code

The `mediator pattern` has three major components:

- `Mediator` - Coordinates communication between all `colleague` objects.
- `Colleague` - Sends message to and receives messages from other `colleagues` through an associated `mediator` object.
- `Client` - Creates `colleagues` and associates an appropriate `mediator` with each.

Having just watched the latest episode of _Game of Thrones_, it seems appropriate to tie the show into our code sample in some way.  To do so, we'll loosely base our code on the real world telephonic communication example cited above -- a carrier service behaves as a `mediator` so two callers (`colleagues`) can communicate with one another.  However, cell reception is spotty, at best, way up north on _The Wall_, so our sample code uses a "town crier" (`Crier` class) to act as the go-between or `mediator` for our list of people who want to communicate with each other.  When a character says something, the crier hears the message and _shouts_ it out, ensuring that all the other characters hear the message themselves and know who said it.

The full code sample can be found below, after which we'll break it down into smaller chunks to see how the `mediator pattern` is actually working:

```cs
// Program.cs
namespace Mediator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var crier = new Crier();

            // Create some people and assign the crier.
            var arya = new Person("Arya Stark", crier);
            var cersei = new Person("Cersei Lannister", crier);
            var daenerys = new Person("Daenerys Targaryen", crier);
            // Without a tongue it's difficult to speak, so Ilyn remains silent and listens.
            var ilyn = new Person("Ilyn Payne", crier);
            var tyrion = new Person("Tyrion Lannister", crier);

            // Send messages from respective characters.
            arya.Say("Valar morghulis.");
            tyrion.Say("Never forget what you are, for surely the world will not.");
            daenerys.Say("Men are mad and gods are madder.");
            cersei.Say("When you play the game of thrones, you win or you die. There is no middle ground.");
        }
    }
}

// Crier.cs
using Utility;

namespace Mediator
{
    /// <summary>
    /// Declares members for Crier.
    /// </summary>
    internal interface ICrier
    {
        void Shout(string message, Person source);
    }

    /// <summary>
    /// Event handler for MessageReceived event.
    /// </summary>
    /// <param name="message">Received message.</param>
    /// <param name="source">Person who sent the message.</param>
    internal delegate void MessageReceivedEventHandler(string message, Person source);
    
    /// <summary>
    /// Communicates messages between all Persons.
    /// 
    /// Acts as 'Mediator' within mediator pattern.
    /// </summary>
    internal class Crier : ICrier
    {
        internal event MessageReceivedEventHandler MessageReceived;

        /// <summary>
        /// Shout a message from passed Person source.
        /// </summary>
        /// <param name="message">Message to be shouted.</param>
        /// <param name="source">Person message originated from.</param>
        public void Shout(string message, Person source)
        {
            // Extend separator beyond name and message length.
            var separatorLength = source.ToString().Length + message.Length + 30;
            // Create separator with message and source
            Logging.LineSeparator($"'{message}' from {source}", separatorLength, '=');
            // If handler isn't null, invoke MessageReceived event with message and source.
            MessageReceived?.Invoke(message, source);
        }
    }
}

// Person.cs
using Utility;

namespace Mediator
{
    /// <summary>
    /// Declares members for Crier.
    /// </summary>
    internal interface IPerson
    {
        void Listen(string message, Person source);
        void Say(string message);
    }

    /// <summary>
    /// Sends and receives messages via the passed Crier.
    /// 
    /// Acts as 'Colleague' within mediator pattern.
    /// </summary>
    internal class Person : IPerson
    {
        public string Name { get; }
        public Crier Crier { get; }

        public Person(string name, Crier crier)
        {
            Crier = crier;
            // Listen method subscribes to MessageReceived event handler.
            Crier.MessageReceived += Listen;

            Name = name;
        }

        /// <summary>
        /// Receives a message from source Person.
        /// </summary>
        /// <param name="message">Message received.</param>
        /// <param name="source">Person who sent the message.</param>
        public void Listen(string message, Person source)
        {
            // If source is self, ignore.
            if (source == this) return;

            // Output received message.
            Logging.Log($"{source} to {this}: '{message}'");
        }

        /// <summary>
        /// Sends passed message to all subscribed Persons via Crier.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        public void Say(string message)
        {
            Crier.Shout(message, this);
        }

        /// <summary>
        /// Overrides ToString() method.
        /// </summary>
        /// <returns>Name property value.</returns>
        public override string ToString() => Name;
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
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                : ObjectDumper.Dump(value));
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
                length -= (insert.Length + 2);
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

We start by defining the `ICrier` interface and the `Crier` class that implements it:

```cs
// Crier.cs
using Utility;

namespace Mediator
{
    /// <summary>
    /// Declares members for Crier.
    /// </summary>
    internal interface ICrier
    {
        void Shout(string message, Person source);
    }

    /// <summary>
    /// Event handler for MessageReceived event.
    /// </summary>
    /// <param name="message">Received message.</param>
    /// <param name="source">Person who sent the message.</param>
    internal delegate void MessageReceivedEventHandler(string message, Person source);
    
    /// <summary>
    /// Communicates messages between all Persons.
    /// 
    /// Acts as 'Mediator' within mediator pattern.
    /// </summary>
    internal class Crier : ICrier
    {
        internal event MessageReceivedEventHandler MessageReceived;

        /// <summary>
        /// Shout a message from passed Person source.
        /// </summary>
        /// <param name="message">Message to be shouted.</param>
        /// <param name="source">Person message originated from.</param>
        public void Shout(string message, Person source)
        {
            // Extend separator beyond name and message length.
            var separatorLength = source.ToString().Length + message.Length + 30;
            // Create separator with message and source
            Logging.LineSeparator($"'{message}' from {source}", separatorLength, '=');
            // If handler isn't null, invoke MessageReceived event with message and source.
            MessageReceived?.Invoke(message, source);
        }
    }
}
```

The `Crier` class acts as our `mediator` object and must receive the `colleague` object that is trying to send a message (`Person`, in this case).  The `Shout(string message, Person source)` method handles the only logic we need for mediation.  Specifically, we're using the `MessageReceived` event handler -- a delegate with the same signature as the calling method -- to fire when the crier shouts out a message.

Now we need to define at least one `colleague` object.  This will be the `IPerson` interface and the `Person` class that implements it:

```cs
// Person.cs
using Utility;

namespace Mediator
{
    /// <summary>
    /// Declares members for Crier.
    /// </summary>
    internal interface IPerson
    {
        void Listen(string message, Person source);
        void Say(string message);
    }

    /// <summary>
    /// Sends and receives messages via the passed Crier.
    /// 
    /// Acts as 'Colleague' within mediator pattern.
    /// </summary>
    internal class Person : IPerson
    {
        public string Name { get; }
        public Crier Crier { get; }

        public Person(string name, Crier crier)
        {
            Crier = crier;
            // Listen method subscribes to MessageReceived event handler.
            Crier.MessageReceived += Listen;

            Name = name;
        }

        /// <summary>
        /// Receives a message from source Person.
        /// </summary>
        /// <param name="message">Message received.</param>
        /// <param name="source">Person who sent the message.</param>
        public void Listen(string message, Person source)
        {
            // If source is self, ignore.
            if (source == this) return;

            // Output received message.
            Logging.Log($"{source} to {this}: '{message}'");
        }

        /// <summary>
        /// Sends passed message to all subscribed Persons via Crier.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        public void Say(string message)
        {
            Crier.Shout(message, this);
        }

        /// <summary>
        /// Overrides ToString() method.
        /// </summary>
        /// <returns>Name property value.</returns>
        public override string ToString() => Name;
    }
}
```

The first requirement of the `Person` class is that it has an association with a `Crier` (`mediator`).  We accomplish this through the constructor parameter and the `Crier` property assignment.  The other requirement is the ability for `Person` to send a message (or some other behavior) that can be handled by the associated `mediator`.  In this case, the `Say(string message)` method handles that by calling `Crier.Shout(message, this)`.  In the fantasy scenario we've built for ourselves, this means that when a person says something, the crier hears it and shouts it loudly, relaying not only the message, but also the person it originated from.

Since we want to avoid having to directly associate _every single_ unique instance of our `colleague` (`Person`) object with every other, we're using an event/subscriber pattern so all persons can listen for messages shouted by the crier.  In the constructor we subscribe the `Listen(string message, Person source)` method to the `Crier.MessageReceived` handler.  Thus, every time the `Crier` shouts out a message, each `Person` will receive that subscribed event, effectively allowing everyone to listen to what every else says.

Alright, enough messing about with setup, let's see this in action.  Our `client` code uses the `Crier` and `Person` classes within the `Program.Main()` method:

```cs
// Program.cs
namespace Mediator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var crier = new Crier();

            // Create some people and assign the crier.
            var arya = new Person("Arya Stark", crier);
            var cersei = new Person("Cersei Lannister", crier);
            var daenerys = new Person("Daenerys Targaryen", crier);
            // Without a tongue it's difficult to speak, so Ilyn remains silent and listens.
            var ilyn = new Person("Ilyn Payne", crier);
            var tyrion = new Person("Tyrion Lannister", crier);

            // Send messages from respective characters.
            arya.Say("Valar morghulis.");
            tyrion.Say("Never forget what you are, for surely the world will not.");
            daenerys.Say("Men are mad and gods are madder.");
            cersei.Say("When you play the game of thrones, you win or you die. There is no middle ground.");
        }
    }
}
```

Connoisseurs of the books (or even the show): please bear with me.  I wanted to choose characters that had some interesting quotes, so we'll need to pretend that these characters were all hanging out together, perhaps at a lovely gala, when these particular utterances were made.

As you can see, we start by instantiating a `Crier` object.  Then, we create a series of characters and be sure to pass the `crier` instance to each constructor, which forms the fundamental connection between `Colleague` and `Mediator` that the entire `mediator pattern` relies on.

Now our characters can `Say()` whatever they'd like.  By using a `mediator design pattern` here, the goal is to allow anything said by one character to be heard by all other characters, as relayed by the associated `crier`.  To see if everything works as expected, let's take a look at the output produced by the code above:

```
========== 'Valar morghulis.' from Arya Stark ==========
Arya Stark to Cersei Lannister: 'Valar morghulis.'
Arya Stark to Daenerys Targaryen: 'Valar morghulis.'
Arya Stark to Ilyn Payne: 'Valar morghulis.'
Arya Stark to Tyrion Lannister: 'Valar morghulis.'

========== 'Never forget what you are, for surely the world will not.' from Tyrion Lannister ==========
Tyrion Lannister to Arya Stark: 'Never forget what you are, for surely the world will not.'
Tyrion Lannister to Cersei Lannister: 'Never forget what you are, for surely the world will not.'
Tyrion Lannister to Daenerys Targaryen: 'Never forget what you are, for surely the world will not.'
Tyrion Lannister to Ilyn Payne: 'Never forget what you are, for surely the world will not.'

========== 'Men are mad and gods are madder.' from Daenerys Targaryen ==========
Daenerys Targaryen to Arya Stark: 'Men are mad and gods are madder.'
Daenerys Targaryen to Cersei Lannister: 'Men are mad and gods are madder.'
Daenerys Targaryen to Ilyn Payne: 'Men are mad and gods are madder.'
Daenerys Targaryen to Tyrion Lannister: 'Men are mad and gods are madder.'

========== 'When you play the game of thrones, you win or you die. There is no middle ground.' from Cersei Lannister ==========
Cersei Lannister to Arya Stark: 'When you play the game of thrones, you win or you die. There is no middle ground.'
Cersei Lannister to Daenerys Targaryen: 'When you play the game of thrones, you win or you die. There is no middle ground.'
Cersei Lannister to Ilyn Payne: 'When you play the game of thrones, you win or you die. There is no middle ground.'
Cersei Lannister to Tyrion Lannister: 'When you play the game of thrones, you win or you die. There is no middle ground.'
```

As expected, each message sent by an individual is transferred to everyone else via the crier.  For example, we see that when Arya says "Valar morghulis", all the other characters receive that message and know it is from her, even though no two `Person` instances have a connection or awareness of one another.  It's also worth noting that, because of the use of an event handler, even though Ilyn Payne cannot speak, he can still listen and is able to receive all the messages sent by the others.

---

That should do it for now!  Hopefully this gave you a better sense of what the mediator design pattern is for and how it can be relatively easily implemented into almost any scenario.  For more information on all the other popular design patterns, head on over to our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 17 of our Software Design Pattern series in which examine the mediator design pattern using fully-functional C# example code.

---

__SOURCES__

- https://en.wikiquote.org/wiki/A_Song_of_Ice_and_Fire
- https://en.wikipedia.org/wiki/Mediator_pattern