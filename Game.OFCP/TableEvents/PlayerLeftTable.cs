using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public class PlayerLeftTable : Event
    {
        public readonly string TableId;
        public readonly string PlayerId;
        /// <summary>
        /// The name the player entered.
        /// </summary>
        public readonly string PlayerName;

        public readonly int Position;

        public PlayerLeftTable(string tableId, string playerId, string playerName, int position)
        {
            TableId = tableId;
            PlayerId = playerId;
            PlayerName = playerName;
            Position = position;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("Player {0}-{1} left Table {1} Position {2}",
                PlayerName,
                PlayerId,
                TableId,
                Position);
        }
    }
}
