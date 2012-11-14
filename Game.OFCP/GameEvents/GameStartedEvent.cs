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
        public readonly string GameId;
        public readonly string TableId;
        public readonly DateTime GameStartTime;
        public GameStartedEvent(string gameId, string tableId, DateTime gameStartTime)
        {
            GameId = gameId;
            TableId = tableId;
            GameStartTime = gameStartTime;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Game {0} Started at {1}on Table {2}", 
                GameId,
                GameStartTime,
                TableId);
        }
    }
}
