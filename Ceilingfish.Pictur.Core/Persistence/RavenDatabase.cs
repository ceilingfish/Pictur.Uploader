using System.Collections.Generic;
using System.Linq;
using Ceilingfish.Pictur.Core.Events;
using Raven.Client.Embedded;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class RavenDatabase : IDatabase
    {
        public event RecordAdded<ManagedDirectory> DirectoryAdded;
        public event RecordRemoved<ManagedDirectory> DirectoryRemoved;

        private readonly EmbeddableDocumentStore _store;

        public RavenDatabase(string path)
        {
            var documentStore = new EmbeddableDocumentStore
                                {
                                    DataDirectory = path,
                                    DefaultDatabase = "pictur"
                                };
            documentStore.Initialize();
            _store = documentStore;
        }
        
        public IEnumerable<ManagedDirectory> ManagedDirectories
        {
            get { return GetDirectories(); }
        }

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