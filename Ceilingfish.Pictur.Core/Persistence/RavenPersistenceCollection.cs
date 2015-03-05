using System;
using System.Collections;
using System.Collections.Generic;
using Ceilingfish.Pictur.Core.Models;
using Raven.Client.Embedded;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class RavenPersistenceCollection<T> : IEnumerable<T>
        where T : BaseRecord
    {
        protected readonly EmbeddableDocumentStore Store;

        public RavenPersistenceCollection(EmbeddableDocumentStore store)
        {
            Store = store;
        }

        public T this[string key]
        {
            get
            {
                using (var session = Store.OpenSession())
                {
                    return session.Load<T>(key);
                }
            }

            set
            {
                var record = value;
                if (record.Id == null)
                {
                    record.Id = key;
                    Add(record);
                }
                else
                {
                    record.Id = key;
                    Update(record);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            using (var session = Store.OpenSession())
            {
                return session.Query<T>().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(T item)
        {
            item.CreatedAt = DateTime.UtcNow;
            Update(item);
        }

        public virtual void Update(T item)
        {
            item.ModifiedAt = DateTime.UtcNow;
            using (var session = Store.OpenSession())
            {
                session.Store(item);
                session.SaveChanges();
            }
        }

        public virtual bool Remove(T item)
        {
            using (var session = Store.OpenSession())
            {
                var sessionItem = session.Load<T>(item.Id);
                if (sessionItem == null)
                    return false;
                session.Delete(sessionItem);
                session.SaveChanges();
                return true;
            }
        }

        public void Clear()
        {
            using (var session = Store.OpenSession())
            {
                var records = session.Query<T>();
                foreach (var record in records)
                {
                    session.Delete(record);
                }
                session.SaveChanges();
            }
        }
    }
}
