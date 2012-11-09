using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OFCP.Server
{
    public class SignalRPlayerConnectionMap : IPlayerConnectionMap
    {
        private readonly Dictionary<string, string> _playerIdMap = new Dictionary<string, string>();

        public string GetConnectionIdForPlayer(string playerId)
        {
            return _playerIdMap[playerId];
        }

        public void UpdateConnectionIdForPlayer(string playerId, string connectionId)
        {
            _playerIdMap[playerId] = connectionId;
        }

        public void RemovePlayer(string playerId)
        {
            _playerIdMap.Remove(playerId);
        }
    }
}