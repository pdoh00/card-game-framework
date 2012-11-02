using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.GameCommands
{
    public class StartNewGameCommand : ICommand
    {
        public readonly string TableId;
        public readonly string GameTypeId;

        public StartNewGameCommand(string tableId, string gameTypeId)
        {
            TableId = tableId;
            GameTypeId = gameTypeId;
        }
        public override string ToString()
        {
            return String.Format("Starting new game on Table {0}, GameType {1}", TableId, GameTypeId);
        }
    }
}
