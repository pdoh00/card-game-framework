using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP
{
    public class OFCPTable : Table<OFCP_Player>
    {
        private const int MAX_PLAYERS_FOR_OFCP = 4;

        public OFCPTable(string tableId, IEventBus bus)
            : base(tableId, bus, MAX_PLAYERS_FOR_OFCP, GameType.ChinesePoker)
        {

        }

        public void SeatPlayer(string playerId, string playerName)
        {
            SeatPlayer(new OFCP_Player(playerId, _bus, playerName));
        }

    }
}
