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