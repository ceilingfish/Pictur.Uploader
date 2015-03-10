using System;
using System.IO;
using System.Linq;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;
using NUnit.Framework;
using Raven.Client.Embedded;
using Directory = System.IO.Directory;

namespace Ceilingfish.Pictur.Core.Tests.Persistence
{
    public class RavenPersistenceCollectionTests
    {
        public class MockRecord : BaseRecord
        {
            public string Name { get; set; }
        }

        private string _path;
        protected RavenPersistenceCollection<MockRecord> PersistenceCollection;
        protected EmbeddableDocumentStore DocumentStore;

        public void SetUp()
        {
            Console.WriteLine("Setting up");
            _path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            DocumentStore = new EmbeddableDocumentStore
            {
                DataDirectory = _path
            };
            DocumentStore.Initialize();
            PersistenceCollection = new RavenPersistenceCollection<MockRecord>(DocumentStore);
        }

        public void TearDown()
        {
            DocumentStore.Dispose();
            DocumentStore.AfterDispose += (a, e) =>
            {
                Directory.Delete(_path, true);
            };
        }

        public class Create_RavenDatabaseTests : RavenPersistenceCollectionTests
        {
            public void ShouldAddARecord()
            {
                var record = new MockRecord { Name = "test" };
                PersistenceCollection.Add(record);
                using (var session = DocumentStore.OpenSession())
                {
                    var stored = session.Query<MockRecord>().Single(m => m.Name.Equals("test"));
                    Assert.AreEqual(stored.Id, record.Id);
                }
            }

            public void ShouldSetCreatedAt()
            {
                var record = new MockRecord { Name = "test" };
                PersistenceCollection.Add(record);
                Assert.AreNotEqual(record.CreatedAt, default(DateTime));

            }

            public void ShouldSetModifiedAt()
            {
                var record = new MockRecord { Name = "test" };
                PersistenceCollection.Add(record);
                Assert.AreNotEqual(record.ModifiedAt, default(DateTime));
            }

            public void ShouldSetId()
            {
                var record = new MockRecord { Name = "test" };
                PersistenceCollection.Add(record);
                Assert.IsNotNull(record.Id);
            }
        }

        public class Update_RavenPersistenceCollectionTests : RavenPersistenceCollectionTests
        {
            private MockRecord _record;

            public new void SetUp()
            {
                _record = new MockRecord { Name = "the original" };
                PersistenceCollection.Add(_record);
            }

            public void ShouldStoreModifiedRecord()
            {
                var newVersion = PersistenceCollection[_record.Id];
                newVersion.Name = "the new";
                PersistenceCollection.Update(newVersion);

                using (var session = DocumentStore.OpenSession())
                {
                    var stored = session.Load<MockRecord>(_record.Id);
                    Assert.AreEqual(stored.Name, "the new");
                }
            }

            public void ShouldUpdateModifiedAtDate()
            {
                var newVersion = PersistenceCollection[_record.Id];
                newVersion.Name = "the new";
                PersistenceCollection.Update(newVersion);
                Assert.Less(_record.ModifiedAt, newVersion.ModifiedAt);
            }
        }
    }
}
