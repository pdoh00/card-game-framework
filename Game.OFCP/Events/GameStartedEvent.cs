using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public class GameStartedEvent : Event
    {
        public string GameId;
        public readonly DateTime GameStartTime;
        public GameStartedEvent(string gameId, DateTime gameStartTime)
        {
            GameId = gameId;
            GameStartTime = gameStartTime;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Game {0} Started at {1}", 
                GameId,
                GameStartTime);
        }
    }
}
