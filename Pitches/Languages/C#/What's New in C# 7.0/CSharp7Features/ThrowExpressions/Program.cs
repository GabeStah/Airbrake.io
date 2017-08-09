using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace ThrowExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    internal class User
    {
        private string _email;
        private string _name;

        /// <summary>
        /// Email property with expression body syntax getter
        /// and throw expression for setter.
        /// </summary>
        internal string Email
        {
            get => _email;
            set => throw new NotImplementedException("User.Email.set is not implemented.");
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
}
