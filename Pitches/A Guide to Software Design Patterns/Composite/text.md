# Structural Design Patterns: Composite

Today we'll be continuing our exploration of the common `Structural` design patterns as part of our much bigger [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series.  Next up is the `composite` design pattern, which is used to separate an abstraction from its implementation so both can be modified on their own.  Specifically, the `composite` pattern aims to treat singular object instances and _collections_ of the same object types uniformly.  This allows the client code to interact with this type of composited object the same throughout the code, regardless of whether it's a singular instance or a collection of instances.

Throughout this article we'll look more closely at the `composite` pattern by examining both a real world example of its use and a fully-functional `C#` code example to show how you might put the `composite` pattern into practice.  Let's get to it!

## In the Real World

A real world concept we can all relate to is that of family.  Love 'em or leave 'em, we all have family members to some degree and we can examine the rough hierarchy of familiar ties as a way to explore how the `composite` design pattern can be seen in the real world.

You, just like your parents or your siblings or cousins, are an individual person.  You have your own unique qualities and attributes that make you you, just as the other individual members of your family have their own quirks and idiosyncrasies.  And yet, even though all our family members are unique individuals, we all make up the larger group of a family.  Put another way, we might say that your family is **composed** of individual family members like yourself.

Expanding this idea further, family members are related to one another through their parentage.  You and your siblings might share the same parent, your children will have you as a parent, while your mother had someone else as a parent, and so forth.  Thus, all these individuals that were born into the same family tree are still a part of that overall composition of that family group.

## How It Works In Code

As discussed in the introduction, the basic purpose of the `composite` pattern is to allow an object to represent both a singular and a collective group of said object type, without any need to differentiate between the two groupings when using them.  To better understand this we'll start with the full code example and then we'll break it down a bit and see what's actually happening and how the `composite` design pattern is being used:

```cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parent 1
            Person john = new Person("John");
            // Parent 2
            Person jane = new Person("Jane");

            // Child 1
            Person alice = new Person("Alice");
            // Child 2
            Person billy = new Person("Billy");
            // Child 3
            Person christine = new Person("Christine");

            // Output children
            john.LogChildren();
            jane.LogChildren();

            // John and Jane are both parents of Alice
            john.AddChild(alice);
            jane.AddChild(alice);

            // John is also Billy's parent
            john.AddChild(billy);

            // Jane is also Christine's parent
            jane.AddChild(christine);

            // Output children
            john.LogChildren();
            jane.LogChildren();

            // Since David is John's brother he is also an uncle.
            Uncle david = new Uncle("David");

            // David and John are both the children of their father Edward.
            Person edward = new Person("Edward");
            edward.AddChild(john);
            // Even though 'david' is class of Uncle it can still be added
            // as a child.
            edward.AddChild(david);

            // Output edward's children.
            edward.LogChildren();
        }
    }

    public interface IFamilyMember
    {
        string Name { get; set; }
    }

    /// <summary>
    /// Enumerable list of Persons that belong to a family.
    /// </summary>
    public class Person : IFamilyMember, IEnumerable<IFamilyMember>
    {
        /// <summary>
        /// Private list of children belonging to this person.
        /// </summary>
        private List<IFamilyMember> _children = new List<IFamilyMember>();

        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Add a child to the list of children.
        /// </summary>
        /// <param name="child">Child to add.</param>
        public void AddChild(IFamilyMember child)
        {
            _children.Add(child);
        }

        /// <summary>
        /// Remove a children from the list of children.
        /// </summary>
        /// <param name="child">Child to remove.</param>
        public void RemoveChild(IFamilyMember child)
        {
            _children.Remove(child);
        }

        /// <summary>
        /// Get a child instance by index.
        /// </summary>
        /// <param name="index">Index of child to retrieve.</param>
        /// <returns>Child record.</returns>
        public IFamilyMember GetChild(int index)
        {
            return _children[index];
        }

        /// <summary>
        /// Get a child instance by name.
        /// </summary>
        /// <param name="name">Name of child to retrieve.</param>
        /// <returns>Child record.</returns>
        public IFamilyMember GetChild(string name)
        {
            return _children.Where(c => c.Name == name).First();
        }

        /// <summary>
        /// Get collection of children.
        /// </summary>
        /// <returns>Collection of children.</returns>
        public IEnumerable<IFamilyMember> GetChildren()
        {
            return _children;
        }

        public IEnumerator<IFamilyMember> GetEnumerator()
        {
            return ((IEnumerable<IFamilyMember>)_children).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IFamilyMember>)_children).GetEnumerator();
        }

        /// <summary>
        /// Output current children list in human-readable form.
        /// </summary>
        public void LogChildren()
        {
            Logging.LineSeparator();
            // Check if person has any children.
            if (GetChildren().Any())
            {
                // Output person's name, number of children, and list of child names.
                Logging.Log($"{Name} has ({GetChildren().Count()}) children:\n{String.Join("\n", GetChildren().Select(c => c.Name))}");
            }
            else
            {
                // No children to output.
                Logging.Log($"{Name} has no children.");
            }
        }
    }

    /// <summary>
    /// Leaf class to act as Aunt.
    /// </summary>
    public class Aunt : IFamilyMember
    {
        public string Name { get; set; }

        public Aunt(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Leaf class to act as Uncle.
    /// </summary>
    public class Uncle : IFamilyMember
    {
        public string Name { get; set; }

        public Uncle(string name)
        {
            Name = name;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(string value)
        {
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }

        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(object value)
        {
#if DEBUG
            Debug.WriteLine(ObjectDumper.Dump(value));
#else
            Console.WriteLine(ObjectDumper.Dump(value));
#endif
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="System.Diagnostics.Debug.WriteLine"/> 
        /// if DEBUG mode is enabled, otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        public static void LineSeparator()
        {
#if DEBUG
            Debug.WriteLine(new string('-', 20));
#else
            Console.WriteLine(new string('-', 20));
#endif
        }
    }
}
```

In this example we're continuing with the family concept.  We want to be able to add parent/child relationships to our individuals while also maintaining a collection of children under the hood.  To begin we start with a simple `IFamilyMember` interface that only provides a single `Name` property:

```cs
public interface IFamilyMember
{
    string Name { get; set; }
}
```

From there we add our `Person` class, which is a normal class but is also `enumerable` through the `IEnumerable` interface:

```cs
/// <summary>
/// Enumerable list of Persons that belong to a family.
/// </summary>
public class Person : IFamilyMember, IEnumerable<IFamilyMember>
{
    /// <summary>
    /// Private list of children belonging to this person.
    /// </summary>
    private List<IFamilyMember> _children = new List<IFamilyMember>();

    public string Name { get; set; }

    public Person(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Add a child to the list of children.
    /// </summary>
    /// <param name="child">Child to add.</param>
    public void AddChild(IFamilyMember child)
    {
        _children.Add(child);
    }

    /// <summary>
    /// Remove a children from the list of children.
    /// </summary>
    /// <param name="child">Child to remove.</param>
    public void RemoveChild(IFamilyMember child)
    {
        _children.Remove(child);
    }

    /// <summary>
    /// Get a child instance by index.
    /// </summary>
    /// <param name="index">Index of child to retrieve.</param>
    /// <returns>Child record.</returns>
    public IFamilyMember GetChild(int index)
    {
        return _children[index];
    }

    /// <summary>
    /// Get a child instance by name.
    /// </summary>
    /// <param name="name">Name of child to retrieve.</param>
    /// <returns>Child record.</returns>
    public IFamilyMember GetChild(string name)
    {
        return _children.Where(c => c.Name == name).First();
    }

    /// <summary>
    /// Get collection of children.
    /// </summary>
    /// <returns>Collection of children.</returns>
    public IEnumerable<IFamilyMember> GetChildren()
    {
        return _children;
    }

    public IEnumerator<IFamilyMember> GetEnumerator()
    {
        return ((IEnumerable<IFamilyMember>)_children).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<IFamilyMember>)_children).GetEnumerator();
    }

    /// <summary>
    /// Output current children list in human-readable form.
    /// </summary>
    public void LogChildren()
    {
        Logging.LineSeparator();
        // Check if person has any children.
        if (GetChildren().Any())
        {
            // Output person's name, number of children, and list of child names.
            Logging.Log($"{Name} has ({GetChildren().Count()}) children:\n{String.Join("\n", GetChildren().Select(c => c.Name))}");
        }
        else
        {
            // No children to output.
            Logging.Log($"{Name} has no children.");
        }
    }
}
```

Our `Person` class implements our simple `IFamilyMember` interface and, therefore, has become the `composite` object of our example.  In order to behave as a composite we need to provide basic operations within it that allow it to alter underlying children objects, which are stored privately in the `_children` property.  `_children` is just a `List<IFamilyMember>`, so we can start to see the potential for recursion that we're after.

To complete the capabilities we've added a few helper methods to `Person` like `AddChild()`, `RemoveChild()`, and `GetChild()`.  We've also added the `GetChildren()` method to get the full list of children, plus a helper `LogChildren()` method to output the list so we can see what's going on during testing.

With everything in place we can now try creating a few people and adding some relationships, like so:

```cs
class Program
{
    static void Main(string[] args)
    {
        // Parent 1
        Person john = new Person("John");
        // Parent 2
        Person jane = new Person("Jane");

        // Output children
        john.LogChildren();
        jane.LogChildren();

        // Child 1
        Person alice = new Person("Alice");
        // Child 2
        Person billy = new Person("Billy");
        // Child 3
        Person christine = new Person("Christine");

        // John and Jane are both parents of Alice
        john.AddChild(alice);
        jane.AddChild(alice);

        // John is also Billy's parent
        john.AddChild(billy);

        // Jane is also Christine's parent
        jane.AddChild(christine);

        // Output children
        john.LogChildren();
        jane.LogChildren();

        // Since David is John's brother he is also an uncle.
        Uncle david = new Uncle("David");

        // David and John are both the children of their father Edward.
        Person edward = new Person("Edward");
        edward.AddChild(john);
        // Even though 'david' is class of Uncle it can still be added
        // as a child.
        edward.AddChild(david);

        // Output edward's children.
        edward.LogChildren();
    }
}
```

We start by creating some new `Person` instances to represent the parents (`Jane` and `John`) and their children (`Alice`, `Billy`, and `Christine`).  Before we do anything else we want to confirm that `Jane` and `John` don't have any children so we output that and confirm:

```
John has no children.
--------------------
Jane has no children.
```

Getting away from the nuclear family a bit, as it happens `Jane` and `John` have one child together (`Alice`), but each of them also have a child from before they got together (`Billy` for `John` and `Christine` for `Jane`).  Thus when we `LogChildren()` after adding their respective kids we get a new output:

```
John has (2) children:
Alice
Billy
--------------------
Jane has (2) children:
Alice
Christine
```

What makes the `composite` pattern particularly cool is that we can expand on the base interface that our object is using -- `IFamilyMember` in this case -- to create other types of components known as `leaf components`.  In this case we've added the `Aunt` and `Uncle` leaf components, both of which must use the `IFamilyMember` interface:

```cs
/// <summary>
/// Leaf class to act as Aunt.
/// </summary>
public class Aunt : IFamilyMember
{
    public string Name { get; set; }

    public Aunt(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Leaf class to act as Uncle.
/// </summary>
public class Uncle : IFamilyMember
{
    public string Name { get; set; }

    public Uncle(string name)
    {
        Name = name;
    }
}
```

This allows us to continue our example from above by introducing a few new people and relationships by using one of the `leaf component` classes:

```cs
// Since David is John's brother he is also an uncle.
Uncle david = new Uncle("David");

// David and John are both the children of their father Edward.
Person edward = new Person("Edward");
edward.AddChild(john);
// Even though 'david' is a type of Uncle it 
// can still be added as a child.
edward.AddChild(david);

// Output edward's children.
edward.LogChildren();
```

Here we've added `John's` brother `David`.  Since `John` has children this obviously makes `David` an `Uncle` so we use that leaf component class for him.  Both `David` and `John` are the sons of `Edward`, so we also add him as another `Person` and then add both `David` and `John` as children.  This is a critical benefit of the `composite` pattern: Even though `David` and `John` are actually different object types (`Uncle` and `Person`, respectively) they can both be added in the same way using the same `AddChild()` method of the `Person` `composite` object.  We can confirm this all works with a call to `edward.LogChildren()`:

```
Edward has (2) children:
John
David
```

---

__META DESCRIPTION__

Part 9 of our Software Design Pattern series in which examine the Composite design pattern using fully-functional C# code examples.