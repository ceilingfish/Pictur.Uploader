using Ceilingfish.Pictur.Core.Models;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class RemoveFileCollection : RavenPersistenceCollection<RemovedFileOperation>
    {
        public RemoveFileCollection(EmbeddableDocumentStore store)
            :base(store)
        {
        }

        public RemovedFileOperation GetByFileId(string id)
        {
            using (var session = Store.OpenSession())
            {
                return session.Query<RemovedFileOperation>().SingleOrDefault(f => f.FileId.Equals(id));
            }
        }
    }
}
