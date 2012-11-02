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
        public readonly string GameTypeId;

        public GameCreatedEvent(string tableId, string gameId, string gameTypeId, DateTime gameStartTime)
        {
            TableId = tableId;
            GameId = gameId;
            GameTypeId = gameTypeId;
            GameCreationTime = gameStartTime;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Game Id: {0}Type: {1} Created on table {2}", 
                GameId,
                GameTypeId,
                TableId);
            
        }
    }
}
