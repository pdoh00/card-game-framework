using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP.Events
{
    public class PlayerQuitGame : Event
    {
        public readonly string PlayerId;
        public readonly string GameId;
        public readonly DateTime QuitTime;

        public PlayerQuitGame(string playerId, string gameId, DateTime quitTime)
        {
            PlayerId = playerId;
            GameId = gameId;
            QuitTime = quitTime;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Player {0} quit game {1} @ {2}",
                PlayerId,
                GameId,
                QuitTime);
        }
    }
}
