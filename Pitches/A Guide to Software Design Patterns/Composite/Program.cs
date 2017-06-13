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
