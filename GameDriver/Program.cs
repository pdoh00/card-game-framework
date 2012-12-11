using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Game.OFCP;
using Game.OFCP.EventHandlers;
using Game.OFCP.GameCommandHandlers;
using Game.OFCP.Games;
using Game.OFCP.TableCommandHandlers;
using Game.OFCP.TableCommands;
using Infrastructure;

namespace GameDriver
{
    class Program
    {
        private class ClientChannelMock : IClientChannel
        {
            private Action<string> Broadcaster;
            public ClientChannelMock(Action<string> broadcaster)
            {
                Broadcaster = broadcaster;
            }

            public void BroadcastPlayerSeated(string tableId, string name, int position)
            {
                Broadcaster(String.Format("Player {0} seated at position {1} on table {2}", name, position, tableId));
            }

            public void BroadcastPlayerLeft(string tableId, string name, int position)
            {
                Broadcaster(String.Format("Player {0} left position {1} on table {2}", name, position, tableId));
            }

            public void BroadcastPlayerHandSet(string tableId, int position)
            {
                Broadcaster(String.Format("Player set hand at position {0} on table {1}", position, tableId));
            }

            public void BroadcastPlayerCardsRearranged(string tableId, int playerPosition, bool[] cards)
            {
                Broadcaster(String.Format("Player position {0} Rearranged cards on table {1}", playerPosition, tableId));
            }

            public void BroadcastDeckShuffled(string tableId)
            {
                Broadcaster(String.Format("Deck shuffled on table {1}", tableId));
            }

            public void BroadcastGameStarted(string tableId, string gameId, DateTime timestamp)
            {
                Broadcaster(String.Format("New game {0} started on table {1}", gameId, tableId));
            }

            public void BroadcastPlayerReady(string tableId, int position)
            {
                Broadcaster(String.Format("Position {0} on table {1} is ready to play.", position, tableId));
            }

            public void PlayerDealtCards(string clientId, string[] cards)
            {
                Broadcaster(String.Format("Player {0} was dealt cards {1}", clientId, String.Join(",", cards)));
            }

            public void PlayerSeated(string clientId, int position)
            {
                Broadcaster(String.Format("Player {0} was seated at position {1}", clientId, position));
            }


            public void BroadcastPlayerCardsRearranged(string tableId, int playerPosition, List<CardPositionState> cards)
            {
                throw new NotImplementedException();
            }

            public void TableInitialized(string connectionId)
            {
                throw new NotImplementedException();
            }

            public void SetTableState(string connectionId, TableState tableState)
            {
                throw new NotImplementedException();
            }
        }


        static void Main(string[] args)
        {
            
            var shuffler = new KnuthShuffler();
            var clientChannel = new ClientChannelMock(Console.WriteLine);
            var tables = new MemoryTableProjection();
            var players = new MemoryPlayerProjection();

            var cmdBus = new MemoryCommandBus();
            var evtBus = new MemoryEventBus();
            var evtStore = new MemoryEventStore(evtBus);
            var tableRepos = new Repository<Table>(evtStore);

            cmdBus.Register(new TableCommandHandler(tableRepos));
            cmdBus.Register(new GameCommandHandler(tables, players, new Repository<OFCP_Game>(evtStore)));
            evtBus.Register(new GameEventHandler(clientChannel));
            var tblEvtHandler = new TableEventHandler(clientChannel, tables, players);
            tblEvtHandler.CommandBus = cmdBus;
            evtBus.Register(tblEvtHandler);
            
            //var table = new OFCPTable(Guid.NewGuid().ToString());
            //var newGame = GameFactory.CreateGame(table.Id, GameType.ChinesePoker, shuffler);

            cmdBus.Send(new CreateNewTableCommand(Game.OFCP.Games.OFCP_Game.OFCP_GAME_TYPE));

            while (tables.ListTables().Count == 0)
            {
                Thread.Sleep(25);
            }

            var table = tables.ListTables()[0];

            var p1Id = Guid.NewGuid().ToString();
            var p2Id = Guid.NewGuid().ToString();
            var p3Id = Guid.NewGuid().ToString();
            var p4Id = Guid.NewGuid().ToString();

            cmdBus.Send(new SeatPlayerCommand(table.TableId, p1Id, "Fred"));
            cmdBus.Send(new SeatPlayerCommand(table.TableId, p2Id, "John"));
            cmdBus.Send(new SeatPlayerCommand(table.TableId, p3Id, "Sally"));
            cmdBus.Send(new SeatPlayerCommand(table.TableId, p4Id, "Bob"));

            cmdBus.Send(new SetPlayerReadyCommand(table.TableId, p1Id));
            cmdBus.Send(new SetPlayerReadyCommand(table.TableId, p2Id));
            cmdBus.Send(new SetPlayerReadyCommand(table.TableId, p3Id));
            cmdBus.Send(new SetPlayerReadyCommand(table.TableId, p4Id));

            

            //table.SeatPlayer(p1Id, "Phil");
            //table.SeatPlayer(p2Id, "Rod");
            //table.SeatPlayer(p3Id, "Steve");
            //table.SeatPlayer(p4Id, "Shannon");

        

            //newGame.SetHand(p1Id, new string[13] { "2c", "3c", "4c", "5c", "6c", "7c", "8c", "9c", "Tc", "Jc", "Qc", "Kc", "Ac" });
            //newGame.SetHand(p2Id, new string[13] { "2d", "3d", "4d", "5d", "6d", "7d", "8d", "9d", "Td", "Jd", "Qd", "Kd", "Ad" });
            //newGame.SetHand(p3Id, new string[13] { "2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "Th", "Jh", "Qh", "Kh", "Ah" });
            //newGame.SetHand(p4Id, new string[13] { "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "Ts", "Js", "Qs", "Ks", "As" });


            //var sw = new Stopwatch();
            //sw.Start();
            //deck.Shuffle();
            //sw.Stop();
            //Console.WriteLine(String.Format("Post-shuffle - {0}", deck.ToString()));
            //Console.WriteLine(String.Format("Shuffle took {0} ms", sw.ElapsedMilliseconds));

            //var hand1 = deck.Take(5);
            //Console.WriteLine(PokerCalculator.PokerLib.print_hand(hand1.ToArray(), hand1.Count));

            //var hand2 = deck.Take(5);
            //Console.WriteLine(PokerCalculator.PokerLib.print_hand(hand2.ToArray(), hand2.Count));

            //var hand3 = deck.Take(5);
            //Console.WriteLine(PokerCalculator.PokerLib.print_hand(hand3.ToArray(), hand3.Count));

            //var hand4 = deck.Take(5);
            //Console.WriteLine(PokerCalculator.PokerLib.print_hand(hand4.ToArray(), hand4.Count));

            //var hand1Val = PokerCalculator.PokerLib.eval_5hand(hand1.ToArray());
            //var hand1Rank = PokerCalculator.PokerLib.hand_rank(hand1Val);
            //Console.WriteLine(String.Format("Hand1 value: {0}", hand1Val));
            //Console.WriteLine(String.Format("Hand1 rank: {0}", hand1Rank));

            //var hand2Val = PokerCalculator.PokerLib.eval_5hand(hand2.ToArray());
            //var hand2Rank = PokerCalculator.PokerLib.hand_rank(hand2Val);
            //Console.WriteLine(String.Format("Hand2 value: {0}", hand2Val));
            //Console.WriteLine(String.Format("Hand2 rank: {0}", hand2Rank));

            //var hand3Val = PokerCalculator.PokerLib.eval_5hand(hand3.ToArray());
            //var hand3Rank = PokerCalculator.PokerLib.hand_rank(hand3Val);
            //Console.WriteLine(String.Format("Hand3 value: {0}", hand3Val));
            //Console.WriteLine(String.Format("Hand3 rank: {0}", hand3Rank));

            //var hand4Val = PokerCalculator.PokerLib.eval_5hand(hand4.ToArray());
            //var hand4Rank = PokerCalculator.PokerLib.hand_rank(hand4Val);
            //Console.WriteLine(String.Format("Hand4 value: {0}", hand4Val));
            //Console.WriteLine(String.Format("Hand4 rank: {0}", hand4Rank));


            //var hand5 = deck.Take(3);
            //hand5.Add(0);
            //hand5.Add(0);

            //Console.WriteLine(PokerCalculator.PokerLib.print_hand(hand5.ToArray(), hand5.Count));
            //var hand5Val = PokerCalculator.PokerLib.eval_5hand(hand5.ToArray());
            //var hand5Rank = PokerCalculator.PokerLib.hand_rank(hand5Val);
            //Console.WriteLine(String.Format("Hand2 value: {0}", hand5Val));
            //Console.WriteLine(String.Format("Hand2 rank: {0}", hand5Rank));


            Console.Read();
        }
    }
}
