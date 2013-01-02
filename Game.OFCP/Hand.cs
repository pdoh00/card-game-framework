using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerCalculator;

namespace Game.OFCP
{
    public abstract class Hand
    {
        public Hand()
        {

        }
    }

    public class ThreeCardHand : IComparable<ThreeCardHand>
    {
        private Card[] _cards = new Card[3];

        public ThreeCardHand(params Card[] cards)
        {
            if (cards.Count() != 3)
                throw new ArgumentException("This is a 3 card hand dummy...give me 3 cards");

            _cards[0] = cards[0];
            _cards[1] = cards[1];
            _cards[2] = cards[2];
        }

        public int CompareTo(ThreeCardHand other)
        {
            if (this.IsSet() && !other.IsSet())
                return 1;
            if (!this.IsSet() && other.IsSet())
                return -1;

            if (this.IsPair() && !other.IsPair())
                return 1;
            if (!this.IsPair() && other.IsPair())
                return -1;

            //set vs set
            if (this.IsSet() && other.IsSet())
            {
                return _cards[0].CompareTo(other._cards[0]);
            }

            //pair vs pair
            if (this.IsPair() && other.IsPair())
            {
                //this is some ugly code.  Prime refactoring target.
                var thisPair = _cards.GroupBy(c => c.Value & 0xFF).Where(g => g.Count() == 2).ToList()[0].ToList();
                var thisPairKicker = _cards.GroupBy(c => c.Value & 0xFF).Where(g => g.Count() == 1).ToList()[0].ToList()[0];
                var otherPair = other._cards.GroupBy(c => c.Value & 0xFF).Where(g => g.Count() == 2).ToList()[0].ToList();
                var otherPairKicker = other._cards.GroupBy(c => c.Value & 0xFF).Where(g => g.Count() == 1).ToList()[0].ToList()[0];

                if (thisPair[0]> otherPair[0])
                    return 1;
                if (thisPair[0] < otherPair[0])
                    return -1;
                if (thisPair[0] == otherPair[0])
                {
                    return thisPairKicker.CompareTo(otherPairKicker);
                }
            }

            //high card hand vs high card hand

            return 0;
        }

        public bool IsSet()
        {
            return _cards[0].CompareTo(_cards[1])==0 &&
                   _cards[1].CompareTo(_cards[2])==0;
        }

        public bool IsPair()
        {
            return _cards[0].CompareTo(_cards[1])==0 ||
                   _cards[0].CompareTo(_cards[2]) == 0 ||
                   _cards[1].CompareTo(_cards[2]) == 0;
        }

        public override string ToString()
        {
            return PokerLib.print_hand(new int[3] { _cards[0].Value, _cards[1].Value, _cards[2].Value }, 3);
        }

    }
}
