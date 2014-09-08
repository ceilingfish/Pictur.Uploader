using System;
using System.Configuration;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.IO;
using System.Linq;
using Ceilingfish.Pictur.Core.Persistence;
using Passive;
using Passive.Dialect;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class PersistenceController : IController
    {
        private DynamicDatabase _db;

        public ObjectSet<Persistence.File> Files
        {
            get { return _db.; }
        }

        public ObjectSet<ManagedDirectory> ManagedDirectories
        {
            get { return _container.ManagedDirectories; }
        }

        public PersistenceController()
        {
            var localAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var sqlFilePath = Path.Combine(localAppFolder, "Pictur", "pictur.db");

            LoadDatabase(sqlFilePath);
        }

        public PersistenceController(string path)
        {
            LoadDatabase(path);
        }

        private void LoadDatabase(string sqlFilePath)
        {
            _db = new DynamicDatabase(String.Format("", sqlFilePath), String.Format("", sqlFilePath));
        }

        internal Persistence.File FindFileByPath(string p)
        {
            return Files.First(f => f.Path.Equals(p));
        }

        internal Persistence.File FindFileByChecksum(string checksum)
        {
            return Files.First(f => f.Checksum.Equals(checksum));
        }

        internal void Save()
        {
            _container.SaveChanges();
        }
    }
}
