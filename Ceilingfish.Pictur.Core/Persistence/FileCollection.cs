using Raven.Client.Embedded;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Client;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class FileCollection : RavenPersistenceCollection<Models.File>
    {
        public FileCollection(IDocumentStore store)
            : base(store)
        {
        }

        public IEnumerable<Models.File> RecentlyModified
        {
            get
            {
                using (var session = Store.OpenSession())
                {
                    var lastDay = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
                    return session.Query<Models.File>().Where(f => f.ModifiedAt > lastDay && !f.Deleted);
                }
            }
        }

        public IEnumerable<Models.File> GetByPath(string path)
        {
            using (var session = Store.OpenSession())
            {
                path = Path.GetFullPath(path);
                return session.Query<Models.File>().Where(f => f.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase) && !f.Deleted);
            }
        }

        public IEnumerable<Models.File> QueryByChecksum(string checksum)
        {
            using (var session = Store.OpenSession())
            {
                return session.Query<Models.File>().Where(f => f.Checksum.Equals(checksum) && !f.Deleted);
            }
        }

        public IEnumerable<Models.File> GetByPathAndChecksum(string path, string checksum)
        {
            using (var session = Store.OpenSession())
            {
                return session.Query<Models.File>()
                    .Where(f => f.Checksum.Equals(checksum) && f.Path.Equals(path) && !f.Deleted);
            }
        }
    }
}