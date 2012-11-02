using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public class PlayerSeatedEvent : Event
    {
        /// <summary>
        /// The Id of the table.
        /// </summary>
        public readonly string TableId;
        /// <summary>
        /// The Id of the player that joined.
        /// </summary>
        public readonly string PlayerId;
        /// <summary>
        /// The position on the table which the player was seated.
        /// </summary>
        public readonly int Position;
        /// <summary>
        /// The name the player entered.
        /// </summary>
        public readonly string PlayerName;

        public PlayerSeatedEvent(string tableId, string playerId, int position, string playerName)
        {
            TableId = tableId;
            PlayerId = playerId;
            Position = position;
            PlayerName = playerName;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Player {0} joined Table {1} at Position {2}",
                PlayerId,
                TableId,
                Position);
        }
    }
}
