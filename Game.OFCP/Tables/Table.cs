using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Events;
using Infrastructure;

namespace Game.OFCP
{
    public class Table : AggregateRoot
    {
        private struct PlayerDetails
        {
            public readonly string Id;
            public readonly string Name;
            public readonly int Position;

            public PlayerDetails(string id, string name, int position)
            {
                Id = id;
                Name = name;
                Position = position;
            }
        }

        private List<PlayerDetails> _playerSeatMap;
        private List<PlayerDetails> _playersReady;
        private string _id;
        private string _gameTypeId;
        private int _numberOfSeats;

        public Table()
        {
            // used to create in repository
            this.Handles<TableCreatedEvent>(OnTableCreated);
            this.Handles<PlayerSeatedEvent>(OnPlayerSeated);
            this.Handles<PlayerLeftTable>(OnPlayerRemoved);
            this.Handles<PlayerReadyEvent>(OnPlayerReady);
            this.Handles<AllPlayersReadyEvent>(OnAllPlayersReady);
        }

        public Table(string tableId, int maxPlayers, string gameType)
            : this()
        {
            Apply(new TableCreatedEvent(tableId, DateTime.Now.ToUniversalTime(), maxPlayers, gameType));
        }

        public override string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Removes a player from the table. If a player is in a game the player will be removed
        /// from the game.
        /// </summary>
        /// <param name="playerId"></param>
        public void RemovePlayer(string playerId)
        {
            var player = _playerSeatMap.Find(p => p.Id == playerId);
            if (player.Id == null)
                throw new InvalidOperationException("Player {0} is not part of game and cannot be removed.");

            Apply(new PlayerLeftTable(Id, player.Id, player.Name, player.Position));
        }

        /// <summary>
        /// Protected so the derived class user doesn't need to create the PlayerType player.  Do it for them
        /// in the derived class and then call this method.
        /// </summary>
        /// <param name="player"></param>
        public void SeatPlayer(string playerId, string playerName)
        {
            if (_playerSeatMap.Count >= _numberOfSeats)
                throw new InvalidOperationException("Maximum 4 players");

            if (_playerSeatMap.Find(p => p.Id == playerId).Id != null)
                throw new InvalidOperationException(String.Format("Player {0} already is seated at this game", playerId));

            Apply(new PlayerSeatedEvent(Id, playerId, GetNextAvailableSeat(), playerName));
        }

        public void PlayerReady(string playerId)
        {
            var playerDetails = _playerSeatMap.Find(p => p.Id == playerId);

            Apply(new PlayerReadyEvent(this.Id, playerDetails.Id, playerDetails.Name, playerDetails.Position));
        }

        private void OnPlayerReady(PlayerReadyEvent @event)
        {
            var player = _playersReady.Find(p => p.Id == @event.PlayerId);
            if (player.Id == null)
                _playersReady.Add(new PlayerDetails(@event.PlayerId, @event.PlayerName, @event.Position));

            //when all players ready up
            if (AllPlayersReady())
                Apply(new AllPlayersReadyEvent(Id, _playerSeatMap.Select(p=>p.Id).ToList()));
        }

        private void OnTableCreated(TableCreatedEvent @event)
        {
            _id = @event.TableId;
            _numberOfSeats = @event.PlayerCapacity;
            _gameTypeId = @event.GameType;
            _playerSeatMap = new List<PlayerDetails>(_numberOfSeats);
            _playersReady = new List<PlayerDetails>(_numberOfSeats);
        }

        private void OnPlayerRemoved(PlayerLeftTable @event)
        {
            _playerSeatMap.RemoveAll(p => p.Id == @event.PlayerId);
        }

        private void OnPlayerSeated(PlayerSeatedEvent @event)
        {
            _playerSeatMap.Add(new PlayerDetails(@event.PlayerId, @event.PlayerName, @event.Position));
        }

        private void OnAllPlayersReady(AllPlayersReadyEvent obj)
        {
            //might not need to handle this here...not sure.
        }

        private int GetNextAvailableSeat()
        {
            return _playerSeatMap.Count + 1;
        }

        private bool AllPlayersReady()
        {
            return _playersReady.Count == _numberOfSeats;
        }

    }
}
