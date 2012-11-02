using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Game.OFCP
{
    public class KnuthShuffler : IShuffler
    {
        public void Shuffle(int[] deck)
        {
            var seed = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(seed);
            int seedInt = BitConverter.ToInt32(seed, 0);
            var rng = new Random(seedInt);

            for (int n = deck.Length - 1; n > 0; --n)
            {
                int k = rng.Next(n + 1);
                int temp = deck[n];
                deck[n] = deck[k];
                deck[k] = temp;
            }
        }
    }
}
