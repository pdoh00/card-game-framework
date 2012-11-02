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
                //var playerPositions = new List<PlayerDetails>(players.Count);
                //for (int i = 0; i < players.Count; i++)
                //{
                //    playerPositions.Add(players[i + 1]); // JB - had to add a +1 because position isn't keyed on a zero indexed array
                //}

                return players.Values.ToList();
            }
            else
                throw new InvalidOperationException(String.Format("Cannot get player data from non-existant table {0}", tableId));

            
        }

    }
}
