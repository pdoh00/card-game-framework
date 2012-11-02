using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.TableCommands
{
    public class RemovePlayerCommand : ICommand
    {
        public readonly string PlayerId;
        public readonly string TableId;//TODO: once we support multi table this will need table too

        public RemovePlayerCommand(string tableId, string playerId)
        {
            PlayerId = playerId;
            TableId = tableId;
        }
    }
}
