using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Helpers;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core
{
    public abstract class BaseUploader
    {
        private readonly IDatabase _database;

        protected BaseUploader(IDatabase db)
        {
            _database = db;
        }

        public void OnFileAdded(object sender, FileAddedArgs args)
        {
            var checksum = ChecksumHelper.GetMd5HashFromFile(args.FilePath);

            var current = _database.GetFileByCheckSum(checksum);

            if (current != null)
                return;

            var newFile = new Persistence.File
                          {
                              Path = args.FilePath,
                              Checksum = checksum
                          };

            Upload(newFile);
        }

        public void OnFileRemoved(object sender, FileRemovedArgs args)
        {
            var current = _database.GetFileByPath(args.FilePath);

            if (current == null)
                return;

            Delete(current);
        }

        protected abstract void Upload(Persistence.File file);

        protected abstract void Delete(Persistence.File file);
    }
}
