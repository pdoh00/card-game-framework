using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP.Events
{
    public class AllPlayersReadyEvent : Event
    {
        public readonly string TableId;
        public AllPlayersReadyEvent(string tableId, List<string> playerIds)
            : base()
        {
            TableId = tableId;
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("All players are ready at table {0}", TableId);
        }
    }
}
