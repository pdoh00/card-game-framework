using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;

namespace OFCPRepository
{
    interface IGameWriter
    {
        bool SaveGame(GameScore game);
    }
}
