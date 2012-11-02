using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.TableCommands
{
    public class SetPlayerReadyCommand : ICommand
    {
        public readonly string TableId;
        public readonly string PlayerId;

        public SetPlayerReadyCommand(string tableId, string playerId)
        {
            TableId = tableId;
            PlayerId = playerId;
        }
    }
}
