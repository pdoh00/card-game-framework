using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP.Events
{
    public class TableCreatedEvent : Event
    {
        public readonly string TableId;
        public readonly int PlayerCapacity;
        public readonly GameType GameType;
        public readonly DateTime TableCreationTime;

        public TableCreatedEvent(string tableId, DateTime tableCreationTime, int playerCapacity, GameType gameType)
        {
            TableId = tableId;
            PlayerCapacity = playerCapacity;
            GameType = gameType;
            TableCreationTime = tableCreationTime;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("New {0} table created with id:{1} @ {2}. The player capacity is {3}", 
                GameType, 
                TableId, 
                TableCreationTime, 
                PlayerCapacity);
        }
    }
}
