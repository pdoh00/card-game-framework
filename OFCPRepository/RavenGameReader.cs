using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Document;

namespace OFCPRepository
{
    class RavenGameReader : IGameReader
    {
        private IDocumentStore m_docStore;
        public RavenGameReader(IDocumentStore docStore) { m_docStore = docStore; }

        public IEnumerable<GameScore> GetGames(DateTime date)
        {
            m_docStore.Initialize();
            using (var session = m_docStore.OpenSession())
            {
                var results = from games in session.Query<GameScore>()
                                where games.PlayTime.Date == date.Date
                                select games;

                return results;
            }
        }
    }
}
