using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP.Events
{
    public class PlayerReadyEvent : Event
    {
        public readonly string TableId;
        public readonly string PlayerId;
        public readonly string PlayerName;
        public readonly int Position;

        public PlayerReadyEvent(string tableId, string playerId, string playerName, int position)
            : base()
        {
            TableId = tableId;
            PlayerId = playerId;
            PlayerName = playerName;
            Position = position;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Player {0} ready on table {1}",
                PlayerId,
                TableId);
        }
    }
}
