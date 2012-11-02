using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure;
using SignalR.Hubs;

namespace OFCP.Server
{
    public class ClientChannel : IClientChannel
    {
        private readonly IHubContext HubCtx;
        public ClientChannel(IHubContext hubCtx)
        {
            HubCtx = hubCtx;
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

        public void PlayerDealtCards(string clientId, string[] cards)
        {
            HubCtx.Clients[clientId].dealCards(cards);
        }

        public void PlayerSeated(string clientId, int position)
        {
            HubCtx.Clients[clientId].playerRegistered(clientId, position);
        }
    }
}