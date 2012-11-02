using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
