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
        private int[] _cardValues = new int[3];

        public ThreeCardHand(params Card[] cards)
        {
            if (cards.Count() != 3)
                throw new ArgumentException("This is a 3 card hand dummy...give me 3 cards");

            _cards[0] = cards[0];
            _cards[1] = cards[1];
            _cards[2] = cards[2];

            _cardValues[0] = cards[0].Value;
            _cardValues[1] = cards[1].Value;
            _cardValues[2] = cards[2].Value;
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
            if(this.IsSet() && other.IsSet())
            {
                var thisVal = _cards[0].Value * _cards[1].Value * _cards[2].Value;
                var otherVal = other._cards[0].Value * other._cards[1].Value * other._cards[2].Value;

                if (thisVal > otherVal)
                    return 1;
                if (otherVal > thisVal)
                    return -1;
                return 0;
            }

            //pair vs pair
            if (this.IsPair() && other.IsPair())
            {

            }
            //high card hand vs high card hand

            return 0;
        }

        public bool IsSet()
        {
            return (_cards[0].Value & 0xFF) == (_cards[1].Value & 0xFF) && 
                   (_cards[1].Value & 0xFF) == (_cards[2].Value & 0xFF);
        }

        public bool IsPair()
        {
            return (_cards[0].Value & 0xFF) == (_cards[1].Value & 0xFF) ||
                   (_cards[0].Value & 0xFF) == (_cards[2].Value & 0xFF) ||
                   (_cards[1].Value & 0xFF) == (_cards[2].Value & 0xFF);
        }

        public override string ToString()
        {
            return PokerLib.print_hand(new int[3] { _cards[0].Value, _cards[1].Value, _cards[2].Value }, 3);
        }

    }
}
