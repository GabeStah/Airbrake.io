# Creational Design Patterns: Singleton

Today we're continuing our journey through our [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series with our final  `Creational` pattern: `singleton`.  The purpose of the `singleton` pattern is to ensure that only a _single_ instance of the object or class in question can exist at one time.

We'll spend some time in this article examining the `singleton` pattern in more detail, using both real world examples and some functional `C#` code.  We'll also briefly touch on the differences between thread safe vs non-thread safe methods of implementation, and why consideration of those two dynamics may be important.  Let's get going!

## Singleton In the Real World

There are certainly many instances in the real world where a particular object could be considered a `singleton`.  Individual human beings are the most relatable, since each of us is extraordinarily unique through the structure of our cells, our minds, our experiences, our behaviors, and so forth (#UniqueSnowflakes).  However, it's important to differentiate between `singularity` and `uniqueness`.  While it would be correct for us to state that there is no other individual person that is exactly like _you_, and therefore you are unique in that regard, of course there are billions of other human beings.  In a programming sense, if you were creating a `class` to represent a `singleton` instance of yourself, you might call the class `Me`:

```cs
public class Me
{
    public Me
    {
        console.log("I'm awesome!");
    }
}
```

However, we couldn't simply create a `Person` class, since we would likely have to create billions of instances of the `Person` class, in order to represent everyone on the planet.  Thus, when considering the `singleton` class, it's critical to think in terms of `uniqueness`.

To that end, another real world example of a `singleton` object is a simple **deck of cards**.  When most of us sit down to play a few games of poker with our buddies, we often play using a single entity known as the `deck`.  This typically consists of 52 cards in total, made up of thirteen ranks of four suits each.  When a hand is dealt around the table, all cards are obtained from that single shuffled instance of the `deck`.  Once the hand is over and you've (hopefully) taken your friends for all they're worth, all 52 cards are gathered up and shuffled back into that same single instance of the `deck`, then a new hand is dealt from it.  Repeat ad nauseam until all money is lost and/or drunkenness overtakes you.

## How Singleton Works In Code

Now, as mentioned in the introduction, the basic purpose of the `singleton` pattern is when there is a class which you want to ensure can only ever have a maximum of one `instance` in existence at any given moment.  While not an very common pattern, there are some cases where it's helpful to have a `singleton` class, such as when performing heavy I/O functions on a data set where you must be assured that multiple duplicate requests don't occur simultaneously.  In that case, a `singleton` pattern could be used as part of a queue or job system.

To illustrate how a `singleton` pattern is implemented we'll continue with a deck of cards example.  As with our previous design pattern articles, it's probably easiest to give the full code at the start, then we'll explore it in more detail to follow:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadSafeExample();
            NonThreadSafeExample();
        }

        static void NonThreadSafeExample()
        {
            // Create two new threads.
            var threadA = new System.Threading.Thread(new System.Threading.ThreadStart(NonSafeThreadA));
            var threadB = new System.Threading.Thread(new System.Threading.ThreadStart(NonSafeThreadB));
            // Execute both new threads.
            threadA.Start();
            threadB.Start();

            // Block this thread for three seconds, so other threads can finish execution.
            threadA.Join(3000);
            threadB.Join(3000);
            // Get deck instance.
            var deck = NonThreadSafeDeck.Instance;
            // Check current card count.
            Logging.Log($"Main Thread card count: {deck.Cards.Count()}");
        }

        static void NonSafeThreadA()
        {
            var deck = NonThreadSafeDeck.Instance;
            Logging.Log($"Thread A card count: {deck.Cards.Count()}");
            Logging.Log("Removing a card from Thread A deck.");
            deck.Cards.RemoveAt(1);
            Logging.Log($"Thread A card count: {deck.Cards.Count()}");
        }

        static void NonSafeThreadB()
        {
            var deck = NonThreadSafeDeck.Instance;
            Logging.Log($"Thread B card count: {deck.Cards.Count()}");
            Logging.Log("Removing a card from Thread B deck.");
            deck.Cards.RemoveAt(1);
            Logging.Log($"Thread B card count: {deck.Cards.Count()}");
        }

        static void ThreadSafeExample()
        {

            // Create two new threads.
            var threadA = new System.Threading.Thread(new System.Threading.ThreadStart(SafeThreadA));
            var threadB = new System.Threading.Thread(new System.Threading.ThreadStart(SafeThreadB));
            // Execute both new threads.
            threadA.Start();
            threadB.Start();

            // Block this thread for three seconds, so other threads can finish execution.
            threadA.Join(3000);
            threadB.Join(3000);
            // Get deck instance.
            var deck = Deck.Instance;
            // Check current card count.
            Logging.Log($"Main thread card count: {deck.Cards.Count()}");
        }

        static void SafeThreadA()
        {
            var deck = Deck.Instance;
            Logging.Log($"Thread A card count: {deck.Cards.Count()}");
            Logging.Log("Removing a card from Thread A deck.");
            deck.Cards.RemoveAt(1);
            Logging.Log($"Thread A card count: {deck.Cards.Count()}");
        }

        static void SafeThreadB()
        {
            var deck = Deck.Instance;
            Logging.Log($"Thread B card count: {deck.Cards.Count()}");
            Logging.Log("Removing a card from Thread B deck.");
            deck.Cards.RemoveAt(1);
            Logging.Log($"Thread B card count: {deck.Cards.Count()}");
        }

    }


    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public enum Rank
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public interface ICard
    {
        Suit Suit { get; set; }
        Rank Rank { get; set; }
    }

    public class Card : ICard
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }

    public sealed class Deck
    {
        private static readonly Lazy<Deck> LazyDeck = new Lazy<Deck>(() => new Deck());

        public static Deck Instance { get { return LazyDeck.Value; } }
        public List<Card> Cards { get; set; }

        private Deck()
        {
            // Query all possible card combinations and create list of Card class instances.
            var cardList = from suit in Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList()
                           from rank in Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList()
                           select new Card(suit, rank);
            // Assign list to Cards property.
            Cards = cardList.ToList<Card>();
        }
    }

    public sealed class NonThreadSafeDeck
    {
        private static NonThreadSafeDeck instance = null;
        public List<Card> Cards { get; set; }

        private NonThreadSafeDeck()
        {
            // Query all possible card combinations and create list of Card class instances.
            var cardList = from suit in Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList()
                           from rank in Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList()
                           select new Card(suit, rank);
            // Assign list to Cards property.
            Cards = cardList.ToList<Card>();
        }

        public static NonThreadSafeDeck Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NonThreadSafeDeck();
                }
                return instance;
            }
        }
    }
}
```

_Note: To keep this a bit shorter we've left out the `Utility` namespace and `Logging` class code.  Those snippets have been included in other [Design Pattern](https://airbrake.io/blog/category/design-patterns) articles, so feel free to check those out if you're curious._

For this example we have a basic `Deck` class and we want to ensure that it is a `singleton`, such that only one instance of the class can ever be created or used in code.  However, when dealing with `singleton` patterns, another important factor must be taken into consideration: `thread safety`.  We won't get into too many of the details but, like many programming languages/frameworks, .NET allows for the creation and execution of multiple threads.  In practice, this usually means that one thread might be handling UI and visual functionality, while another thread could be handling background logic and processing.

The challenge with developing multi-thread applications is ensuring that the entire codebase is [thread safe](https://en.wikipedia.org/wiki/Thread_safety).  An application is `thread safe` when multiple threads can execute in tandem, but never present what are known as [`race conditions`](https://en.wikipedia.org/wiki/Race_condition#Software), which occur when the application behavior or state changes based on sequential or timed executions.  As we'll see in our example code shortly, a non-thread safe application can allow for multiple threads to interact with (and thus alter) the state of an object without the other thread knowing about these interactions.

Thus, our example code begins with some basic `enums` to represent the possible suits and ranks of our cards.  We also have a simple `ICard` interface which is used by our `Card` class to indicate that a single instance of `Card` must have a valid `Suit` and `Rank` enumeration value:

```cs
public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

public enum Rank
{
    Ace,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

public interface ICard
{
    Suit Suit { get; set; }
    Rank Rank { get; set; }
}

public class Card : ICard
{
    public Suit Suit { get; set; }
    public Rank Rank { get; set; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }
}
```

With those basics setup, we can start by defining our `Deck` classes.  We begin with the `NonThreadSafeDeck` class:

```cs
public sealed class NonThreadSafeDeck
{
    private static NonThreadSafeDeck instance = null;
    public List<Card> Cards { get; set; }

    private NonThreadSafeDeck()
    {
        // Query all possible card combinations and create list of Card class instances.
        var cardList = from suit in Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList()
                       from rank in Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList()
                       select new Card(suit, rank);
        // Assign list to Cards property.
        Cards = cardList.ToList<Card>();
    }

    public static NonThreadSafeDeck Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NonThreadSafeDeck();
            }
            return instance;
        }
    }
}
```

This is our first crack at a `singleton` pattern to represent our deck of cards.  The `sealed` keyword ensure that no other class can inherit from `NonThreadSafeDeck`.  This is a critical part of the `singleton` pattern, since allowing inheritance would mean that two (or more) subclasses could be defined and then initialized, which would lead to two (or more) unique instances of our class (which violates the `singleton` pattern).

We then define a `static` property called `instance`, which expects to be set to a `NonThreadSafeDeck` type.  This is used to store our single instance of our class.  We also have the `Cards` property, which is just a list of `Card` class instances that we'll use for the basic logic of populating our deck.

The `private` `NonThreadSafeDeck` constructor could be blank, though here we have our basic logic to instantiate our list of `Cards` by using a `LINQ` query to create a unique `Card` instance for each combination of all `Ranks` and `Suits` (giving us the standard 52 card deck).

Finally, we have a `static` `Instance` method that is our only `public` member of the class.  This method checks if our `instance` property exists yet, and if not, creates a new instance of our class then returns that value.

The result is that we can instantiate our `singleton` instance of `NonThreadSafeDeck` via the `NonThreadSafeDeck.Instance` property:

```cs
var deck = NonThreadSafeDeck.Instance;
```

However, as the name of our class implies, this potentially presents a pretty big problem: It's not `thread safe`.  To see why we have three simple methods that use `NonThreadSafeDeck` across a total of three execution threads:

```cs
static void NonThreadSafeExample()
{
    // Create two new threads.
    var threadA = new System.Threading.Thread(new System.Threading.ThreadStart(NonSafeThreadA));
    var threadB = new System.Threading.Thread(new System.Threading.ThreadStart(NonSafeThreadB));
    // Execute both new threads.
    threadA.Start();
    threadB.Start();

    // Block this thread for three seconds, so other threads can finish execution.
    threadA.Join(3000);
    threadB.Join(3000);
    // Get deck instance.
    var deck = NonThreadSafeDeck.Instance;
    // Check current card count.
    Logging.Log($"Main Thread card count: {deck.Cards.Count()}");
}

static void NonSafeThreadA()
{
    var deck = NonThreadSafeDeck.Instance;
    Logging.Log($"Thread A card count: {deck.Cards.Count()}");
    Logging.Log("Removing a card from Thread A deck.");
    deck.Cards.RemoveAt(1);
    Logging.Log($"Thread A card count: {deck.Cards.Count()}");
}

static void NonSafeThreadB()
{
    var deck = NonThreadSafeDeck.Instance;
    Logging.Log($"Thread B card count: {deck.Cards.Count()}");
    Logging.Log("Removing a card from Thread B deck.");
    deck.Cards.RemoveAt(1);
    Logging.Log($"Thread B card count: {deck.Cards.Count()}");
}
```

The code is reasonably commented, but the basic idea here is that we create two new threads (`A` and `B`) and define that the execution of each thread should begin with the `NonSafeThreadA` and `NonSafeThreadB` methods, respectively.  Within each method we get the `Instance` of our `NonThreadSafeDeck` class, assign it to a local `deck` variable, then perform some simple steps: Output the current card count, remove a single card, then output the updated card count.

Finally, we wait for three seconds to ensure our `A` and `B` threads have completed before we continue in the main thread, grabbing the `Instance` one last time and outputting that current card count a final time.

While you can probably guess what will happen, here's the resulting output of this test:

```
Thread B card count: 52
Removing a card from Thread B deck.
Thread B card count: 51
Thread A card count: 52
Removing a card from Thread A deck.
Thread A card count: 51
Main Thread card count: 51
The thread 0x38c0 has exited with code 0 (0x0).
The thread 0x4634 has exited with code 0 (0x0).
```

As we can see from the output our code is not thread safe.  We first get our `Instance` in thread `A` and decrement the card count down to `51`.  Then when we get our `Instance` in thread `B`, the card count is back up to `52`, so we again decrement it down to `51`.  After both `A` and `B` threads are complete, we check the card count of `Instance` in our main thread, and we find that it is at `51`.  This is clearly not what was intended, since we decremented the count two times, once in each sub-thread, so we should expect the count in the main thread to be `50`.

The reason for our trouble -- and thus why `NonThreadSafeDeck` is not a true `singleton` pattern class -- is because we had no checks in place within our class (and the `Instance` method specifically) to determine if our `instance` property was already defined in another thread.  Since our `A` and `B` threads effectively executed simultaneously, they both checked (and confirmed) that `instance` was still `null`, and therefore they both create a `new NonThreadSafeDeck` instance and returned it for us at the same time.

To resolve this issue we can make a few simple changes in our design within the `Deck` class:

```cs
public sealed class Deck
{
    private static readonly Lazy<Deck> LazyDeck = new Lazy<Deck>(() => new Deck());

    public static Deck Instance { get { return LazyDeck.Value; } }
    public List<Card> Cards { get; set; }

    private Deck()
    {
        // Query all possible card combinations and create list of Card class instances.
        var cardList = from suit in Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList()
                        from rank in Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList()
                        select new Card(suit, rank);
        // Assign list to Cards property.
        Cards = cardList.ToList<Card>();
    }
}
```

Here we're defining the `LazyDeck` property of our `Deck` class, which is a `static` `readonly` of the [`Lazy<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.lazy-1?view=netframework-4.7) class type.  `Lazy<T>` allows us to create an object that uses `lazy initialization`, which means that the actual creation of the `LazyDeck` property is deferred until it is first used.  `Lazy initialization` is commonly used for expensive operations within an object creation, so it can be delayed until a later time.  However, in our case, we're using it as a simpler (and thread safe) means of creating a unique, single instance of our `Deck` class.  `Lazy<T>` is inherently thread safe, which we'll see in a moment.

We set the `LazyDeck` property's initial value to `new Lazy<Deck>`, then pass in a lambda to specify the function that will be called when `LazyDeck.Value` is first accessed.  In this case, it is an anonymous function that simply creates a `new Deck()` instance and returns that to be assigned to the `LazyDeck.Value` property.

Our `Deck.Instance` property is much the same as before, but we're using that to return `LazyDeck.Value`, which behaves the same as our previous example, giving us the `singleton` instance of our class.  From there, the constructor and `Cards` property are both the same as before, so we won't go over those.

The result is that we now have a thread safe version of our deck via the `Deck` class.  To illustrate this we have a similar setup that we used before, creating two threads (`A` and `B`), retrieving the `Deck.Instance` value within each, decrementing the card count, then checking the final counts afterward and in the main thread at the end:

```cs
static void ThreadSafeExample()
{

    // Create two new threads.
    var threadA = new System.Threading.Thread(new System.Threading.ThreadStart(SafeThreadA));
    var threadB = new System.Threading.Thread(new System.Threading.ThreadStart(SafeThreadB));
    // Execute both new threads.
    threadA.Start();
    threadB.Start();

    // Block this thread for three seconds, so other threads can finish execution.
    threadA.Join(3000);
    threadB.Join(3000);
    // Get deck instance.
    var deck = Deck.Instance;
    // Check current card count.
    Logging.Log($"Main thread card count: {deck.Cards.Count()}");
}

static void SafeThreadA()
{
    var deck = Deck.Instance;
    Logging.Log($"Thread A card count: {deck.Cards.Count()}");
    Logging.Log("Removing a card from Thread A deck.");
    deck.Cards.RemoveAt(1);
    Logging.Log($"Thread A card count: {deck.Cards.Count()}");
}

static void SafeThreadB()
{
    var deck = Deck.Instance;
    Logging.Log($"Thread B card count: {deck.Cards.Count()}");
    Logging.Log("Removing a card from Thread B deck.");
    deck.Cards.RemoveAt(1);
    Logging.Log($"Thread B card count: {deck.Cards.Count()}");
}
```

The output shows us that our underlying `Deck` object instance is a true `singleton`:

```
Thread B card count: 52
Removing a card from Thread B deck.
Thread B card count: 51
Thread A card count: 52
Removing a card from Thread A deck.
Thread A card count: 50
Main thread card count: 50
The thread 0x2bbc has exited with code 0 (0x0).
The thread 0x3fe8 has exited with code 0 (0x0).
```

Even though we had two threads running in parallel, we are able to decrement the card count within each thread and have the `singleton` instance of `Deck` retain it's state and values.  This results in the card count now being dropped down to the expected value of `50` in the main thread.

---

__META DESCRIPTION__

Part 6 of our Software Design Pattern series in which examine the Singleton design pattern, including a bit of C# example code.