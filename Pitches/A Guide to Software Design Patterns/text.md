# A Guide to Software Design Patterns

Working with design patterns during software development can be tricky at times.  Their purpose is to provide guidelines for dealing with particular problems that might arise during development.  However, the sheer quantity of potential design patterns, combined with the intricacies of each pattern's inner workings, can often make it challenging to select the correct pattern for your particular project, component, or programming language.

This article will attempt to provide a brief overview of what design patterns are in the realm of software development, and serve as a jumping-off point for future articles, which will further detail each design pattern, including code examples using `C#`.

## The Origins of Software Design Patterns

The groundbreaking book [_Design Patterns: Elements of Reusable Object-Oriented Software_](https://www.amazon.com/Design-Patterns-Elements-Reusable-Object-Oriented/dp/0201633612/), published in 1995, has sold hundreds of thousands of copies to date, and is largely considered one of the foremost authorities on object-oriented theory and software development practices.  In fact, the contents of the book was so influential that the four authors have since been given the nickname: [The Gang of Four](https://en.wikipedia.org/wiki/Design_Patterns) (`GoF`).

The book is roughly split into two parts.  Whereas Chapter 1 deals with general object-oriented design techniques, Chapter 2 introduces a total of `23` different software design patterns, split into three basic categories: `Creational`, `Structural`, and `Behavioral`.  Spanning across seven different example design problems, the various design patterns are shown to be applicable across these seven scenarios, resulting in one of the first published examples of modern software design patterns.

Some criticism was directed at the book shortly after publication.  Much of this criticism focused on the belief that many of the proposed design patterns, which were originally written using the `C++` language, were merely workarounds to deal with missing features of the originating language they were written in.  Counterarguments were made that other languages at the time (Lisp, Dylan, AspectJ, etc), were able to eliminate the need for the majority of the `23` patterns, on account of the syntax and components presented by these other languages.

In the decades since the initial publication of _Design Patterns_, most modern languages have adopted techniques and syntax for built-in support for many of these design patterns, while others remain largely unnecessary.  To provide the most applicable and real-world information possible, we'll briefly define each of the original patterns in the sections below.  Then, in future articles that explore the patterns in more detail, using `C#` code examples, we'll focus on the most relevant patterns that are likely to be used in modern development.

## Creational Patterns

`Creational patterns` emphasize the automatic creation of objects within code, rather than requiring you to instantiate objects directly.  In most cases, this means that a function or method can call the code necessary to instantiate new objects on your behalf, so you only need to explicitly modify that object creation when it is necessary, and allow default behaviors to take over otherwise.

- **Abstract Factory**: Encapsulates groups of factories based on common themes.  Often uses `polymorphism`, the concept in object-oriented programming that allows one `interface` to serve as a basis for multiple functions of different `types`.
- **Builder**: Splits up the `construction` of an object from its `representation`.  This is usually done by defining a `Builder` object that presents methods to update the object, without directly interacting with the object itself.
- **Factory**: Creates objects without the need to specify the exact class or `type` of object to be created.  As the name suggests, this object instantiation is performed through a secondary `Factory` class, again using `polymorphism`.
- **Prototype**: Creates new objects by `prototyping` or cloning a `prototypical instance` of an object.  Effectively, an abstract `Prototype` class is created, and from that base prototype, new secondary `inherited` classes are defined.
- **Singleton**: Restricts the total number of instances of a particular class to only one at a time.  This is commonly used when global access to the object is required across the system, and any changes or queries to the object must be consistent and identical.

## Structural patterns

`Structural patterns` focus on the composition of classes and objects.  By using `inheritance` and `interfaces`, these patterns allow objects to be composed in a manner that provides new functionality.  In most cases, an `interface` in object-oriented programming is an abstract type or class which has no logical code, but instead is used to define method signatures and behaviors for other classes that will implement the `interface`.

- **Adapter**: Allows for an `interface`, which is otherwise incompatible, to be _adapted_ to fit a new class.  Typically, this is performed by creating a new `ClassNameAdapter` class that implements the `interface`, allowing for compatibility across the system.
- **Bridge**: Distinguishes between implementation and abstraction.  Or, put another way, it's a pattern that separates the "look and feel" of code from the "logical behavior" of it, which we often see in websites and other visual applications.
- **Composite**: Groups of objects should behave the same as individual objects from within that group.  Primarily useful when creating a collection of objects that inherit from the same type, yet are uniquely different types themselves.  Since they are of the same `composition` type, their behavior should be identical when combined into a collective group.
- **Decorator**: Dynamically modifies the behavior of an object at run time, typically by wrapping the object in a `decorator` class.  This pattern is commonly used when an object is instantiated, but as code execution progresses, modifications must be made to the object before it is finalized.
- **Facade**: Creates a front-end (`facade`) object that obfuscates and simplifies interactions between it and the more complicated interface behind it.  Commonly used when a complex series of actions must take place to perform a task, where executing each and every task, in order, is too complicated.  Instead, a simple `facade` replaces that series of tasks with a single task to be executed.
- **Flyweight**: Reduces memory and resource usage by sharing data with other, similar objects.  Often relies heavily on `Factory`-style patterns to access and store already generated data during future executions.
- **Proxy**: Defines a wrapper class for an object, which acts as an interface for the wrapped object.  Typically, the `proxy` class attaches additional behavior onto the wrapped object, without the need to modify the base object class behavior.

## Behavioral Patterns

`Behavioral patterns` are concerned with communication and assignment _between_ objects.  

- **Chain of Responsibility**: Forces execution to follow a specific `chain of command` during execution, such that the first object is used, then the second, and so on.  Often used as a failsafe in applications, checking the validity of the primary object, before moving onto the secondary object if the primary fails, and so forth.
- **Command**: Decouples the actions of the client from the behavior of the receiver.  Often through the use of an `interface`, an object can specify individual behavior when a particular command is invoked, while a different object type can use that same command, but invoke its own unique behavior instead.
- **Interpreter**: Defines a series of classes used to `interpret` language syntax from a provided sentence.  Typically, each symbol is defined by one class, and then a syntax tree is used to parse (`interpret`) the overall sentence.
- **Iterator**: Allows access to underlying elements of an object, without exposing those elements or their respective logic.  A very commonly used pattern, often as a simple means of fetching the next item in a list or array of objects.
- **Mediator**: Generates a third party object (`mediator`) that acts as a go-between for interactions between two other similar objects (`colleagues`).  Commonly, this is used when multiple objects need to communicate, but do not (or should not) be aware of the others respective implementation or behavior.
- **Memento**: Stores the state of an object, allowing for restoration (rollback) of the object to a previous state.  This behavior is well-known when using word processors that implement the `undo` feature.
- **Observer**: Creates an event-based dependency between objects, such that an update to the `observed` object causes the `observer` objects to be notified.  Typically, this is found in many languages that utilize `asynchronous` functionality, which requires events to be `observed` and responded to outside of typical execution order.
- **State**: Allows for the behavior of a class to change based on the current `state`.  While these `states` are often changed throughout execution, the implementation of each possible `state` is typically defined by a unique class `interface`.
- **Strategy**: Defines a pattern where logical `strategy` changes based on the current situation.  This is merely an object-oriented extension of common `if-else` statements, by altering the execution of code based on the outcome of previous code.
- **Template**: Allows for a skeletal `template` to be used as the basis for execution, without defining the inner-workings of any individual class or object.  This is commonly seen in web applications, where the visual interface of the application is generated using `templates`, which are created using underlying data, but neither the `template` nor the underlying data are aware of the implementation of the other.
- **Visitor**: Allows for new operations to be added to objects without modifying their original implementation structures.  Typically, the `visitor` class defines unique methods that are shared between it and other objects, without the need for the other object to be aware of the additional functionality.

While this is just a brief description of each design pattern, we hope this serves as a good basis for understanding just how varied design patterns can be, and how useful as well.  In future articles, we'll dive deeper into specific design patterns, and examine how they might be implemented in real-world scenarios, using actual code examples.