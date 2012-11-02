using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP
{
    public struct PlayerDetails
    {
        public readonly string PlayerId;
        public readonly string PlayerName;
        
        public PlayerDetails(string playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
        }
    }
    public interface IPlayerProjection
    {
        void AddPlayer(string id, string name);
        void RemovePlayer(string id);
        PlayerDetails GetPlayerById(string playerId);
        
    }
}
