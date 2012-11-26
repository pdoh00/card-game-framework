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
    /// 
    /// Uses the XPokerEval.CactusKev.CSharp library for building the deck
    /// </summary>
    public class StandardDeck
    {
        private const int STANDARD_DECK_SIZE = 52;
        private readonly IShuffler Shuffler;
        private readonly string _gameid;
        protected HashSet<Card> Cards;
        private int[] deck = new int[STANDARD_DECK_SIZE];


        private int[] expectedDeck = new int[STANDARD_DECK_SIZE]
        {
            69634, 73730, 81922, 98306, 135427, 139523,   
            147715,164099, 266757, 270853, 279045, 295429,   
            529159, 533255, 541447, 557831, 1053707, 1057803,  
            1065995, 1082379, 2102541, 2106637, 2114829, 2131213,  
            4199953, 4204049, 4212241, 4228625, 8394515, 8398611,  
            8406803, 8423187, 16783383, 16787479, 16795671, 16812055, 
            33560861, 33564957, 33573149, 33589533, 67115551, 67119647, 
            67127839, 67144223, 134224677, 134228773, 134236965, 134253349,
            268442665, 268446761, 268454953, 268471337
        };

        public StandardDeck(IShuffler shuffler, string gameId)
            : base()
        {
            _gameid = gameId;
            Shuffler = shuffler;
            Cards = new HashSet<Card>();
            PokerCalculator.PokerLib.init_deck(deck);

            var ed = new HashSet<int>(expectedDeck);
            var actDeck = new HashSet<int>(deck);
            if (!actDeck.SetEquals(ed)) throw new ApplicationException("The deck returned from the PokerLib is corrupt");
        }

        public void Shuffle()
        {
            Shuffler.Shuffle(deck);
            BuildDeck();
        }

        private void BuildDeck()
        {
            Cards.Clear();
            for (int i = 0; i < deck.Length; i++)
            {
                var card = new int[1];
                card[0] = deck[i];
                Cards.Add(new Card(deck[i], PokerCalculator.PokerLib.print_hand(card, 1).Trim()));
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

            //var hand = new List<Card>();
            var hand = Cards.Take(numCards).ToList();

            for (int i = 0; i < hand.Count; i++)
            {
                Cards.Remove(hand[i]);
                //hand.Add(Cards[i]);
            }
            //Cards.RemoveRange(0, numCards);
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
