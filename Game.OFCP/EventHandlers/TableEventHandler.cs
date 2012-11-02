﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Events;
using Game.OFCP.GameCommands;
using Infrastructure;

namespace Game.OFCP.EventHandlers
{
    public class TableEventHandler :
        IEventHandler<TableCreatedEvent>,
        IEventHandler<PlayerSeatedEvent>,
        IEventHandler<PlayerLeftTable>,
        IEventHandler<PlayerReadyEvent>,
        IEventHandler<AllPlayersReadyEvent>
    {
        private readonly IClientChannel _clientChannel;
        private readonly ITableProjection _tableProjection;
        private readonly IPlayerProjection _playerStore;
        private readonly ICommandBus _commandBus;
        public TableEventHandler(IClientChannel clientChannel, ITableProjection tableProjection, ICommandBus commandBus, IPlayerProjection playerStore)
        {
            _clientChannel = clientChannel;
            _tableProjection = tableProjection;
            _commandBus = commandBus;
            _playerStore = playerStore;
        }

        public void Handle(PlayerSeatedEvent @event)
        {

            _clientChannel.BroadcastPlayerSeated(@event.TableId, @event.PlayerName, @event.Position);
            _clientChannel.PlayerSeated(@event.PlayerId, @event.Position);
            _tableProjection.AddPlayer(@event.TableId, @event.Position, @event.PlayerName, @event.PlayerId);
            _playerStore.AddPlayer(@event.PlayerId, @event.PlayerName);
        }

        public void Handle(PlayerLeftTable @event)
        {
            _clientChannel.BroadcastPlayerLeft(@event.TableId, @event.PlayerName, @event.Position);
            _tableProjection.RemovePlayer(@event.TableId, @event.Position);
            _playerStore.RemovePlayer(@event.PlayerId);
        }

        public void Handle(AllPlayersReadyEvent @event)
        {
            //send a start game command with the table id.
            var table = _tableProjection.ListTables().Single(t => t.TableId == @event.TableId);
            _commandBus.Send(new StartNewGameCommand(table.TableId, table.GameType));

            Console.WriteLine(@event);
        }

        public void Handle(TableCreatedEvent @event)
        {
            _tableProjection.AddTable(new TableDetails(@event.TableId, @event.GameType));
            Console.WriteLine(@event);
        }

        public void Handle(PlayerReadyEvent @event)
        {
            Console.WriteLine(@event);
        }
    }
}
