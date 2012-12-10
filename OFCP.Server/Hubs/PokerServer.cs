using System;
using System.Collections.Generic;
using System.Linq;
using Game.OFCP;
using Game.OFCP.EventHandlers;
using Game.OFCP.Games;
using Infrastructure;
using SignalR.Hubs;
using System.Threading.Tasks;
using Game.OFCP.TableCommands;
using Game.OFCP.TableCommandHandlers;
using Game.OFCP.GameCommandHandlers;
using System.Threading;
using SignalR;
using Game.OFCP.GameCommands;

namespace OFCP.Server.Hubs
{
    public class PokerServer : Hub, IDisconnect
    {
        private readonly ICommandBus _cmdBus;
        private readonly ITableProjection _tables;
        private readonly IPlayerConnectionMap _playerConnectionMap;
        private readonly IClientChannel _clientChannel;

        public PokerServer(ICommandBus cmdBus, ITableProjection tableProjection, IPlayerConnectionMap playerConnectionMap, IClientChannel clientChannel)
        {
            _cmdBus = cmdBus;
            _tables = tableProjection;
            _playerConnectionMap = playerConnectionMap;
            _clientChannel = clientChannel;
        }

        #region Server To Client Methods

        public void GameAboutToStart()
        {
            //this is broadcast to the group (table)

            //this is the game about to start timer.  Set to a configurable amount like 30 seconds
            //  and ticks down on all the clients

        }

        //send down data to represent who won/scores etc...
        public void RoundOver()
        {

            //this is broadcast to the group (table)
            //Clients[tableId].roundOver();
        }

        //send down summary of game and who won the game along with everyone's scores
        public void GameOver()
        {
            //this is broadcast to the group (table)
            //Clients[tableId].gameOver();
        }

        #endregion

        #region Client To Server Methods

        public void InitializeTable(string tableId)
        {
            //register the tableId as a group
            Groups.Add(Context.ConnectionId, tableId);

            //alert the client that the table is initialized
            _clientChannel.TableInitialized(Context.ConnectionId);
        }

        public void TakeSeat(string tableId, string playerId, string playerName)
        {
            _playerConnectionMap.UpdateConnectionIdForPlayer(playerId, Context.ConnectionId);
            _cmdBus.Send(new SeatPlayerCommand(tableId, playerId, playerName));   
        }

        public void Reconnect(string playerId)
        {
            _playerConnectionMap.UpdateConnectionIdForPlayer(playerId, Context.ConnectionId);
        }

        public void LeaveGame(string tableId, string playerId)
        {
            _playerConnectionMap.RemovePlayer(playerId);
            _cmdBus.Send(new RemovePlayerCommand(tableId, playerId));
            Groups.Remove(Context.ConnectionId, tableId);
        }

        public void GetTableState(string tableId)
        {
            var tableDetails = _tables.ListTables().Select(tbl => tbl.TableId == tableId);
            var playerDetails = _tables.GetPlayerPositions(tableId);
            var tableState = _tables.GetTableState(tableId);
            _clientChannel.SetTableState(Context.ConnectionId, tableState);
        }

        /// <summary>
        /// Sets all of the players hands for the current game.
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <param name="cards">Array of size 13 with the following format
        /// 
        /// bbbbbbmmmmmttt
        /// b:bottom hand card
        /// m:middle hand card
        /// t:top hand card
        /// 
        /// </param>
        public void SetHand(string tableId, string gameId, string playerId, string[] cards)
        {
            var btmHand = new string[5];
            Array.Copy(cards, 0, btmHand, 0, 5);

            var middleHand = new string[5];
            Array.Copy(cards, 5, middleHand, 0, 5);

            var topHand = new string[3];
            Array.Copy(cards, 10, topHand, 0, 3);

            _cmdBus.Send(new CommitPlayerHandsCommand(tableId, gameId, playerId, btmHand, middleHand, topHand));
        }

        public void PlayerReady(string tableId, string playerId)
        {
            _cmdBus.Send(new SetPlayerReadyCommand(tableId, playerId));
        }

        public void RearrangeHand(string tableId, int playerPosition, List<CardPositionState> cards)
        {
            _clientChannel.BroadcastPlayerCardsRearranged(tableId, playerPosition, cards);
        }

        #endregion

        #region IDisconnect

        public Task Disconnect()
        {
            //return Clients[Table.Id].leave(Context.ConnectionId);
            return Task.Factory.StartNew(() => { });
        }

        #endregion

    }
}