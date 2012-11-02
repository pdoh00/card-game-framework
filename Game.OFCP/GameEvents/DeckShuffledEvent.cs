using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public class DeckShuffledEvent : Event
    {
        public readonly string GameId;
        public IEnumerable<Card> Deck;

        public DeckShuffledEvent(string gameId, IEnumerable<Card> cards)
	    {
            GameId = gameId;
            Deck = cards;
	    }

        public override string ToString()
        {
            string deck = String.Empty;
            foreach (var c in Deck)
            {
                deck += c.ToString() + " ";
            }
            return base.ToString() + String.Format("Deck Shuffled for Game {0}, Cards {1}",
                GameId,
                deck);
        }
    }
}
