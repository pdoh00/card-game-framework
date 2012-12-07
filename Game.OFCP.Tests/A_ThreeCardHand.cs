using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Game.OFCP.Tests
{
    [TestClass]
    public class A_ThreeCardHand
    {
        [TestMethod]
        public void evaluates_a_set_correctly()
        {
            var tch = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            Assert.IsTrue(tch.IsSet());
        }

        [TestMethod]
        public void which_is_a_set_is_also_a_pair()
        {
            //arrange
            var tch = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            //act

            //assert
            Assert.IsTrue(tch.IsPair());
        }

        [TestMethod]
        public void evaluates_a_pair_correctly()
        {
            //arrange
            var tch = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            //act

            //assert
            Assert.IsTrue(tch.IsPair());
        }

        [TestMethod]
        public void with_some_different_ranks_is_not_a_set()
        {
            //arrange
            var tch = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            //act

            //assert
            Assert.IsFalse(tch.IsSet());
        }

        [TestMethod]
        public void with_all_different_ranks_is_not_a_pair()
        {
            //arrange
            var tch = new ThreeCardHand(new Card(268471337, "Ac"), new Card(8423187, "9c"), new Card(134253349, "Kc"));
            //act

            //assert
            Assert.IsFalse(tch.IsPair());
        }

        [TestMethod]
        public void set_is_greater_than_a_pair()
        {
            //arrange
            var set = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            var pair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));

            //act
            
            //assert
            Assert.IsTrue(set.CompareTo(pair) > 0);
        }

        [TestMethod]
        public void set_is_greater_than_high_card_hand()
        {
            //arrange
            var set = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            var highCardHand = new ThreeCardHand(new Card(268471337, "Ac"), new Card(8423187, "9c"), new Card(134253349, "Kc"));
            //act

            //assert
            Assert.IsTrue(set.CompareTo(highCardHand) > 0);
        }

        [TestMethod]
        public void pair_is_greater_than_high_card()
        {
            //arrange
            var pair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            var highCardHand = new ThreeCardHand(new Card(268471337, "Ac"), new Card(8423187, "9c"), new Card(134253349, "Kc"));

            //act

            //assert
            Assert.IsTrue(pair.CompareTo(highCardHand) > 0);
        }

        [TestMethod]
        public void pair_is_less_than_set()
        {
            //arrange
            var set = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            var pair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            //act

            //assert
            Assert.IsTrue(pair.CompareTo(set) < 0);
        }

        [TestMethod]
        public void equal_sets_are_equal() //this should never happen in a standard deck
        {
            //arrange
            var bigSet1 = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            var bigSet2 = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            //act

            //assert
            Assert.IsTrue(bigSet1.CompareTo(bigSet2) == 0);
        }

        [TestMethod]
        public void bigger_rank_set_greater_than_smaller_rank_set()
        {
            //arrange
            var bigSet = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            var smallSet = new ThreeCardHand(new Card(134253349, "Kc"), new Card(134236965, "Kd"), new Card(134228773, "Kh"));
            //act

            //assert
            Assert.IsTrue(bigSet.CompareTo(smallSet) > 0);
        }

        [TestMethod]
        public void smaller_rank_set_less_than_bigger_rank_set()
        {
            //arrange
            var bigSet = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(268446761, "Ah"));
            var smallSet = new ThreeCardHand(new Card(134253349, "Kc"), new Card(134236965, "Kd"), new Card(134228773, "Kh"));
            //act

            //assert
            Assert.IsTrue(smallSet.CompareTo(bigSet) < 0);
        }

        [TestMethod]
        public void bigger_pair_is_greater_than_smaller_pair()
        {
            //arrange
            var bigPair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            var smallPair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(134236965, "Kd"), new Card(134253349, "Kc"));
            //act

            //assert
            Assert.IsTrue(bigPair.CompareTo(smallPair) > 0);
        }

        [TestMethod]
        public void smaller_pair_is_less_than_bigger_pair()
        {
            //arrange
            var bigPair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            var smallPair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(134236965, "Kd"), new Card(134253349, "Kc"));
            //act

            //assert
            Assert.IsTrue(smallPair.CompareTo(bigPair) < 0);
        }

        [TestMethod]
        public void equal_pairs_with_same_kicker_are_equal()
        {
            //arrange
            var pair1 = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            var pair2 = new ThreeCardHand(new Card(268446761, "Ah"), new Card(268442665, "As"), new Card(134236965, "Kd"));
            //act

            //assert
            Assert.IsTrue(pair1.CompareTo(pair2) == 0);
        }

        [TestMethod]
        public void equal_pair_with_bigger_kicker_is_greater()
        {
            //arrange
            var bigKickerPair = new ThreeCardHand(new Card(268471337, "Ac"), new Card(268454953, "Ad"), new Card(134253349, "Kc"));
            var smallKickerPair = new ThreeCardHand(new Card(268446761, "Ah"), new Card(268442665, "As"), new Card(67144223, "Qc"));
            //act

            //assert
            Assert.IsTrue(bigKickerPair.CompareTo(smallKickerPair) > 0);
        }
    }
}
