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
        public readonly string GameId;
        public readonly string PlayerId;

        public PlayerReadyEvent(string tableId, string gameId, string playerId)
            : base()
        {
            TableId = tableId;
            GameId = gameId;
            PlayerId = playerId;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Player {0} ready for game {1} on table {2}",
                PlayerId,
                GameId,
                TableId);
        }
    }
}
