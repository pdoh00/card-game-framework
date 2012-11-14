using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.OFCP.Events;
using Infrastructure;

namespace Game.OFCP
{
    public abstract class Player
    {
        protected string _gameId = null;
        private List<Card> _pocketCards;
        private string _id;

        private Player()
        {
            _pocketCards = new List<Card>();
        }

        public Player(string id, string name)
            : this()
        {
            Name = name;
            _id = id;
        }
        public string Id
        {
            get { return _id; }
        }

        public IReadOnlyList<Card> Cards
        {
            get
            {
                return _pocketCards.AsReadOnly();
            }
        }

        public readonly string Name;
        public byte[] Avatar { get; set; }

        public void AcceptCards(List<Card> cards, int round)
        {
            _pocketCards.Clear();
            _pocketCards.AddRange(cards);
        }
    }

    public sealed class OFCP_Player : Player
    {
        private int BOTTOM_HAND_SIZE = 5;
        private int MIDDLE_HAND_SIZE = 5;
        private int TOP_HAND_SIZE = 3;

        private string[] _bottomHand;
        private string[] _middleHand;
        private string[] _topHand;

        public OFCP_Player(string playerId, string name)
            : base(playerId, name)
        {
            _bottomHand = new string[BOTTOM_HAND_SIZE];
            _middleHand = new string[MIDDLE_HAND_SIZE];
            _topHand = new string[TOP_HAND_SIZE];
        }

        public void SetBottomHand(string[] cards)
        {
            if (cards.Length != BOTTOM_HAND_SIZE)
                throw new InvalidOperationException("The bottom hand must be 5 cards");

            ThrowIfUsingInvalidCards(cards);

            cards.CopyTo(_bottomHand, 0);
        }

        public void SetMiddleHand(string[] cards)
        {
            if (cards.Length != MIDDLE_HAND_SIZE)
                throw new InvalidOperationException("The middle hand must be 5 cards");

            ThrowIfUsingInvalidCards(cards);

            cards.CopyTo(_middleHand, 0);
        }

        public void SetTopHand(string[] cards)
        {
            if (cards.Length != TOP_HAND_SIZE)
                throw new InvalidOperationException("The top hand must be 3 cards");

            ThrowIfUsingInvalidCards(cards);

            cards.CopyTo(_topHand, 0);
        }

        private void ThrowIfUsingInvalidCards(string[] cards)
        {
            var intersect = cards.Intersect(Cards.Select(c => c.Text));
            var invalidCards = cards.Except(intersect);
            var hasInvalidCards = invalidCards.Any();
            if (hasInvalidCards)
                throw new InvalidOperationException("The player is attempting to make a hand of cards that they don't have.");
        }
    }
}
