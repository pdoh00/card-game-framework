using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace OFCPRepository
{
    public class DocumentStorageFactory 
    {
        static IDocumentStore GetDB(string location, string key)
        {
            if (key == null)
            {
                return new EmbeddableDocumentStore() { DataDirectory = location };
            }
            else
            {
                return new DocumentStore() { Url = location, ApiKey = key };
            }
        }
    }
}
