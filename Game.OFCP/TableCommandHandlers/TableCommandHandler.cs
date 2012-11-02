using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Games;
using Game.OFCP.TableCommands;
using Infrastructure;

namespace Game.OFCP.TableCommandHandlers
{
    public class TableCommandHandler :
        ICommandHandler<CreateNewTableCommand>,
        ICommandHandler<SeatPlayerCommand>,
        ICommandHandler<RemovePlayerCommand>,
        ICommandHandler<SetPlayerReadyCommand>
    {
        private readonly IRepository<Table> TableRepository;
        private object _padLock = new object();

        public TableCommandHandler(IRepository<Table> tableRepository)
        {
            TableRepository = tableRepository;
        }

        public void Handle(SeatPlayerCommand command)
        {
            int retryAttempt=0;
            try
            {
                var table = TableRepository.GetById(command.TableId);
                table.SeatPlayer(command.PlayerId, command.PlayerName);
                TableRepository.Save(table, table.Version);
            }
            catch (ConcurrencyException ex)
            {
                retryAttempt++;
                Console.WriteLine(ex + System.Threading.Thread.CurrentThread.ToString());
                if(retryAttempt < 10)
                    Handle(command); //retry
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + System.Threading.Thread.CurrentThread.ToString());
            }
        }

        public void Handle(RemovePlayerCommand command)
        {
            var table = TableRepository.GetById(command.TableId);
            table.RemovePlayer(command.PlayerId);
            TableRepository.Save(table, table.Version);
        }

        public void Handle(CreateNewTableCommand command)
        {
            const int MAX_PLAYERS_FOR_OFCP = 4;

            //add factory pattern
            switch (command.TableType)
            {
                case OFCP_Game.OFCP_GAME_TYPE:
                    {
                        var table = new Table(Guid.NewGuid().ToString(), MAX_PLAYERS_FOR_OFCP, OFCP_Game.OFCP_GAME_TYPE);
                        TableRepository.Save(table, -1);
                        break;
                    }
                default:
                    break;
            }

        }

        public void Handle(SetPlayerReadyCommand command)
        {
            var table = TableRepository.GetById(command.TableId);
            table.PlayerReady(command.PlayerId);
            TableRepository.Save(table, table.Version);
        }
    }
}
