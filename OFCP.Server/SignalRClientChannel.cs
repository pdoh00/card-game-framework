using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure;
using OFCP.Server.Hubs;
using SignalR;
using SignalR.Hubs;

namespace OFCP.Server
{
    internal class SignalRClientChannel : IClientChannel
    {
        private readonly IPlayerConnectionMap _playerConnectionMap;

        public SignalRClientChannel(IPlayerConnectionMap playerConnectionMap)
        {
            _playerConnectionMap = playerConnectionMap;
        }
        public void BroadcastPlayerSeated(string tableId, string name, int position)
        {
            GetPokerServerHubContext().Clients[tableId].playerTookSeat(name, position, "");
        }

        public void BroadcastPlayerLeft(string tableId, string name, int position)
        {
            GetPokerServerHubContext().Clients[tableId].playerLeftSeat(name, position);
        }

        public void BroadcastPlayerHandSet(string tableId, int position)
        {
            GetPokerServerHubContext().Clients[tableId].playerSetCards(position);
        }

        public void BroadcastPlayerCardsRearranged(string tableId, int playerPosition, List<CardPositionState> cards)
        {
            GetPokerServerHubContext().Clients[tableId].playerCardsRearranged(playerPosition, cards);
        }

        public void BroadcastDeckShuffled(string tableId)
        {
            GetPokerServerHubContext().Clients[tableId].deckShuffled();
        }

        public void BroadcastGameStarted(string tableId, string gameId, DateTime timestamp)
        {
            GetPokerServerHubContext().Clients[tableId].gameStarted(gameId, timestamp);
        }

        public void BroadcastPlayerReady(string tableId, int position)
        {
            GetPokerServerHubContext().Clients[tableId].playerReadiedUp(position);
        }

        public void BroadcastMessage(string tableId, string message)
        {
            GetPokerServerHubContext().Clients[tableId].broadcastToConsole(message);
        }

        public void PlayerDealtCards(string playerId, string[] cards)
        {
            var connectionId = _playerConnectionMap.GetConnectionIdForPlayer(playerId);
            GetPokerServerHubContext().Clients[connectionId].dealCards(cards);
        }

        public void PlayerSeated(string playerId, int position)
        {
            var connectionId = _playerConnectionMap.GetConnectionIdForPlayer(playerId);
            GetPokerServerHubContext().Clients[connectionId].playerRegistered(playerId, position);
        }

        public void TableInitialized(string connectionId)
        {
            GetPokerServerHubContext().Clients[connectionId].tableInitialized();
        }

        public void SetTableState(string connectionId, TableState tableState)
        {
            GetPokerServerHubContext().Clients[connectionId].setTableState(tableState);
        }

        private static IHubContext GetPokerServerHubContext()
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<PokerServer>();
            return ctx;
        }
    }
}