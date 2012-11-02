using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP
{
    public enum HandRank
    {
        None = 0,
        Pair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush
    }

    public enum CardId
    {
        TwoClubs = 1,
        ThreeClubs = 2,
        FourClubs = 3,
        FiveClubs = 4,
        SixClubs = 5,
        SevenClubs = 6,
        EightClubs = 7,
        NineClubs = 8,
        TenClubs = 9,
        JackClubs = 10,
        QueenClubs = 11,
        KingClubs = 12,
        AceClubs = 13,

        TwoHearts = 14,
        ThreeHearts = 15,
        FourHearts = 16,
        FiveHearts = 17,
        SixHearts = 18,
        SevenHearts = 19,
        EightHearts = 20,
        NineHearts = 21,
        TenHearts = 22,
        JackHearts = 23,
        QueenHearts = 24,
        KingHearts = 25,
        AceHearts = 26,

        TwoDiamonds = 27,
        ThreeDiamonds = 28,
        FourDiamonds = 29,
        FiveDiamonds = 30,
        SixDiamonds = 31,
        SevenDiamonds = 32,
        EightDiamonds = 33,
        NineDiamonds = 34,
        TenDiamonds = 35,
        JackDiamonds = 36,
        QueenDiamonds = 37,
        KingDiamonds = 38,
        AceDiamonds = 39,

        TwoSpades = 40,
        ThreeSpades = 41,
        FourSpades = 42,
        FiveSpades = 43,
        SixSpades = 44,
        SevenSpades = 45,
        EightSpades = 46,
        NineSpades = 47,
        TenSpades = 48,
        JackSpades = 49,
        QueenSpades = 50,
        KingSpades = 51,
        AceSpades = 52
    }

    public class NaiveHandEvaluator
    {
        /// <summary>
        /// Compares to hands
        /// </summary>
        /// <param name="hand1"></param>
        /// <param name="hand2"></param>
        /// <returns>1 if hand1 is best, -1 if hand 2 is best, 0 if hands are equal.</returns>
        public int Compare(IEnumerable<int> hand1, IEnumerable<int> hand2)
        {
            return 0;
        }

        public HandRank Evaluate(IEnumerable<int> hand)
        {
            //is straight flush (5 cards in rank order same suit)

            //is four of a kind (4 cards same rank)

            //is fullhouse(3 cards same rank and 2 cards same rank)

            //is flush (5 cards same suit)

            //is straight (5 cards in rank order)

            //is 3 of a kind (3 cards same rank)

            //is 2 pair (2 cards same rank and 2 other cards same rank)

            //is pair (2 cards same rank)

            return HandRank.None;
        }
    }
}
