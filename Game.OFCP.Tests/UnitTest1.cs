using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerCalculator;


namespace Game.OFCP.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedExceptionAttribute(typeof(InvalidOperationException))]
        public void TestMethod1()
        {
            OFCP_Player player = new OFCP_Player("P1", "P1");
            player.AcceptCards(new List<Card>(), 0);
            string[] s1 = { "Ac", "Ad", "Kc", "Td", "5c" };
            player.SetBottomHand(s1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            OFCP_Player player = new OFCP_Player("P1", "P1");
            player.AcceptCards(new List<Card>() 
                                { 
                                    new Card(1, "Ac"),
                                    new Card(1, "Ad"),
                                    new Card(1, "Kc"),
                                    new Card(1, "Td"),
                                    new Card(1, "5c")

                                }, 0);
            string[] s1 = { "Ac", "Ad", "Kc", "Td", "5c" };
            player.SetBottomHand(s1);
        }

        private class ThreeCardHand
        {
            private const int THREE_CARD_HAND_SIZE = 3;
            private int[] _hand = new int[THREE_CARD_HAND_SIZE];

            public ThreeCardHand(int c1, int c2, int c3)
            {
                _hand[0] = c1;
                _hand[1] = c2;
                _hand[2] = c3;
            }

            public override string ToString()
            {
                return PokerLib.print_hand(_hand, THREE_CARD_HAND_SIZE);
            }
        }

        [TestMethod]
        public void Make_all_three_card_hands()
        {
            int[] deck = new int[52];
            List<ThreeCardHand> allHands = new List<ThreeCardHand>();

            // Init the deck in the Cactus Kev format..
            PokerLib.init_deck(deck);

            int[] hand = new int[3];
            //arrange
            for (int a = 0; a < 50; a++)
            {
                hand[0] = deck[a];
                for (int b = a + 1; b < 51; b++)
                {
                    hand[1] = deck[b];
                    for (int c = b + 1; c < 52; c++)
                    {
                        hand[2] = deck[c];
                        allHands.Add(new ThreeCardHand(hand[0], hand[1], hand[2]));
                    }
                }
            }
            //act
            using (var writer = File.CreateText(@"C:\temp\ThreeCardHands.txt"))
            {
                foreach (var h in allHands)
                {
                    writer.WriteLine(h.ToString());
                }
                writer.WriteLine(String.Format("Total hands: {0}", allHands.Count));
            }
        }
    }
}
