using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public class GameCreatedEvent : Event
    {
        public readonly string TableId;
        public readonly string GameId;
        public readonly DateTime GameCreationTime;

        public GameCreatedEvent(string tableId, string gameId, DateTime gameStartTime)
        {
            TableId = tableId;
            GameId = gameId;
            GameCreationTime = gameStartTime;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Game Created {0} on table {1}", 
                GameId,
                TableId);
            
        }
    }
}
