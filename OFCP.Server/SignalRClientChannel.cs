using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure;
using SignalR.Hubs;

namespace OFCP.Server
{
    internal class SignalRClientChannel : IClientChannel
    {
        private readonly IHubContext HubCtx;
        private readonly IPlayerConnectionMap _playerConnectionMap;

        public SignalRClientChannel(IHubContext hubCtx, IPlayerConnectionMap playerConnectionMap)
        {
            HubCtx = hubCtx;
            _playerConnectionMap = playerConnectionMap;
        }
        public void BroadcastPlayerSeated(string tableId, string name, int position)
        {
            HubCtx.Clients[tableId].playerTookSeat(name, position, "");
        }

        public void BroadcastPlayerLeft(string tableId, string name, int position)
        {
            HubCtx.Clients[tableId].playerLeftSeat(name, position);
        }

        public void BroadcastPlayerHandSet(string tableId, int position)
        {
            HubCtx.Clients[tableId].playerSetCards(position);
        }

        public void BroadcastPlayerCardsRearranged(string tableId, int playerPosition, bool[] cards)
        {
            HubCtx.Clients[tableId].playerCardsRearranged(playerPosition, cards);
        }

        public void BroadcastDeckShuffled(string tableId)
        {
            HubCtx.Clients[tableId].deckShuffled();
        }

        public void BroadcastGameStarted(string tableId, string gameId, DateTime timestamp)
        {
            HubCtx.Clients[tableId].gameStarted(gameId, timestamp);
        }

        public void BroadcastPlayerReady(string tableId, int position)
        {
            HubCtx.Clients[tableId].playerReadiedUp(position);
        }

        public void BroadcastMessage(string tableId, string message)
        {
            HubCtx.Clients[tableId].broadcastToConsole(message);
        }

        public void PlayerDealtCards(string playerId, string[] cards)
        {
            var connectionId = _playerConnectionMap.GetConnectionIdForPlayer(playerId);
            HubCtx.Clients[connectionId].dealCards(cards);
        }

        public void PlayerSeated(string playerId, int position)
        {
            var connectionId = _playerConnectionMap.GetConnectionIdForPlayer(playerId);
            HubCtx.Clients[connectionId].playerRegistered(playerId, position);
        }
    }
}