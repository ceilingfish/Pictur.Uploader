using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using System.IO;

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
            if (context.FileOperation == FileOperationType.Added && context.UploadState == FlickrUploadState.New)
            {
                var api = new ApiWrapper(_db);
                var id = api.UploadPhoto(context.File.Path, GetName(context));
                api.UploadPhoto(context.File.Path, GetName(context));

                var record = new FlickrUpload { FileId = context.File.Id, PhotoId = id };

                _db.FlickrUploads.Add(record);
            }
        }

        private string GetName(FlickrContext context)
        {
            return Path.GetFileNameWithoutExtension(context.File.Path);
        }
    }
}
