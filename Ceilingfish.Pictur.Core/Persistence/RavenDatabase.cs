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

        public bool Remove(ManagedDirectory directory)
        {
            using (var session = _store.OpenSession())
            {
                var sessionThinger = session.Load<ManagedDirectory>(directory.Id);
                if (sessionThinger == null)
                    return false;
                session.Delete(sessionThinger);
                session.SaveChanges();
                return true;
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

        public IEnumerable<FileHarmonization> GetHarmonizationsByFile(string id)
        {
            using (var session = _store.OpenSession())
            {
                return session
                    .Query<FileHarmonization>()
                    .Where(f => f.FileId.Equals(id));
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

        public IEnumerable<File> RecentlyModifiedFiles
        {
            get 
            {
                using (var session = _store.OpenSession())
                {
                    return session.Query<File>()
                                    .OrderBy(c => c.ModifiedAt)
                                    .Take(20)
                                    .ToArray();
                }
            }

        }

        public void Add(ManagedDirectory directory)
        {
            UpdateRecord(directory);
        }

        public void Update(FileHarmonization current)
        {
            UpdateRecord(current);
        }

        public void Update(File current)
        {
            UpdateRecord(current);
        }

        public void Add(File newFile)
        {
            UpdateRecord(newFile);
        }

        public void Add(FileHarmonization harmonization)
        {
            UpdateRecord(harmonization);
        }

        private void UpdateRecord<T>(T current)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(current);
                session.SaveChanges();
            }
        }
    }
}