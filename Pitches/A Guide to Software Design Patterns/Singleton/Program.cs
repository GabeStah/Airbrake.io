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
            //NonThreadSafeExample();
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
