using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Document;

namespace OFCPRepository
{
    public class RavenGameWriter : IGameWriter
    {
        private IDocumentStore m_docStore;
        public RavenGameWriter(IDocumentStore docStore) { m_docStore = docStore; }
        public bool SaveGame(GameScore game)
        {
            bool saveSuccessful = false;

            try
            {
                m_docStore.Initialize();
                using (var session = m_docStore.OpenSession())
                {
                    session.Store(game);
                    session.SaveChanges();
                    saveSuccessful = true;
                }
            }
            catch (Exception)
            {
                saveSuccessful = false;
            }

            return saveSuccessful;
        }
    }
}
