using System.Collections.Generic;
using System.Linq;
using Ceilingfish.Pictur.Core.Events;
using Raven.Client.Embedded;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class RavenDatabase : IDatabase
    {
        private readonly EmbeddableDocumentStore _store;

        public RavenDatabase(string path)
        {
            var documentStore = new EmbeddableDocumentStore
                                {
                                    DataDirectory = path
                                };
            documentStore.Initialize();
            _store = documentStore;
        }
        
        public IEnumerable<ManagedDirectory> ManagedDirectories
        {
            get { return GetDirectories(); }
        }

        public IEnumerable<File> Files { get; private set; }

        public bool Add(ManagedDirectory directory)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(directory);
                session.SaveChanges();
                return true;
            }
        }

        public void Remove(ManagedDirectory directory)
        {
            using (var session = _store.OpenSession())
            {
                session.Delete(directory);
                session.SaveChanges();
            }
        }

        public File GetFileByPath(string path)
        {
            using (var session = _store.OpenSession())
            {
                return session
                    .Query<File>()
                    .SingleOrDefault(f => f.Path.Equals(path));
            }
        }

        public File GetFileByCheckSum(string checksum)
        {
            using (var session = _store.OpenSession())
            {
                return session
                    .Query<File>()
                    .SingleOrDefault(f => f.Checksum.Equals(checksum));
            }
        }


        private IEnumerable<ManagedDirectory> GetDirectories()
        {
            using (var session = _store.OpenSession())
            {
                return session
                    .Query<ManagedDirectory>()
                    .ToArray();
            }
        }
    }
}