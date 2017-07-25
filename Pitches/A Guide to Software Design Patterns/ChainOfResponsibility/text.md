# Behavioral Design Patterns: Chain of Responsibility

Today we'll begin the last leg of our journey through all the most common design patterns within our [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series.  It's finally time to move onto `Behavioral` patterns, which try to build a foundation on which objects can communicate and assign responsibilities to one another.  First up in the `Behavioral` pattern list is the Chain of Responsibility design pattern, which makes it easy to chain objects together in an ordered set.  A specific, logical requirement is passed into the chain and is checked against each object in the set, in order, until a suitable match is found that meets the needs of the particular requirement.

In this article we'll explore the `chain of responsibility design pattern` in more detail, looking at both a real world example and a fully-functional C# code sample that will illustrate how you might use the pattern in your own code, so let's get going!

## In the Real World

Upon hearing the term `chain of responsibility`, it isn't a far stretch to think of the similar phrase known as a `chain of command`.  In point of fact, both terms are often used interchangeably to mean the same thing.  However, when dealing with programming, the concept of `command` isn't always applicable (or, more likely, could be confused for its other meanings), so the chain of responsibility pattern was born and is generally more representative of the intended goals.

That said, one obvious real world example of a `chain of responsibility` pattern is the order of command in a governmental or business organization.  For example, The United States Presidency is held by the current President, but an extensive [line of succession](https://en.wikipedia.org/wiki/United_States_presidential_line_of_succession) is laid out through the United States Constitution and the [Presidential Succession Act of 1947](https://en.wikipedia.org/wiki/Presidential_Succession_Act).  In the event that the current President cannot continue with his or her responsibilities, the Presidency would pass down to the Vice President, then the Speaker of the House of Representatives, and so forth.

While a chain of succession begins from the top of the chain and works its way downward, a `chain of responsibility` typically works its way upward, starting from the bottom.  This practice is commonly seen in the realm of business, when an important decision must be made within a company.  The decision must pass up the chain of command, from employee to supervisors to manager to president and so on, until an individual with enough authority to make an appropriate decision is found.

## How It Works In Code

We'll be basing our code sample off the real world `chain of responsibility` pattern found in many companies and put into practice via its employees.  Specifically, imagine we starting a publishing company and we have a series of books we want to publish.  Each book is assigned a type of cover (paperback, digital, or hard cover), along with an estimated publishing cost.  We'll then create a series of `specifications` (using the [`specifications pattern`](https://en.wikipedia.org/wiki/Specification_pattern)) that will define various criteria to check for our books, such as their cover type or publication cost.  We'll create a list of employees with varying authority levels (CEO, President, CFO, etc), and assign each employee a set of specifications that will determine what types of books that employee is qualified to approve for publication.  Finally, with everything in place, we can loop through each book and run up the `chain of responsibility` of each employee, checking whether he or she has the proper authority to publish that book.  If so, publication occurs, and if not, the next employee up the chain is checked for authority.

That's the basic explanation of what our code accomplishes.  As usual, we'll start with the full code sample below, and then we'll dive into the details and explain what's going on afterward:

```cs
using System;
using System.Collections.Generic;
using Utility;

namespace ChainOfResponsibility
{
    class Program
    {
        static void Main(string[] args)
        {
           // Create a list of books to be published, including cover type and publication cost.
            var books = new List<Book> {
                new Book("The Stand", "Stephen King", CoverType.Paperback, 35000),
                new Book("The Hobbit", "J.R.R. Tolkien", CoverType.Paperback, 25000),
                new Book("The Name of the Wind", "Patrick Rothfuss", CoverType.Digital, 7500),
                new Book("To Kill a Mockingbird", "Harper Lee", CoverType.Hard, 65000),
                new Book("1984", "George Orwell", CoverType.Paperback, 22500) ,
                new Book("Jane Eyre", "Charlotte Brontë", CoverType.Hard, 82750)
            };

            // Create specifications for individual cover types.
            var digitalCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Digital);
            var hardCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Hard);
            var paperbackCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Paperback);

            // Create budget spec for cost exceeding $75000.
            var extremeBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 75000);
            // Create budget spec for cost between $50000 and $75000.
            var highBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 50000 && book.PublicationCost < 75000);
            // Create budget spec for cost between $25000 and $50000.
            var mediumBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 25000 && book.PublicationCost < 50000);
            // Create budget spec for cost below $25000.
            var lowBudgetSpec = new Specification<Book>(book => book.PublicationCost < 25000);

            // Default spec, always returns true.
            var defaultSpec = new Specification<Book>(book => true);

            // Create publication process instance, used to pass Action<T> to Employee instances.
            var publicationProcess = new PublicationProcess();

            // Create employees with various positions.
            var ceo = new Employee<Book>("Alice", Position.CEO, publicationProcess.PublishBook);
            var president = new Employee<Book>("Bob", Position.President, publicationProcess.PublishBook);
            var cfo = new Employee<Book>("Christine", Position.CFO, publicationProcess.PublishBook);
            var director = new Employee<Book>("Dave", Position.DirectorOfPublishing, publicationProcess.PublishBook);
            // Default employee, used as successor of CEO and to handle unpublishable books.
            var defaultEmployee = new Employee<Book>("INVALID", Position.Default, publicationProcess.FailedPublication);

            // Director can handle digital low budget only.
            director.SetSpecification(digitalCoverSpec.And<Book>(lowBudgetSpec));

            // CFO can handle digital/paperbacks that are medium or high budget.
            cfo.SetSpecification(digitalCoverSpec.Or<Book>(paperbackCoverSpec).And<Book>(mediumBudgetSpec.Or<Book>(highBudgetSpec)));

            // President can handle all medium/high budget.
            president.SetSpecification(mediumBudgetSpec.Or<Book>(highBudgetSpec));

            // CEO can handle all extreme budget.
            ceo.SetSpecification(extremeBudgetSpec);

            // Default employee can handle only default specification (all).
            defaultEmployee.SetSpecification(defaultSpec);

            // Set chain of responsibility: CEO > President > CFO > Director.
            director.SetSuccessor(cfo);
            cfo.SetSuccessor(president);
            president.SetSuccessor(ceo);
            ceo.SetSuccessor(defaultEmployee);

            // Loop through books, trying to publish, starting at bottom of chain of responsibility (Director).
            books.ForEach(book => director.PublishBook(book));
        }
    }

    /// <summary>
    /// Defines success/failure actions to use as Actions if Employee can approve a publication.
    /// </summary>
    public class PublicationProcess
    {
        public void PublishBook(Book book)
        {
            book.Publish();
        }

        public void FailedPublication(Book book)
        {
            Logging.Log($"Unable to publish: {book}.");
        }
    }

    public enum CoverType
    {
        Digital,
        Hard,
        Paperback
    }

    public interface IPublishable
    {
        string Author { get; set; }
        CoverType CoverType { get; }
        decimal PublicationCost { get; set; }
        void Publish();
        string Title { get; set; }
    }

    /// <summary>
    /// Book class that includes author, title, publication cost, and cover type.
    /// </summary>
    public class Book : IPublishable
    {
        public string Author { get; set; }
        public CoverType CoverType { get; set; }
        public decimal PublicationCost { get; set; }
        public string Title { get; set; }

        public Book(string title, string author, CoverType coverType, decimal publicationCost)
        {
            this.Author = author;
            this.PublicationCost = publicationCost;
            this.CoverType = coverType;
            this.Title = title;
        }

        /// <summary>
        /// Simulates publication of the book via confirmation output.
        /// </summary>
        public void Publish()
        {
            Logging.Log($"Successfully published {this}.");
        }

        /// <summary>
        /// Converts this Book object to string format.
        /// </summary>
        /// <returns>Formatted book information.</returns>
        public override string ToString()
        {
            return $"{CoverType} cover '{Title}' by {Author} for {PublicationCost:C2}";
        }
    }

    /// <summary>
    /// Defines the specification pattern interface.
    /// </summary>
    /// <typeparam name="T">Type of object specification applies to.</typeparam>
    public interface ISpecification<in T>
    {
        /// <summary>
        /// Checks if current specification is satisfied by passed expression.
        /// </summary>
        /// <param name="expression">Expression to check.</param>
        /// <returns>Result.</returns>
        bool IsSatisfiedBy(T expression);
    }

    /// <summary>
    /// Basic specification that checks if passed function expression is satisfied by other expressions.
    /// 
    /// Extensions will allow multiple expressions to be chained together.
    /// </summary>
    /// <typeparam name="T">Type of object specification applies to.</typeparam>
    public class Specification<T> : ISpecification<T>
    {
        private readonly Func<T, bool> _expression;
        public Specification(Func<T, bool> expression)
        {
            this._expression = expression;
        }

        public bool IsSatisfiedBy(T expression)
        {
            return this._expression(expression);
        }
    }

    /// <summary>
    /// Extends Specification class with logical And, Or, and Not methods.
    /// </summary>
    public static class SpecificationExtensions
    {
        public static Specification<T> And<T>(this ISpecification<T> a, ISpecification<T> b)
        {
            if (a != null && b != null)
            {
                // Check if both a AND b satisfy expression.
                return new Specification<T>(expression => a.IsSatisfiedBy(expression) && b.IsSatisfiedBy(expression));
            }
            return null;
        }
        public static Specification<T> Or<T>(this ISpecification<T> a, ISpecification<T> b)
        {
            if (a != null && b != null)
            {
                // Check if either a OR b satisfy expression.
                return new Specification<T>(expression => a.IsSatisfiedBy(expression) || b.IsSatisfiedBy(expression));
            }
            return null;
        }
        public static Specification<T> Not<T>(this ISpecification<T> a)
        {
            // Check .
            return a != null ? new Specification<T>(expression => !a.IsSatisfiedBy(expression)) : null;
        }
    }

    /// <summary>
    /// Potential employee positions.
    /// </summary>
    public enum Position
    {
        CEO,
        President,
        CFO,
        DirectorOfPublishing,
        Default
    }
    
    /// <summary>
    /// Basic employee interface.
    /// 
    /// Acts as the Handler in Chain of Responsibility.
    /// </summary>
    /// <typeparam name="T">Type of object employee will be dealing with.</typeparam>
    public interface IEmployee<T>
    {
        void PublishBook(T book);
        void SetSpecification(ISpecification<T> specification);
        void SetSuccessor(IEmployee<T> employee);
    }

    /// <summary>
    /// Employee class used to create basic chain of responsibility.
    /// 
    /// Acts as the ConcreteHandler in Chain of Responsibility.
    /// </summary>
    /// <typeparam name="T">Type of object employee will be dealing with.</typeparam>
    public class Employee<T> : IEmployee<T> where T : IPublishable
    {
        private IEmployee<T> _successor;
        private readonly string _name;
        private ISpecification<T> _specification;
        private readonly Action<T> _publicationAction;
        private readonly Position _position;

        public Employee(string name, Position position, Action<T> publicationAction)
        {
            _name = name;
            _position = position;
            _publicationAction = publicationAction;
        }

        /// <summary>
        /// Check if Employee can approve book for publication by checking
        /// if current specification is satisfied by book.
        /// </summary>
        /// <param name="book">Book to check for approval.</param>
        /// <returns>Indicates if Employee can approve book or not.</returns>
        public bool CanApprove(T book)
        {
            if (_specification != null && book != null)
            {
                return _specification.IsSatisfiedBy(book);
            }
            return false;
        }

        /// <summary>
        /// Attempts to publish book.
        /// </summary>
        /// <param name="book">Book to attempt publication upon.</param>
        public void PublishBook(T book)
        {
            // Check if Employee has rights to approve publication.
            if (CanApprove(book))
            {
                // Ensure position isn't Default (which indicates no employees in chain can approve).
                if (_position != Position.Default)
                {
                    Logging.Log($"{_position} {_name} approved publication of {book}.");
                }
                // Invoke passed action to publish book.
                _publicationAction.Invoke(book);
                Logging.LineSeparator();
            }
            else
            {
                // If unable to approve, check if successor and try to publish book as successor.
                _successor?.PublishBook(book);
            }
        }

        public void SetSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public void SetSuccessor(IEmployee<T> employee)
        {
            _successor = employee;
        }
    }
}
```

---

_Note_: As you can see, this code sample is quite massive and somewhat complex.  As such, to save space in this article, we've excluded the code of the `Logging` class that is used to output information to the console.  Feel free to check out previous design pattern articles, such as our [`Proxy Design Pattern`](https://airbrake.io/blog/design-patterns/proxy-design-pattern) article, if you need that code.  Otherwise, this sample will work perfectly fine by replacing all `Logging.Log()` method calls with `Console.WriteLine()` calls instead.

The chain of responsibility design pattern consists of just two fundamental objects:

- `Handler Interface` - Defines the methods and properties that will be used to handle requests.
- `ConcreteHandler Class` - Handles all requests and stores a `successor`.  If the current `ConcreteHandler` can manage a request, it does so, otherwise it passes it along up the chain to the `successor`.

For our purposes, we've defined the `IEmployee` interface to behave as the `Handler` interface, along with the `Employee` class, which implements `IEmployee` and acts as our `ConcreteHandler`:

```cs
/// <summary>
/// Potential employee positions.
/// </summary>
public enum Position
{
    CEO,
    President,
    CFO,
    DirectorOfPublishing,
    Default
}

/// <summary>
/// Basic employee interface.
/// 
/// Acts as the Handler in Chain of Responsibility.
/// </summary>
/// <typeparam name="T">Type of object employee will be dealing with.</typeparam>
public interface IEmployee<T>
{
    void PublishBook(T book);
    void SetSpecification(ISpecification<T> specification);
    void SetSuccessor(IEmployee<T> employee);
}

/// <summary>
/// Employee class used to create basic chain of responsibility.
/// 
/// Acts as the ConcreteHandler in Chain of Responsibility.
/// </summary>
/// <typeparam name="T">Type of object employee will be dealing with.</typeparam>
public class Employee<T> : IEmployee<T> where T : IPublishable
{
    private IEmployee<T> _successor;
    private readonly string _name;
    private ISpecification<T> _specification;
    private readonly Action<T> _publicationAction;
    private readonly Position _position;

    public Employee(string name, Position position, Action<T> publicationAction)
    {
        _name = name;
        _position = position;
        _publicationAction = publicationAction;
    }

    /// <summary>
    /// Check if Employee can approve book for publication by checking
    /// if current specification is satisfied by book.
    /// </summary>
    /// <param name="book">Book to check for approval.</param>
    /// <returns>Indicates if Employee can approve book or not.</returns>
    public bool CanApprove(T book)
    {
        if (_specification != null && book != null)
        {
            return _specification.IsSatisfiedBy(book);
        }
        return false;
    }

    /// <summary>
    /// Attempts to publish book.
    /// </summary>
    /// <param name="book">Book to attempt publication upon.</param>
    public void PublishBook(T book)
    {
        // Check if Employee has rights to approve publication.
        if (CanApprove(book))
        {
            // Ensure position isn't Default (which indicates no employees in chain can approve).
            if (_position != Position.Default)
            {
                Logging.Log($"{_position} {_name} approved publication of {book}.");
            }
            // Invoke passed action to publish book.
            _publicationAction.Invoke(book);
            Logging.LineSeparator();
        }
        else
        {
            // If unable to approve, check if successor and try to publish book as successor.
            _successor?.PublishBook(book);
        }
    }

    public void SetSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    public void SetSuccessor(IEmployee<T> employee)
    {
        _successor = employee;
    }
}
```

For now let's ignore the `IPublishable` interface, which `Employee` requires its `Type` to be a member of, and just briefly explain what the class does.  The fundamental requirement is the storage of a `successor`, which is an `IEmployee<T>` instance itself.  This will be used to indicate which _other_ `Employee` is next in the `chain of responsibility`.  We also have a few properties like `name` and `position` that we're using for logic and output elsewhere, to make it easier to differentiate between employees.

Beyond that, our basic logic occurs in `CanApprove(T book)` and `PublishBook(T book)`.  When we want to publish a passed book we first need to check if the current `Employee` has the authority to approve that request, so `CanApprove(T book)` calls the `_specification.IsSatisfiedBy(T book)` method.  We'll explore what this does later, but the purpose here is to check if the current `Employee` meets all the specification criteria required by the book in question.  If so, we `Invoke()` the `_publicationAction` that was passed and output the successful publication result.  On the other hand, if `CanApprove(T book)` is false, we check if `_successor` exists, and if so, we call `PublishBook(T book)` for the _successor_ (the next `Employee` up the `chain of responsibility`).  This process repeats until an `Employee` with the proper specifications necessary to publish the book are found, _or_ until the end of the chain is reached and publication, therefore, fails.

Our `Book` class implements the `IPublishable` interface, which we saw used earlier by the `Employee` class, and simply covers the fundamental properties of a book (title, author, cover type, and publication cost):

```cs
public enum CoverType
{
    Digital,
    Hard,
    Paperback
}

public interface IPublishable
{
    string Author { get; set; }
    CoverType CoverType { get; }
    decimal PublicationCost { get; set; }
    void Publish();
    string Title { get; set; }
}

/// <summary>
/// Book class that includes author, title, publication cost, and cover type.
/// </summary>
public class Book : IPublishable
{
    public string Author { get; set; }
    public CoverType CoverType { get; set; }
    public decimal PublicationCost { get; set; }
    public string Title { get; set; }

    public Book(string title, string author, CoverType coverType, decimal publicationCost)
    {
        this.Author = author;
        this.PublicationCost = publicationCost;
        this.CoverType = coverType;
        this.Title = title;
    }

    /// <summary>
    /// Simulates publication of the book via confirmation output.
    /// </summary>
    public void Publish()
    {
        Logging.Log($"Successfully published {this}.");
    }

    /// <summary>
    /// Converts this Book object to string format.
    /// </summary>
    /// <returns>Formatted book information.</returns>
    public override string ToString()
    {
        return $"{CoverType} cover '{Title}' by {Author} for {PublicationCost:C2}";
    }
}
```

The last set of classes we'll need for our setup is the `ISpecification<T>` interface, and the `Specification<T>` class that implements it.  As previously mentioned, we're using the [`specification pattern`](https://en.wikipedia.org/wiki/Specification_pattern) here, so that we can define loose sets of business rules for our books, which can then be freely applied to our various employees based on their ranks.  To that end, the fundamental behavior of the `specification` pattern is the `IsSatisfiedBy(T expression)` method, which basically checks if a passed in candidate or expression meets the criteria of (is satisfied by) the current specification.  `IsSatisfiedBy(T expression)` returns a boolean indicating the success or failure of this check.

Beyond that we also have a few extensions for the `Specification<T>` class, so we can perform simple `And`, `Or`, or `Not` logic using multiple specifications:

```cs
/// <summary>
/// Defines the specification pattern interface.
/// </summary>
/// <typeparam name="T">Type of object specification applies to.</typeparam>
public interface ISpecification<in T>
{
    /// <summary>
    /// Checks if current specification is satisfied by passed expression.
    /// </summary>
    /// <param name="expression">Expression to check.</param>
    /// <returns>Result.</returns>
    bool IsSatisfiedBy(T expression);
}

/// <summary>
/// Basic specification that checks if passed function expression is satisfied by other expressions.
/// 
/// Extensions will allow multiple expressions to be chained together.
/// </summary>
/// <typeparam name="T">Type of object specification applies to.</typeparam>
public class Specification<T> : ISpecification<T>
{
    private readonly Func<T, bool> _expression;
    public Specification(Func<T, bool> expression)
    {
        this._expression = expression;
    }

    public bool IsSatisfiedBy(T expression)
    {
        return this._expression(expression);
    }
}

/// <summary>
/// Extends Specification class with logical And, Or, and Not methods.
/// </summary>
public static class SpecificationExtensions
{
    public static Specification<T> And<T>(this ISpecification<T> a, ISpecification<T> b)
    {
        if (a != null && b != null)
        {
            // Check if both a AND b satisfy expression.
            return new Specification<T>(expression => a.IsSatisfiedBy(expression) && b.IsSatisfiedBy(expression));
        }
        return null;
    }
    public static Specification<T> Or<T>(this ISpecification<T> a, ISpecification<T> b)
    {
        if (a != null && b != null)
        {
            // Check if either a OR b satisfy expression.
            return new Specification<T>(expression => a.IsSatisfiedBy(expression) || b.IsSatisfiedBy(expression));
        }
        return null;
    }
    public static Specification<T> Not<T>(this ISpecification<T> a)
    {
        // Check .
        return a != null ? new Specification<T>(expression => !a.IsSatisfiedBy(expression)) : null;
    }
}
```

Whew!  Now that everything is setup we can make use of our `chain of responsibility` pattern and test it out.  There's a lot going on in our `Main(string[] args)` method, so we'll break it down into smaller chunks.

We start by creating a new `List<Book>` and add a handful of books, each with a title, author, cover type, and estimated publication cost.  Our goal is to check this set of books, to see which ones can be published, and which `Employee` within our `chain of responsibility` has the authority to do so:

```cs
static void Main(string[] args)
{
    // Create a list of books to be published, including cover type and publication cost.
    var books = new List<Book> {
        new Book("The Stand", "Stephen King", CoverType.Paperback, 35000),
        new Book("The Hobbit", "J.R.R. Tolkien", CoverType.Paperback, 25000),
        new Book("The Name of the Wind", "Patrick Rothfuss", CoverType.Digital, 7500),
        new Book("To Kill a Mockingbird", "Harper Lee", CoverType.Hard, 65000),
        new Book("1984", "George Orwell", CoverType.Paperback, 22500) ,
        new Book("Jane Eyre", "Charlotte Brontë", CoverType.Hard, 82750)
    };

    // ...
}
```

Next we need to create some specifications.  These can be as simple or as complex as we want.  In this case, we start by creating a specification for each `CoverType` of book that we have.  We also create some `PublicationCost`-based specifications, which each cover a unique range of costs:

```cs
 // Create specifications for individual cover types.
var digitalCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Digital);
var hardCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Hard);
var paperbackCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Paperback);

// Create budget spec for cost exceeding $75000.
var extremeBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 75000);
// Create budget spec for cost between $50000 and $75000.
var highBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 50000 && book.PublicationCost < 75000);
// Create budget spec for cost between $25000 and $50000.
var mediumBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 25000 && book.PublicationCost < 50000);
// Create budget spec for cost below $25000.
var lowBudgetSpec = new Specification<Book>(book => book.PublicationCost < 25000);

// Default spec, always returns true.
var defaultSpec = new Specification<Book>(book => true);
```

Now we need to create a few `Employees`, which will serve as the basis of our `chain of responsibility`.  To better differentiate between each employee we assign them a unique `Name` and `Position`.  Lastly, we've created a new `PublicationProcess` instance, which is a simple class that implements two methods, `PublishBook(Book book)` and `FailedPublication(Book book)`.  These two methods will be passed to our various `Employee` instances to indicate the behavior (the publication `Action<T>`) that should be taken when the `Employee` successfully publishes (or fails to publish) a book:

```cs
// Create publication process instance, used to pass Action<T> to Employee instances.
var publicationProcess = new PublicationProcess();

// Create employees with various positions.
var ceo = new Employee<Book>("Alice", Position.CEO, publicationProcess.PublishBook);
var president = new Employee<Book>("Bob", Position.President, publicationProcess.PublishBook);
var cfo = new Employee<Book>("Christine", Position.CFO, publicationProcess.PublishBook);
var director = new Employee<Book>("Dave", Position.DirectorOfPublishing, publicationProcess.PublishBook);
// Default employee, used as successor of CEO and to handle unpublishable books.
var defaultEmployee = new Employee<Book>("INVALID", Position.Default, publicationProcess.FailedPublication);
```

Now that our employees are defines we want to start giving them each various levels of responsibility and authority.  We accomplish this by assigning them sets of specifications.  The comments provide details on what is going on in each example, but the basic goal is to ensure that a particular `Employee` can only handle certain types of books, based on `CoverType` and/or `PublicationCost`:

```cs
// Director can handle digital low budget only.
director.SetSpecification(digitalCoverSpec.And<Book>(lowBudgetSpec));

// CFO can handle digital/paperbacks that are medium or high budget.
cfo.SetSpecification(digitalCoverSpec.Or<Book>(paperbackCoverSpec).And<Book>(mediumBudgetSpec.Or<Book>(highBudgetSpec)));

// President can handle all medium/high budget.
president.SetSpecification(mediumBudgetSpec.Or<Book>(highBudgetSpec));

// CEO can handle all extreme budget.
ceo.SetSpecification(extremeBudgetSpec);

// Default employee can handle only default specification (all).
defaultEmployee.SetSpecification(defaultSpec);
```

Now that each `Employee` has their own set of specifications, the last step is to **create the `chain of responsibility`**.  We do this by calling `SetSuccessor()` for each `Employee`, to indicate which other `Employee` is his or her immediate superior.  We've used `defaultEmployee` throughout this to be the baseline or "fallback" `Employee` instance.  The `Action<T>` that was passed to `defaultEmployee` during instantiation was `publicationProcess.FailedPublication`.  This action allows us to specify that, should a particular book reach the `defaultEmployee` in the chain (above the CEO), that indicates that no actual employee met the criteria of the book, so publication is not possible:

```cs
// Set chain of responsibility: CEO > President > CFO > Director.
director.SetSuccessor(cfo);
cfo.SetSuccessor(president);
president.SetSuccessor(ceo);
ceo.SetSuccessor(defaultEmployee);
```

With everything ready to go, we begin the process of searching up through the `chain of responsibility` for an applicable `Employee` that has the authority to publish each `Book`, starting at the bottom with the `director`:

```cs
// Loop through books, trying to publish, starting at bottom of chain of responsibility (Director).
books.ForEach(book => director.PublishBook(book));
```

The end result of all this after running our code is some output showing us which books in our set were actually published, and by whom:

```
CFO Christine approved publication of Paperback cover 'The Stand' by Stephen King for $35,000.00.
Successfully published Paperback cover 'The Stand' by Stephen King for $35,000.00.
--------------------
CFO Christine approved publication of Paperback cover 'The Hobbit' by J.R.R. Tolkien for $25,000.00.
Successfully published Paperback cover 'The Hobbit' by J.R.R. Tolkien for $25,000.00.
--------------------
DirectorOfPublishing Dave approved publication of Digital cover 'The Name of the Wind' by Patrick Rothfuss for $7,500.00.
Successfully published Digital cover 'The Name of the Wind' by Patrick Rothfuss for $7,500.00.
--------------------
President Bob approved publication of Hard cover 'To Kill a Mockingbird' by Harper Lee for $65,000.00.
Successfully published Hard cover 'To Kill a Mockingbird' by Harper Lee for $65,000.00.
--------------------
Unable to publish: Paperback cover '1984' by George Orwell for $22,500.00.
--------------------
CEO Alice approved publication of Hard cover 'Jane Eyre' by Charlotte Brontë for $82,750.00.
Successfully published Hard cover 'Jane Eyre' by Charlotte Brontë for $82,750.00.
```

For each `Book` in the set we can actually look back at the specifications assigned to each `Employee` in the chain to see why that `Employee` had the authority to approve publication.  In the case of _1984_ by George Orwell, no `Employee` was found that met those specifications.  We can see it's a `paperback` `CoverType` and at `$22,500` would fall into the `lowBudgetSpec`.  However, looking back at our `Employee` specifications we can see that none of them meet both those criteria.  The `director` meets the `lowBudgetSpec`, but _only_ for books that are `digital` `CoverType`.  As a result, our `defaultEmployee` ended up being the only candidate, which forced a call to `FailedPublication()`, hence the output that we see.

---

This is a small taste of what the chain of responsibility design pattern is capable of.  It can be quite useful in certain situations, and when combined with other patterns like the specification pattern used above, it can be both powerful and robust.  For more information on all the other popular design patterns, take a look at our [ongoing design pattern series over here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 14 of our Software Design Pattern series in which examine the chain of responsibility design pattern using fully-functional C# example code.

---

__SOURCES__

- https://www.codeproject.com/Articles/743783/Reusable-Chain-of-responsibility-in-Csharp
- https://en.wikipedia.org/wiki/Specification_pattern