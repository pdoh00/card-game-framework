using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Events;
using Infrastructure;

namespace Game.OFCP
{
    /// <summary>
    /// Standard poker deck.  
    /// 52 cards 
    /// 4 suits - Clubs, Hearts, Diamonds, Spades
    /// 13 ranks 2,3,...,J,Q,K,A
    /// </summary>
    public class StandardDeck
    {
        private const int STANDARD_DECK_SIZE = 52;
        private readonly IShuffler Shuffler;
        private readonly string _gameid;
        protected List<Card> Cards;
        private int[] deck = new int[STANDARD_DECK_SIZE];


        public StandardDeck(IShuffler shuffler, string gameId)
            : base()
        {
            _gameid = gameId;
            Shuffler = shuffler;
            PokerCalculator.PokerLib.init_deck(deck);
        }

        public void Shuffle()
        {
            Shuffler.Shuffle(deck);
            BuildDeck();
            //Apply(new DeckShuffledEvent(_gameid, Cards));
        }

        private void BuildDeck()
        {
            Cards = new List<Card>(STANDARD_DECK_SIZE);
            for (int i = 0; i < deck.Length; i++)
            {
                var card = new int[1];
                card[0] = deck[i];
                Cards.Add(new Card(deck[i], PokerCalculator.PokerLib.print_hand(card, card.Length)));
            }
        }

        /// <summary>
        /// Returns top n cards from the deck;
        /// </summary>
        /// <param name="numCards"></param>
        /// <returns>A List of Card</returns>
        /// <exception cref="InvalidOperationException">When take more than available cards.</exception>
        public List<Card> Deal(int numCards)
        {
            if (numCards > RemainingCards())
                throw new InvalidOperationException("The deck doesn't have enough cards to satisfy the Take request.");

            var hand = new List<Card>();
            for (int i = 0; i < numCards; i++)
            {
                hand.Add(Cards[i]);
            }
            Cards.RemoveRange(0, numCards);
            return hand;
        }

        public bool IsEmpty()
        {
            return Cards.Count == 0;
        }

        public int RemainingCards()
        {
            return Cards.Count;
        }

        public override string ToString()
        {
            return PokerCalculator.PokerLib.print_hand(deck, deck.Length);
        }

        
    }
}
