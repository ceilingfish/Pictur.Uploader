using System;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;
using Raven.Client.Embedded;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class RavenDatabase : IDatabase, IDisposable
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

        public FileCollection Files
        {
            get { return new FileCollection(_store); }
        }

        public RavenPersistenceCollection<Directory> Directories
        {
            get { return new RavenPersistenceCollection<Directory>(_store); }
        }

        public RemoveFileCollection RemovedFileOperations
        {
            get { return new RemoveFileCollection(_store); }
        }

        public void Dispose()
        {
            _store.Dispose();
        }
    }
}