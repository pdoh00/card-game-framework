using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Games;
using Infrastructure;

namespace Game.OFCP
{
    public enum GameType
    {
        ChinesePoker
    }

    public sealed class GameFactory
    {
        public static Game CreateGame(string tableId, GameType gameType, IShuffler shuffler)
        {
            return new OFCP_Game(tableId, Guid.NewGuid().ToString(), shuffler);
        }
    }
}
