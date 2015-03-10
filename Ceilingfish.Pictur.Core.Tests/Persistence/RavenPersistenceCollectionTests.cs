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
        protected MockRecord Record;
        protected string RecordId;

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
            RecordId = Guid.NewGuid().ToString();
            Record = new MockRecord { Name = "the original", Id = RecordId };
            PersistenceCollection.Add(Record);
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
            public void ShouldStoreModifiedRecord()
            {
                var newVersion = PersistenceCollection[RecordId];
                newVersion.Name = "the new";
                PersistenceCollection.Update(newVersion);

                using (var session = DocumentStore.OpenSession())
                {
                    var stored = session.Load<MockRecord>(Record.Id);
                    Assert.AreEqual(stored.Name, "the new");
                }
            }

            public void ShouldUpdateModifiedAtDate()
            {
                var newVersion = PersistenceCollection[RecordId];
                newVersion.Name = "the new";
                PersistenceCollection.Update(newVersion);
                Assert.Less(Record.ModifiedAt, newVersion.ModifiedAt);
            }
        }

        public class Remove_RavenPersistenceCollectionTests : RavenPersistenceCollectionTests
        {
            public void ShouldRemoveRecord()
            {
                PersistenceCollection.Remove(Record);

                using (var session = DocumentStore.OpenSession())
                {
                    var nothing = session.Load<MockRecord>(RecordId);
                    Assert.IsNull(nothing);
                }
            }
        }

        public class Clear_RavenPersistenceCollectionTests : RavenPersistenceCollectionTests
        {
            public void ShouldDeleteAllRecords()
            {
                PersistenceCollection.Add(new MockRecord { Name = "angry" });
                PersistenceCollection.Clear();
                Assert.IsFalse(PersistenceCollection.Any());
            }
        }

        public class IndexOperator_RavenPersistenceCollectionTests : RavenPersistenceCollectionTests
        {
            public void ShouldGetRecordsByPrimaryKey()
            {
                var storedItem = PersistenceCollection[RecordId];
                Assert.IsNotNull(storedItem);
                Assert.AreEqual(storedItem.Name, Record.Name);
            }

            public void ShouldAddRecordIfPrimaryKeyDoesNotExist()
            {
                var newRecord = new MockRecord { Name = "geoffer" };
                PersistenceCollection["kittenhat"] = newRecord;

                using (var session = DocumentStore.OpenSession())
                {
                    var stored = session.Load<MockRecord>("kittenhat");
                    Assert.IsNotNull(stored);
                    Assert.AreEqual(stored.Id, "kittenhat");
                    Assert.AreEqual(stored.Name, "geoffer");
                }
            }

            public void ShouldUpdateRecordIfPrimaryKeyExists()
            {
                var newRecord = new MockRecord { Name = "wump" };
                PersistenceCollection[RecordId] = newRecord;

                using (var session = DocumentStore.OpenSession())
                {
                    var stored = session.Load<MockRecord>(RecordId);
                    Assert.IsNotNull(stored);
                    Assert.AreEqual(stored.Id, RecordId);
                    Assert.AreEqual(stored.Name, "wump");
                }
            }
        }

        public class Enumerable_RavenPersistenceCollectionTests : RavenPersistenceCollectionTests
        {
            public void ShouldEnumerateOverAllRecords()
            {

            }
        }
    }
}
