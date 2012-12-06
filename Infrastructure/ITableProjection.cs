using System;
using System.Collections.Generic;
namespace Infrastructure
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

    public interface ITableProjection
    {
        void AddTable(TableDetails tableId);
        void AddPlayer(string tableId, int position, string name, string playerId);
        List<PlayerDetails> GetPlayerPositions(string tableId);
        void RemovePlayer(string tableId, int position);
        List<TableDetails> ListTables();
        TableState GetTableState(string tableId);
    }
}
