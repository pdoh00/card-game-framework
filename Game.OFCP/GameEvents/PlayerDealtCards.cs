using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public class PlayerDealtCards : Event
    {
        public readonly string PlayerId;
        public readonly string GameId;
        public readonly int RoundId;
        public readonly IEnumerable<Card> Cards;

        public PlayerDealtCards(string playerId, string gameId, int roundId, IEnumerable<Card> cards)
        {
            PlayerId = playerId;
            GameId = gameId;
            RoundId = roundId;
            Cards = cards;
        }

        public override string ToString()
        {
            string cards = String.Empty;
            foreach (var c in Cards)
            {
                cards += c.ToString() + " ";
            }
            return base.ToString() + String.Format("Player Id {0} in Game {1} Round {2} dealt cards {3}",
                PlayerId,
                GameId,
                RoundId,
                cards);
        }
    }
}
