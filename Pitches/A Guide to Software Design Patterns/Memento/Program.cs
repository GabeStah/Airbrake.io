using System;
using System.Collections.Generic;
using System.Linq;
using Command;
using Utility;

namespace Memento
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an originator.
            var originator = new Originator<Character>();
            // Create a caretaker with passed originator instance.
            var caretaker = new Caretaker<Character>(originator);

            // Create Alice character with default stats.
            var alice = new Character("Alice");
            // Create others characters with initial stats.
            var bob = new Character("Bob", 12, 10, 11);
            var christine = new Character("Christine", 25, -4, 0);

            // Set state to Alice.
            originator.SetState(alice);
            // Save state.
            caretaker.Save();

            // Set state to Bob.
            originator.SetState(bob);
            var bobMemento = caretaker.Save();

            // Set state to Christine.
            originator.SetState(christine);
            caretaker.Save();

            // Restore state back to Bob.
            caretaker.Restore(bobMemento);
        }
    }

    /// <summary>
    /// Simple object that allows us to store basic state value.
    /// </summary>
    /// <typeparam name="T">Type of State object.</typeparam>
    internal class Memento<T>
    {
        public T State { get; set; }

        public Memento() { }

        public Memento(T state)
        {
            State = state;
        }

        public override string ToString()
        {
            return State.ToString();
        }
    }

    /// <summary>
    /// Basic event args used to pass a Memento instance when event fires.
    /// </summary>
    /// <typeparam name="T">Object type of Memento.</typeparam>
    internal class MementoChangedEventArgs<T> : EventArgs
    {
        internal Memento<T> Memento { get; set; }
    }

    /// <summary>
    /// Gets/sets values of Mementoes.
    /// Creates new Mementoes and assigns current values to them.
    /// </summary>
    /// <typeparam name="T">Type of State object.</typeparam>
    internal class Originator<T>
    {
        private T _state;

        /// <summary>
        /// Creates a new Memento instance using _state.
        /// </summary>
        /// <returns>Newly generated Memento instance.</returns>
        public Memento<T> CreateMemento()
        {
            // Create memento and set state to current state.
            var memento = new Memento<T>(_state);

            // Invoke event handler.
            OnMementoChanged(new MementoChangedEventArgs<T>
            {
                Memento = memento,
            });

            return memento;
        }

        /// <summary>
        /// Set current state based on passed Memento.State property.
        /// </summary>
        /// <param name="memento">Memento from which to get State property.</param>
        public void SetMemento(Memento<T> memento)
        {
            _state = memento.State;

            // Invoke event handler.
            OnMementoChanged(new MementoChangedEventArgs<T>
            {
                Memento = memento,
            });
        }

        /// <summary>
        /// Explicitly set the state property.
        /// </summary>
        /// <param name="state">State object to set.</param>
        public void SetState(T state)
        {
            _state = state;
        }

        /// <summary>
        /// Handler for event when Memento is changed.
        /// </summary>
        public event EventHandler<MementoChangedEventArgs<T>> MementoChanged;

        /// <summary>
        /// Fires when Memento is changed.
        /// </summary>
        /// <param name="e">Event args containing Memento instance.</param>
        protected virtual void OnMementoChanged(MementoChangedEventArgs<T> e)
        {
            // Invoke event.
            MementoChanged?.Invoke(this, e);

            // Output log message with changed Memento.
            Logging.LineSeparator("MEMENTO CHANGED");
            Logging.Log(e.Memento.ToString());
        }
    }

    /// <summary>
    /// Holds a collection that contains all previous versions of the Memento.
    /// Can store and retrieve Mementos.
    /// </summary>
    /// <typeparam name="T">Type of Memento object.</typeparam>
    internal class Caretaker<T>
    {
        private static readonly List<Memento<T>> MementoList = new List<Memento<T>>();
        private Originator<T> Originator { get; set; }

        public Caretaker(Originator<T> originator)
        {
            Originator = originator;
        }

        /// <summary>
        /// Save Memento of Originator by creating new 
        /// Memento and adding to list.
        /// </summary>
        /// <returns>Created Memento instance.</returns>
        public Memento<T> Save()
        {
            var memento = Originator.CreateMemento();
            MementoList.Add(memento);
            return memento;
        }

        /// <summary>
        /// Restore Originator to Memento via passed list index.
        /// </summary>
        /// <param name="index">Index of Memento instance.</param>
        public void Restore(int index)
        {
            // Find match.
            var match = MementoList[index];

            // Can't restore if not in the list.
            if (match == null)
            {
                throw new ArgumentException($"Memento at index [{index}] not found, cannot restore.");
            }

            // Restore Memento.
            Originator.SetMemento(match);
        }

        /// <summary>
        /// Restore Originator to passed Memento state, if exists.
        /// </summary>
        /// <param name="memento">Memento to be restored to</param>
        public void Restore(Memento<T> memento)
        {
            // Find match.
            var match = MementoList.FirstOrDefault(x => x == memento);

            // Can't restore if not in the list.
            if (match == null)
            {
                throw new ArgumentException($"Memento [{memento}] not found, cannot restore.");
            }
            
            // Restore Memento.
            Originator.SetMemento(match);
        }
    }
}
