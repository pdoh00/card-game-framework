using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFCP.Server
{
    public interface IPlayerConnectionMap
    {
        string GetConnectionIdForPlayer(string playerId);
        void UpdateConnectionIdForPlayer(string playerId, string connectionId);
        void RemovePlayer(string playerId);
    }
}
