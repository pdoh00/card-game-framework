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

namespace OFCP.Server.Hubs
{
    public class PokerServer : Hub, IDisconnect
    {
        private readonly ICommandBus _cmdBus;
        private readonly ITableProjection _tables;
        static Dictionary<string, string> _playerIdMap = new Dictionary<string, string>();

        public PokerServer()
        {
            Console.WriteLine("Im in");
        }

        public PokerServer(ICommandBus cmdBus, ITableProjection tableProjection)
        {
            _cmdBus = cmdBus;
            _tables = tableProjection;
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
        
        //throwaway method to enable a single table site for now.  Someday the client
        //will have the table id from a table selection in the lobby.
        public void GetTableId()
        {
            while (_tables.ListTables().Count == 0)
            {
                Thread.Sleep(200);
            }
            Clients[Context.ConnectionId].setTableId(_tables.ListTables()[0].TableId);
        }

        public void TakeSeat(string tableId, string playerName)
        {
            var playerId = Guid.NewGuid().ToString();
            _cmdBus.Send(new SeatPlayerCommand(tableId, playerId, playerName));
            _playerIdMap[playerId] = Context.ConnectionId;
            //register player with group the group is the table id
            Groups.Add(Context.ConnectionId, tableId);
        }

        public void Reconnect(string playerId)
        {
            var connectionId = string.Empty;
            if (_playerIdMap.TryGetValue(playerId, out connectionId))
            {
                _playerIdMap[playerId] = Context.ConnectionId;
            }
            else
            {
                throw new InvalidOperationException("Unable to reconnect.  Player doesn't exist");
            }
        }

        public void LeaveGame(string tableId, string playerId)
        {
            _cmdBus.Send(new RemovePlayerCommand(tableId, playerId));
            if (_playerIdMap.ContainsKey(playerId))
                _playerIdMap.Remove(playerId);
            
            Groups.Remove(Context.ConnectionId, tableId);
        }

        public void GetPlayerPositionsAtTable(string tableId)
        {
            Clients[tableId].setPlayerState(_tables.GetPlayerPositions(tableId).Select(x => x.PlayerName));
        }

        public void SetHand(string tableId, string playerId, string[] cards)
        {
            //when a client wants to set his hand he calls this method
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

            //Channel.BroadcastPlayerCardsRearranged(
        }

        #endregion

        #region IDisconnect


        public Task Disconnect()
        {
            //return Clients[Table.Id].leave(Context.ConnectionId);
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        //#region IClientChannel

        //public void BroadcastPlayerSeated(string tableId, string name, int position)
        //{
        //    Clients[tableId].playerTookSeat(name, position, "");
        //}

        //public void BroadcastPlayerLeft(string tableId, string name, int position)
        //{
        //    Clients[tableId].playerLeftSeat(name, position);
        //}

        //public void BroadcastPlayerHandSet(string tableId, int position)
        //{
        //    Clients[tableId].playerSetCards(position);
        //}

        //public void BroadcastPlayerCardsRearranged(string tableId, int playerPosition, bool[] cards)
        //{
        //    Clients[tableId].playerCardsRearranged(playerPosition, cards);
        //}

        //public void BroadcastDeckShuffled(string tableId)
        //{
        //    Clients[tableId].deckShuffled();
        //}

        //public void BroadcastGameStarted(string tableId, string gameId, DateTime timestamp)
        //{
        //    Clients[tableId].gameStarted(gameId, timestamp);
        //}

        //public void BroadcastPlayerReady(string tableId, int position)
        //{
        //    Clients[tableId].playerReadiedUp(position);
        //}

        //public void BroadcastMessage(string tableId, string message)
        //{
        //    Clients[tableId].broadcastToConsole(message);
        //}

        //public void PlayerDealtCards(string clientId, string[] cards)
        //{
        //    Clients[_playerIdMap[clientId]].dealCards(cards);
        //}

        //public void PlayerSeated(string clientId, int position)
        //{
        //    var connectionId = "";
        //    if (_playerIdMap.TryGetValue(clientId, out connectionId))
        //    {
        //        Clients[connectionId].playerRegistered(clientId, position);
        //    }
        //}

        //#endregion

    }
}