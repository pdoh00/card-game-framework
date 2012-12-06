using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public struct TableDetails
    {
        public readonly string TableId;
        public readonly string GameType;

        public TableDetails(string tableId, string gameType)
        {
            TableId = tableId;
            GameType = gameType;
        }
    }

    public struct TableState
    {
        public readonly string TableId;
        public readonly string GameType;
        public readonly PlayerDetails[] PlayerDetails;

        public TableState(TableDetails tblDetails, SortedDictionary<int, PlayerDetails> playerDetails)
        {
            TableId = tblDetails.TableId;
            GameType = tblDetails.GameType;
            PlayerDetails = new PlayerDetails[4];
            foreach (var p in playerDetails)
            {
                PlayerDetails[p.Key] = p.Value;
            }
        }
    }

    public class MemoryTableProjection : ITableProjection
    {
        //SortedDictionary<int, string> _players = new SortedDictionary<int, string>();
        Dictionary<string, SortedDictionary<int, PlayerDetails>> _tables = new Dictionary<string, SortedDictionary<int, PlayerDetails>>();
        Dictionary<string, TableDetails> _tableDetails = new Dictionary<string, TableDetails>();
        private object _lock = new object();

        public void AddTable(TableDetails table)
        {
            _tables.Add(table.TableId, new SortedDictionary<int, PlayerDetails>());
            _tableDetails.Add(table.TableId, table);
        }

        public List<TableDetails> ListTables()
        {
            return _tableDetails.Values.ToList();
        }

        /// <summary>
        /// Updates the player state of the table.  Send an empty
        /// string to remove a player.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="name"></param>
        public void AddPlayer(string tableId, int position, string name, string playerdId)
        {
            lock (_lock)
            {
                SortedDictionary<int, PlayerDetails> players;
                if (_tables.TryGetValue(tableId, out players))
                    players[position] = new PlayerDetails(playerdId, name);
                else
                    throw new InvalidOperationException(String.Format("Cannot add a player to non-existant table {0}", tableId));
            }
        }

        public void RemovePlayer(string tableId, int position)
        {
            lock (_lock)
            {
                SortedDictionary<int, PlayerDetails> players;
                if (_tables.TryGetValue(tableId, out players))
                    players.Remove(position);
                else
                    throw new InvalidOperationException(String.Format("Cannot remove a player from non-existant table {0}", tableId));
            }
        }

        public List<PlayerDetails> GetPlayerPositions(string tableId)
        {
            SortedDictionary<int, PlayerDetails> players;
            if (_tables.TryGetValue(tableId, out players))
            {
                return players.Values.ToList();
            }
            else
                throw new InvalidOperationException(String.Format("Cannot get player data from non-existant table {0}", tableId));
        }

        public TableState GetTableState(string tableId)
        {
            //TODO: Make this index code safe if we keep the in mem projection.
            var tableDetails = _tableDetails[tableId];
            var playerDetails = _tables[tableId];

            return new TableState(tableDetails, _tables[tableId]);
        }

    }
}
