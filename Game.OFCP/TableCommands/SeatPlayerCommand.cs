using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.TableCommands
{
    public class SeatPlayerCommand :ICommand
    {
        public readonly string PlayerId;
        public readonly string PlayerName;
        public readonly string TableId;

        public SeatPlayerCommand(string tableId, string playerId, string playerName)
        {
            TableId = tableId;
            PlayerId = playerId;
            PlayerName = playerName;
        }
    }
}
