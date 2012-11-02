using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.GameCommands;
using Game.OFCP.Games;
using Infrastructure;

namespace Game.OFCP.GameCommandHandlers
{
    public class GameCommandHandler :
        ICommandHandler<StartNewGameCommand>
    {
        private KnuthShuffler _shuffler = new KnuthShuffler();
        private readonly IRepository<OFCP_Game> GameRepository;
        private readonly ITableProjection TableStore;
        private readonly IPlayerProjection PlayerStore;

        public GameCommandHandler(ITableProjection tableStore, IPlayerProjection playerStore, IRepository<OFCP_Game> gameRepository)
        {
            GameRepository = gameRepository;
            TableStore = tableStore;
            PlayerStore = playerStore;
        }

        public void Handle(StartNewGameCommand command)
        {
            var game = new OFCP_Game(command.TableId, Guid.NewGuid().ToString(), _shuffler);
            var players = TableStore.GetPlayerPositions(command.TableId).ToList();

            foreach (var player in players)
            {
                game.AddPlayer(player.PlayerId, player.PlayerName, players.IndexOf(player));
            }

            game.Start();
            GameRepository.Save(game, game.Version);
        }
    }
}
