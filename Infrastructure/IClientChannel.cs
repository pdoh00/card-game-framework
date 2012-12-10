using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IClientChannel
    {
        void BroadcastPlayerSeated(string tableId, string name, int position);
        void BroadcastPlayerLeft(string tableId, string name, int position);
        void BroadcastPlayerHandSet(string tableId, int position);
        /// <summary>
        /// Notification of a card slot either being filled or removed.
        /// </summary>
        /// <param name="cards">true if card is in slot false if slot is empty</param>
        void BroadcastPlayerCardsRearranged(string tableId, int playerPosition, List<CardPositionState> cards);
        void BroadcastDeckShuffled(string tableId);
        void BroadcastGameStarted(string tableId, string gameId, DateTime timestamp);
        void BroadcastPlayerReady(string tableId, int position);

        void PlayerDealtCards(string clientId, string[] cards);
        void PlayerSeated(string clientId, int position);
        void TableInitialized(string connectionId);
        void SetTableState(string connectionId, TableState tableState);
    }
}
