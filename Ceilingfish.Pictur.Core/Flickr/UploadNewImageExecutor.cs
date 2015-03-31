using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class UploadNewImageExecutor : IExecutor<FlickrContext>
    {
        private readonly IDatabase _db;

        public UploadNewImageExecutor(IDatabase db)
        {
            _db = db;
        }

        public void Execute(FlickrContext context)
        {
            if (context.Type != FileOperationType.Added)
                return;

            var flickr = new FlickrNet.Flickr(_db.Settings.Flickr.ApiKey);

            var id = flickr.UploadPicture(context.File.Path);

            var record = new FlickrUpload { FileId = context.File.Id, PhotoId = id };

            _db.FlickrUploads.Add(record);
        }
    }
}
