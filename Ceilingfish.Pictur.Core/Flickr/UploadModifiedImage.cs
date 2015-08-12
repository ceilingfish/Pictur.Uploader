using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class UploadModifiedImage : IExecutor<FlickrContext>
    {
        private readonly IDatabase _db;
        
        public void Execute(FlickrContext context)
        {
            var isCorrectOperation = context.FileOperation == FileOperationType.Modified || context.FileOperation == FileOperationType.Moved;
            var isCorrectState = context.UploadState == FlickrUploadState.New;

            if (isCorrectOperation && isCorrectState)
            {
                var flickr = new FlickrNet.Flickr(_db.Settings.Flickr.ApiKey);
                var id = flickr.UploadPicture(context.File.Path, GetName(context));
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
