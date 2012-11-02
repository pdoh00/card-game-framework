using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Events;
using Infrastructure;

namespace Game.OFCP
{
    public enum GameStatus
    {
        InProgress,
        Complete
    }

    public abstract class Game : AggregateRoot
    {
        private string _id;
        protected string TableId { get; private set; }
        protected string GameTypeId { get; private set; }

        public Game()
        {
            // used to create in repository
            this.Handles<GameCreatedEvent>(OnGameCreated);
            this.Handles<GameStartedEvent>(OnGameStarted);
        }

        public Game(string tableId, string gameId, string gameTypeId)
            : this()
        {
            Apply(new GameCreatedEvent(tableId, gameId, gameTypeId, DateTime.Now.ToUniversalTime()));
        }

        private void OnGameCreated(GameCreatedEvent @event)
        {
            _id = @event.GameId;
            TableId = @event.TableId;
            GameTypeId = @event.GameTypeId;
        }

        public abstract void Start();
        protected abstract void OnGameStarted(GameStartedEvent @event);

        public override string Id
        {
            get { return _id; }
        }
    }

    //This might go away.  Not sure if we need this generic game type.
    public abstract class Game<GameType, PlayerType> : Game
        where PlayerType : Player
        where GameType : Game
    {
        protected List<PlayerType> _players;

        //TODO: don't really like that i have to add the handlers to both ctor, but
        //I don't see a way to chain them.

        public Game()
        {
            // used to create in repository
            this.Handles<PlayerJoinedGame>(OnPlayerJoined);
            this.Handles<PlayerQuitGame>(OnPlayerQuitGame);
        }

        public Game(string tableId, string gameId, string gameTypeId)
            : base(tableId, gameId, gameTypeId)
        {
            this.Handles<PlayerJoinedGame>(OnPlayerJoined);
            this.Handles<PlayerQuitGame>(OnPlayerQuitGame);
            this.Handles<PlayerDealtCards>(OnPlayerDealtCards);
        }

        private void OnPlayerDealtCards(PlayerDealtCards obj)
        {
            //not sure if anything has to be done here.
        }

        protected abstract void OnPlayerQuitGame(PlayerQuitGame @event);
        protected abstract void OnPlayerJoined(PlayerJoinedGame @event);
    }


}
