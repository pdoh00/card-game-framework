using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.OFCP.Events;
using Infrastructure;

namespace Game.OFCP.EventHandlers
{
    public class GameEventHandler : 
        IEventHandler<GameCreatedEvent>,
        IEventHandler<GameStartedEvent>,
        IEventHandler<DeckShuffledEvent>,
        IEventHandler<PlayerDealtCards>,
        IEventHandler<PlayerJoinedGame>,
        IEventHandler<PlayerQuitGame>
    {
        private IClientChannel _clientChannel;

        public GameEventHandler(IClientChannel clientChannel)
        {
            _clientChannel = clientChannel;
        }

        //might not need to broadcast this.  The game starting is what matters to the user.
        //game creation is only important to the domain. -PC
        public void Handle(GameCreatedEvent @event)
        {
            //_clientChannel.

        }

        public void Handle(GameStartedEvent @event)
        {
            _clientChannel.BroadcastGameStarted(@event.TableId, @event.GameId, @event.GameStartTime);
        }

        public void Handle(DeckShuffledEvent @event)
        {
            _clientChannel.BroadcastDeckShuffled("Need table id here");
        }

        public void Handle(PlayerDealtCards @event)
        {
            _clientChannel.PlayerDealtCards(@event.PlayerId, @event.Cards.Select(c => c.Text).ToArray());
        }

        public void Handle(PlayerJoinedGame @event)
        {
            _clientChannel.BroadcastPlayerReady("Need table id here", @event.Position);
        }

        public void Handle(PlayerQuitGame @event)
        {
            //_broadcast(@event.ToString());
        }
    }
}
