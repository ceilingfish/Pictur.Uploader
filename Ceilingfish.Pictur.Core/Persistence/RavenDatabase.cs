using System;
using Ceilingfish.Pictur.Core.Flickr;
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

        Settings IDatabase.Settings
        {
            get { throw new NotImplementedException(); }
        }

        public PluginResourceCollection<FlickrUpload> FlickrUploads
        {
            get { return new PluginResourceCollection<FlickrUpload>(_store); }
        }


        public void Dispose()
        {
            _store.Dispose();
        }
    }
}