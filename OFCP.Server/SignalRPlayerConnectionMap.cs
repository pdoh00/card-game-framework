using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OFCP.Server
{
    public class SignalRPlayerConnectionMap : IPlayerConnectionMap
    {
        private readonly ConcurrentDictionary<string, string> _playerIdMap = new ConcurrentDictionary<string, string>();

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
            string removedPlayerId;
            _playerIdMap.TryRemove(playerId, out removedPlayerId);
        }
    }
}