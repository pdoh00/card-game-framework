using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP
{
    public enum CardId
    {
        TwoClubs = 98306,
        ThreeClubs = 164099,
        FourClubs = 295429,
        FiveClubs = 557831,
        SixClubs = 1082379,
        SevenClubs = 2131213,
        EightClubs = 4228625,
        NineClubs = 8423187,
        TenClubs = 16812055,
        JackClubs = 33589533,
        QueenClubs = 67144223,
        KingClubs = 134253349,
        AceClubs = 268471337,

        TwoHearts = 73730,
        ThreeHearts = 139523,
        FourHearts = 270853,
        FiveHearts = 533255,
        SixHearts = 1057803,
        SevenHearts = 2106637,
        EightHearts = 4204049,
        NineHearts = 8398611,
        TenHearts = 16787479,
        JackHearts = 33564957,
        QueenHearts = 67119647,
        KingHearts = 134228773,
        AceHearts = 268446761,

        TwoDiamonds = 81922,
        ThreeDiamonds = 147715,
        FourDiamonds = 279045,
        FiveDiamonds = 541447,
        SixDiamonds = 1065995,
        SevenDiamonds = 2114829,
        EightDiamonds = 4212241,
        NineDiamonds = 8406803,
        TenDiamonds = 16795671,
        JackDiamonds = 33573149,
        QueenDiamonds = 67127839,
        KingDiamonds = 134236965,
        AceDiamonds = 268454953,

        TwoSpades = 69634,
        ThreeSpades = 135427,
        FourSpades = 266757,
        FiveSpades = 529159,
        SixSpades = 1053707,
        SevenSpades = 2102541,
        EightSpades = 4199953,
        NineSpades = 8394515,
        TenSpades = 16783383,
        JackSpades = 33560861,
        QueenSpades = 67115551,
        KingSpades = 134224677,
        AceSpades = 268442665
    }

    public struct Card : IEquatable<Card>, IComparable<Card>
    {
        public readonly int Value;
        public readonly string Text;

        public Card(int value, string text)
        {
            Value = value;
            Text = text;
        }

        public Card(CardId value, string text)
        {
            Value = (int)value;
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
