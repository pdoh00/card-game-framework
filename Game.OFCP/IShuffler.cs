using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.OFCP
{
    public interface IShuffler
    {
        /// <summary>
        /// Shuffles a deck of cards.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        void Shuffle(int[] deck);
    }
}
