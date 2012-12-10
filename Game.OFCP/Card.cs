﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP
{
    public struct Card : IEquatable<Card>, IComparable<Card>
    {
        public readonly int Value;
        public readonly string Text;

        public Card(int value, string text)
        {
            Value = value;
            Text = text;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Text, Value);
        }

        public bool Equals(Card other)
        {
            return (Value & 0xFF) == (other.Value & 0xFF);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Card)obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Value ^ Text.GetHashCode();
        }

        public static bool operator ==(Card leftCard, Card rightCard)
        {
            if (Object.ReferenceEquals(leftCard, null))
            {
                if (Object.ReferenceEquals(rightCard, null))
                {
                    return true; //true if both are null
                }
                return false;
            }
            return leftCard.Equals(rightCard); //will handle if rightCard is null
        }

        public static bool operator !=(Card leftCard, Card rightCard)
        {
            return !(leftCard == rightCard);
        }

        public static bool operator >(Card leftCard, Card rightCard)
        {
            return (leftCard.Value & 0xFF) > (rightCard.Value & 0xFF);
        }

        public static bool operator <(Card leftCard, Card rightCard)
        {
            return (leftCard.Value & 0xFF) < (rightCard.Value & 0xFF);
        }

        public int CompareTo(Card other)
        {
            if ((Value & 0xFF) > (other.Value & 0xFF))
                return 1;
            if ((Value & 0xFF) < (other.Value & 0xFF))
                return -1;
            if ((Value & 0xFF) == (other.Value & 0xFF))
                return 0;

            throw new InvalidOperationException("Card equality is not working correctly.");
        }
    }
}
