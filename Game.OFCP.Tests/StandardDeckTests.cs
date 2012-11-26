using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Game.OFCP.Tests
{
    /// <summary>
    /// Summary description for StandardDeckTests
    /// </summary>
    [TestClass]
    public class StandardDeckTests
    {
        private const int STANDARD_DECK_SIZE = 52;
        StandardDeck _theDeck;

        private static HashSet<Card> _expectedDeckCards;

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
            _expectedDeckCards = new HashSet<Card>();
            _expectedDeckCards.Add(new Card(69634, "2s"));
            _expectedDeckCards.Add(new Card(73730, "2h"));
            _expectedDeckCards.Add(new Card(81922, "2d"));
            _expectedDeckCards.Add(new Card(98306, "2c"));
            _expectedDeckCards.Add(new Card(135427, "3s"));
            _expectedDeckCards.Add(new Card(139523, "3h"));
            _expectedDeckCards.Add(new Card(147715, "3d"));
            _expectedDeckCards.Add(new Card(164099, "3c"));
            _expectedDeckCards.Add(new Card(266757, "4s"));
            _expectedDeckCards.Add(new Card(270853, "4h"));
            _expectedDeckCards.Add(new Card(279045, "4d"));
            _expectedDeckCards.Add(new Card(295429, "4c"));
            _expectedDeckCards.Add(new Card(529159, "5s"));
            _expectedDeckCards.Add(new Card(533255, "5h"));
            _expectedDeckCards.Add(new Card(541447, "5d"));
            _expectedDeckCards.Add(new Card(557831, "5c"));
            _expectedDeckCards.Add(new Card(1053707, "6s"));
            _expectedDeckCards.Add(new Card(1057803, "6h"));
            _expectedDeckCards.Add(new Card(1065995, "6d"));
            _expectedDeckCards.Add(new Card(1082379, "6c"));
            _expectedDeckCards.Add(new Card(2102541, "7s"));
            _expectedDeckCards.Add(new Card(2106637, "7h"));
            _expectedDeckCards.Add(new Card(2114829, "7d"));
            _expectedDeckCards.Add(new Card(2131213, "7c"));
            _expectedDeckCards.Add(new Card(4199953, "8s"));
            _expectedDeckCards.Add(new Card(4204049, "8h"));
            _expectedDeckCards.Add(new Card(4212241, "8d"));
            _expectedDeckCards.Add(new Card(4228625, "8c"));
            _expectedDeckCards.Add(new Card(8394515, "9s"));
            _expectedDeckCards.Add(new Card(8398611, "9h"));
            _expectedDeckCards.Add(new Card(8406803, "9d"));
            _expectedDeckCards.Add(new Card(8423187, "9c"));
            _expectedDeckCards.Add(new Card(16783383, "Ts"));
            _expectedDeckCards.Add(new Card(16787479, "Th"));
            _expectedDeckCards.Add(new Card(16795671, "Td"));
            _expectedDeckCards.Add(new Card(16812055, "Tc"));
            _expectedDeckCards.Add(new Card(33560861, "Js"));
            _expectedDeckCards.Add(new Card(33564957, "Jh"));
            _expectedDeckCards.Add(new Card(33573149, "Jd"));
            _expectedDeckCards.Add(new Card(33589533, "Jc"));
            _expectedDeckCards.Add(new Card(67115551, "Qs"));
            _expectedDeckCards.Add(new Card(67119647, "Qh"));
            _expectedDeckCards.Add(new Card(67127839, "Qd"));
            _expectedDeckCards.Add(new Card(67144223, "Qc"));
            _expectedDeckCards.Add(new Card(134224677, "Ks"));
            _expectedDeckCards.Add(new Card(134228773, "Kh"));
            _expectedDeckCards.Add(new Card(134236965, "Kd"));
            _expectedDeckCards.Add(new Card(134253349, "Kc"));
            _expectedDeckCards.Add(new Card(268442665, "As"));
            _expectedDeckCards.Add(new Card(268446761, "Ah"));
            _expectedDeckCards.Add(new Card(268454953, "Ad"));
            _expectedDeckCards.Add(new Card(268471337, "Ac"));
        }                                                     
        //                                                    
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]                                   
        // public static void MyClassCleanup() { }            
        //                                                    
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]                                    
        public void MyTestInitialize() 
        {
            _theDeck = new StandardDeck(new KnuthShuffler(), Guid.NewGuid().ToString());
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void unshuffeled_deck_is_empty()
        {
            //arrange

            //act

            //assert
            Assert.IsTrue(_theDeck.IsEmpty());
        }

        [TestMethod]
        public void shuffled_deck_is_not_empty()
        {
            //arrange

            //act
            _theDeck.Shuffle();

            //assert
            Assert.IsFalse(_theDeck.IsEmpty());
        }

        [TestMethod]
        public void shuffled_deck_starts_with_52_cards()
        {
            //arrange

            //act
            _theDeck.Shuffle();

            //assert
            Assert.AreEqual(STANDARD_DECK_SIZE , _theDeck.RemainingCards());
        }

        [TestMethod]
        public void shuffled_deck_contains_exactly_the_right_cards()
        {
            //arrange

            //act
            _theDeck.Shuffle();
            //assert
            Assert.IsTrue(_expectedDeckCards.SetEquals(_theDeck.Deal(STANDARD_DECK_SIZE)));
        }

        [TestMethod]
        public void shuffled_deck_deals_unique_hands()
        {
            //arrange
            List<Card> hand1;
            List<Card> hand2;
            List<Card> hand3;
            List<Card> hand4;
            _theDeck.Shuffle();

            //act
            hand1 = _theDeck.Deal(13);
            hand2 = _theDeck.Deal(13);
            hand3 = _theDeck.Deal(13);
            hand4 = _theDeck.Deal(13);

            //assert
            Assert.AreEqual(0, hand1.Intersect(hand2).Count());
            Assert.AreEqual(0, hand1.Intersect(hand3).Count());
            Assert.AreEqual(0, hand1.Intersect(hand4).Count());
            Assert.AreEqual(0, hand2.Intersect(hand3).Count());
            Assert.AreEqual(0, hand2.Intersect(hand4).Count());
            Assert.AreEqual(0, hand3.Intersect(hand4).Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//assert
        public void unshuffled_deck_throws_if_dealt()
        {
            //arrange

            //act
            _theDeck.Deal(1);
            
        }
    }
}
