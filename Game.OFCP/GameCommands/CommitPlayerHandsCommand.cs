using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.GameCommands
{
    public class CommitPlayerHandsCommand : ICommand
    {
        private const int BOTTOM_HAND_SIZE = 5;
        private const int MIDDLE_HAND_SIZE = 5;
        private const int TOP_HAND_SIZE = 3;

        public readonly string TableId;
        public readonly string GameId;
        public readonly string PlayerId;

        public readonly string[] BottomHand = new string[BOTTOM_HAND_SIZE];
        public readonly string[] MiddleHand = new string[MIDDLE_HAND_SIZE];
        public readonly string[] TopHand = new string[TOP_HAND_SIZE];

        public CommitPlayerHandsCommand(string tableId, string gameId, string playerId, string[] bottom, string[] middle, string[] top)
        {
            if (bottom.Length != BOTTOM_HAND_SIZE)
                throw new ArgumentException("The bottom hand must contain exactly 5 cards");

            if (middle.Length != MIDDLE_HAND_SIZE)
                throw new ArgumentException("The middle hand must contain exactly 5 cards");

            if (top.Length != TOP_HAND_SIZE)
                throw new ArgumentException("The top hand must contain exactly 3 cards");

            TableId = tableId;
            GameId = gameId;
            PlayerId = playerId;

            bottom.CopyTo(BottomHand, 0);
            middle.CopyTo(MiddleHand, 0);
            top.CopyTo(TopHand, 0);
        }

        public override string ToString()
        {
            return String.Format("Player: {0} on Table: {1}, Game: {2} Bottom: {3} Middle: {4} Top: {5}",
                PlayerId,
                TableId,
                GameId,
                String.Concat(BottomHand),
                String.Concat(MiddleHand),
                String.Concat(TopHand));
        }
    }
}
