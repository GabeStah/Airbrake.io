using Utility;

namespace ExpressionBodiedMembers
{
    class Program
    {
        static void Main(string[] args)
        {
            var users = new UserCollection();
            Logging.Log(users[1].ToString());
        }
    }

    internal class User
    {
        private string _email;
        private string _name;

        /// <summary>
        /// Email property with expression body syntax.
        /// </summary>
        internal string Email
        {
            get => _email;
            set => _email = value;
        }

        /// <summary>
        /// Name property with expression body syntax.
        /// </summary>
        internal string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Constructor with expression body syntax.
        /// </summary>
        /// <param name="email"></param>
        internal User(string email) => _email = email;

        internal User(string name, string email)
        {
            _email = email;
            _name = name;
        }

        /// <summary>
        /// Override ToString() method with expression body syntax.
        /// </summary>
        /// <returns>Name and email combination.</returns>
        public override string ToString() => $"{Name} - {Email}";

        /// <summary>
        /// Destructor with expression body syntax.
        /// </summary>
        ~User() => Logging.Log($"{this} is being destroyed.");
    }

    internal class UserCollection
    {
        private readonly User[] _users =
        {
            new User("Alice Smith", "alice.smith@airbrake.io"),
            new User("Bob Smith", "bob.smith@airbrake.io"),
            new User("Christine Parker", "christine.parker@airbrake.io"),
            new User("Danny Danson", "danny.danson@airbrake.io")
        };

        /// <summary>
        /// Indexer with get/set expression body syntax.
        /// </summary>
        /// <param name="index">Index of User.</param>
        /// <returns>User.</returns>
        public User this[int index]
        {
            get => _users[index];
            set => _users[index] = value;
        }
    }
}
