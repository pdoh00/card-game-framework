using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Events;
using Infrastructure;

namespace Game.OFCP.Games
{
    public sealed class OFCP_Game : Game<OFCP_Game, OFCP_Player>
    {
        public const string OFCP_GAME_TYPE = "Open Faced Chinese Poker";

        private const int MAX_PLAYERS = 4;
        private readonly StandardDeck _deck;
        private bool _isGameInProgress;
        private int _round;

        private Dictionary<int, Player> _seatMap = new Dictionary<int, Player>(4);

        public OFCP_Game()
        {
            // used to create in repository
        }
        public OFCP_Game(string tableId, string gameId, IShuffler shuffler)
            : base(tableId, gameId, OFCP_GAME_TYPE)
        {
            _deck = new StandardDeck(shuffler, this.Id); //TODO: inject the deck
            _players = new List<OFCP_Player>(MAX_PLAYERS);
        }

        public override void Start()
        {
            if (_players.Count < 2)
                throw new InvalidOperationException("Cannot start a new game with less than 2 players");
            if(_isGameInProgress)
                throw new InvalidOperationException("Cannot start a game already in progress");

            
            Apply(new GameStartedEvent(Id, DateTime.Now.ToUniversalTime()));
        }

        public void StartNextRound()
        {
            //if (!_isGameInProgress)
            //    throw new InvalidOperationException("Cannot start a new round when the game is not in progress");

            //_round++;
            //Apply(new NewRoundStarted(Id, _round, DateTime.Now.ToUniversalTime()));
            //foreach (var player in _players)
            //{
            //    player.AcceptCards(_deck.Deal(13), _round);
            //}
        }

        public void AddPlayer(string playerId, string playerName, int position)
        {
            if (_players.Count >= MAX_PLAYERS)
                throw new InvalidOperationException("Maximum 4 players");

            var player = FindPlayerById(playerId);
            if (player != null)
                throw new InvalidOperationException(String.Format("Player {0} already is seated at this game", player.Id));

            Apply(new PlayerJoinedGame(playerId, Id, DateTime.Now.ToUniversalTime(), playerName, position));
        }

        public void RemovePlayer(string playerId)
        {
            var player = FindPlayerById(playerId);
            if (player == null)
                throw new InvalidOperationException("Player {0} is not part of game and cannot be removed.");

            Apply(new PlayerQuitGame(player.Id, Id, DateTime.Now.ToUniversalTime()));
        }

        public void SetHand(string playerId, string[] h)
        {
            var player = FindPlayerById(playerId);
            if (player == null)
                throw new InvalidOperationException("Player {0} is not part of game and cannot be removed.");

            //if all players set
            var btmHand = new string[5];
            Array.Copy(h, 0, btmHand, 0, 5);

            var middleHand = new string[5];
            Array.Copy(h, 5, middleHand, 0, 5);

            var topHand = new string[3];
            Array.Copy(h, 10, topHand, 0, 3);

            player.SetBottomHand(btmHand);
            player.SetMiddleHand(middleHand);
            player.SetTopHand(topHand);

            //calc score

            //roundCompleteevent
        }

        protected override void OnPlayerQuitGame(PlayerQuitGame @event)
        {
            _players.RemoveAll(p => p.Id == @event.PlayerId);
        }

        protected override void OnPlayerJoined(PlayerJoinedGame @event)
        {
            var player = new OFCP_Player(@event.PlayerId, @event.PlayerName);
            _players.Add(player);
        }

        protected override void OnGameStarted(GameStartedEvent obj)
        {
            _isGameInProgress = true;
            _deck.Shuffle();

            //temp here
            
            foreach (var player in _players)
            {
                var cards = _deck.Deal(13);
                player.AcceptCards(cards, _round);
                Apply(new PlayerDealtCards(player.Id, Id, _round, cards));
            }
        }

        private OFCP_Player FindPlayerById(string playerId)
        {
            var player = _players.FirstOrDefault(p => p.Id == playerId);

            return player;
        }


        
    }
}
