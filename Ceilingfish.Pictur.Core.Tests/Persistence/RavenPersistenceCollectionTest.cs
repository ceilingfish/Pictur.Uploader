using Ceilingfish.Pictur.Core.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Models;
using Raven.Client.Embedded;

namespace Ceilingfish.Pictur.Core.Tests.Persistence
{
    public class RavenPersistenceCollectionTest
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
                System.IO.Directory.Delete(_path, true);
            };
        }

        public class Create_RavenDatabaseTests : RavenPersistenceCollectionTest
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
    }
}
