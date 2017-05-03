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
            var deck = new Deck();
            Logging.Log($"Card count: {deck.Cards.Count()}");
            Logging.Log(deck);
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

    public interface IDeck
    {
        int CardCount { get; set; }
    }

    public class Deck
    {
        public List<Card> Cards { get; set; }
        
        public Deck()
        {
            // Query all possible card combinations and create list of Card class instances.
            var cardList = from suit in Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList()
                           from rank in Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList()
                           select new Card(suit, rank);
            // Assign list to Cards property.
            Cards = cardList.ToList<Card>();
        }
    }
}
