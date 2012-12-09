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

            ////TODO: Remove once we have accounts.//
            //_availablePlayerIds.Push(PLAYER_1_ID);
            //_availablePlayerIds.Push(PLAYER_2_ID);
            //_availablePlayerIds.Push(PLAYER_3_ID);
            //_availablePlayerIds.Push(PLAYER_4_ID);
            /////////////////////////////////////////
        }

        //***********************************************************************
        //THIS METHOD WILL BE DELETED AFTER I GET THE DRAG DROP FIGURED OUT
        //***********************************
        public void GetCards()
        {
            var cardArray = new string[] { 
                "Ah", "As", "Ac", "Ad",
                "Kh", "Ks", "Kc", "Kd",
                "Qh", "Qs", "Qc", "Qd",
                "Jh", "Js", "Jc", "Jd",
                "Th", "Ts", "Tc", "Td",
                "9h", "9s", "9c", "9d",
                "8h", "8s", "8c", "8d",
                "7h", "7s", "7c", "7d",
                "6h", "6s", "6c", "6d",
                "5h", "5s", "5c", "5d",
                "4h", "4s", "4c", "4d",
                "3h", "3s", "3c", "3d",
                "2h", "2s", "2c", "2d"
            };

            var cardsList = new List<string>();
            var dictChecker = new Dictionary<int, int>();
            var rnd = new Random(DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour);
            for (int i = 0; i < 13; i++)
            {
                var rndPosition = rnd.Next(0, 51);
                var checkerVal = 0;

                while (dictChecker.TryGetValue(rndPosition, out checkerVal))
                {
                    rndPosition = rnd.Next(0, 51);
                }

                cardsList.Add(cardArray[rndPosition]);
                dictChecker.Add(rndPosition, rndPosition);
            }

            //call client method
            Clients[Context.ConnectionId].dealCards(cardsList.ToArray());
        }
        //***********************************************************************
        //THIS METHOD WILL BE DELETED AFTER I GET THE DRAG DROP FIGURED OUT
        //***********************************

        #region Server To Client Methods

        public void DealToPlayer(string clientId, string[] cards)
        {
            //this is to an individual player
            Clients[clientId].dealToPlayer(cards);
        }

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

        //this is the global broadcast messages to console method
        //   very userful when debugging or wanting to monitor traffic
        public void BroadcastToConsole(string message)
        {
            //BroadcastMessage("Need tableId here", message);
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

        public void RearrangeHand(string tableId, string playerId, bool[] cards)
        {
            //every time a client rearranges his hand this method is called
            //no need to go to the domain here.  Just broadcast a message and the client can randomly move the cards
            //so that it appears the players is doing something.

            //Channel.BroadcastPlayerCardsRearranged();
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