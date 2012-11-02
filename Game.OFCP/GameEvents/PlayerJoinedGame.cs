using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP.Events
{
    public class PlayerJoinedGame : Event
    {
        public readonly string PlayerId;
        public readonly string GameId;
        public readonly DateTime JoinTime;
        public readonly string PlayerName;
        public readonly int Position;

        public PlayerJoinedGame(string playerId, 
            string gameId, 
            DateTime joinTime, 
            string playerName,
            int position)
        {
            PlayerId = playerId;
            GameId = gameId;
            JoinTime = joinTime;
            PlayerName = playerName;
            Position = position;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Player {0}-{1} joined game {2} position {3} @ {4}",
                PlayerName,
                PlayerId,
                GameId,
                Position,
                JoinTime);
        }
    }
}
