using System.Linq;
using Ceilingfish.Pictur.Core.Models;
using Raven.Client;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class PluginResourceCollection<T> : RavenPersistenceCollection<T>
        where T : PluginResource
    {
        public PluginResourceCollection(IDocumentStore store)
            : base(store)
        {
        }

        public T GetByFileId(string fileId)
        {
            using (var session = Store.OpenSession())
            {
                return session.Query<T>().Single(r => string.Equals(r.FileId, fileId));
            }
        }
    }
}
