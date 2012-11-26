using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Game.OFCP.Tests
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void Equality()
        {
            var c1 = new Card(69634, "2s");
            var c2 = new Card(69634, "2s");
            var c3 = new Card(69634, "2s");
            var c4 = new Card(73730, "2h");

            Assert.IsTrue(c1.Equals(c1));
            Assert.IsTrue(c1.Equals(c2));
            Assert.IsTrue(c2.Equals(c1));
            Assert.IsTrue(c1.Equals(c2) && c2.Equals(c3) && c1.Equals(c3));
            Assert.IsFalse(c1.Equals(null));
            Assert.IsTrue(c1 == c2 && c2 == c3 && c1 == c3);
            Assert.IsFalse(c1.Equals(c4));
            Assert.IsTrue(c1 != c4);
        }
    }
}
