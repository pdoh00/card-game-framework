using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP
{
    public class MemoryPlayerProjection : IPlayerProjection
    {
        private readonly Dictionary<string, string> _players = new Dictionary<string, string>();

        public void AddPlayer(string id, string name)
        {
            _players[id] = name;
        }

        public PlayerDetails GetPlayerById(string playerId)
        {
            return new PlayerDetails(playerId, _players[playerId]);
        }


        public void RemovePlayer(string id)
        {
            _players.Remove(id);
        }
    }
}
