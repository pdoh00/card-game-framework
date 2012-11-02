using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public class NewRoundStarted : Event
    {
        public readonly string GameId;
        public readonly int RoundNumber;
        public readonly DateTime StartTime;

        public NewRoundStarted(string gameId, int roundNumber, DateTime startTime)
        {
            GameId = gameId;
            RoundNumber = roundNumber;
            StartTime = startTime;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Round {0} of Game {1} started @ {2}",
                RoundNumber,
                GameId,
                StartTime);
        }
    }
}
