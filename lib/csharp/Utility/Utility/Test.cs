using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class Test
    {
        public static void EnumerableOutputTest()
        {
            // Parent 1
            Person john = new Person("John");
            // Parent 2
            Person jane = new Person("Jane");

            // Child 1
            Person alice = new Person("Alice");
            // Child 2
            Person alex = new Person("Alex");
            // Child 3
            Person adam = new Person("Adam");

            // Output children
            Logging.Log(john);
            Logging.Log(jane);

            // Add children
            john.AddChild(alice);
            john.AddChild(alex);
            john.AddChild(adam);

            jane.AddChild(alice);
            jane.AddChild(alex);
            jane.AddChild(adam);

            // Output children
            Logging.Log(john.Children.ToArray());
            Logging.Log(jane);
        }

        public interface IFamilyMember
        {
            string Name { get; set; }
        }

        public class Person : IFamilyMember, IEnumerable<IFamilyMember>
        {
            public List<IFamilyMember> Children = new List<IFamilyMember>();

            public string Name { get; set; }

            public Person(string name)
            {
                Name = name;
            }

            public void AddChild(IFamilyMember child)
            {
                Children.Add(child);
            }

            public void RemoveChild(IFamilyMember child)
            {
                Children.Remove(child);
            }

            public IFamilyMember GetChild(int index)
            {
                return Children[index];
            }

            public IFamilyMember GetChild(string name)
            {
                return Children.Where(c => c.Name == name).First();
            }

            public IEnumerable<IFamilyMember> GetChildren()
            {
                return Children;
            }

            public IEnumerator<IFamilyMember> GetEnumerator()
            {
                return ((IEnumerable<IFamilyMember>)Children).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<IFamilyMember>)Children).GetEnumerator();
            }

            //public IEnumerator<IFamilyMember> GetEnumerator()
            //{
            //    foreach (IFamilyMember child in _children)
            //    {
            //        yield return child;
            //    }
            //}

            //IEnumerator IEnumerable.GetEnumerator()
            //{
            //    return GetEnumerator();
            //}
        }
    }
}