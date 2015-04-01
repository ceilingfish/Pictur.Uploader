using System.IO;
using System.Net;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using ImageMagick;
using System;
using System.Linq;
using Ceilingfish.Pictur.Core.Flickr.Api;
using System.Collections.Generic;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class PopulateUploadDiscovery : IExecutor<FlickrContext>
    {
        private readonly IDatabase _db;

        public PopulateUploadDiscovery(IDatabase db)
        {
            _db = db;
        }

        public void Execute(FlickrContext context)
        {
            switch (context.Type)
            {
                case FileOperationType.Added:
                    context.Upload = FindDuplicate(context);
                    break;
                default:
                    context.Upload = LookupUpload(context);
                    break;
            }
        }

        private FlickrUpload LookupUpload(FlickrContext context)
        {
            return _db.FlickrUploads.GetByFileId(context.File.Id);
        }

        private FlickrUpload FindDuplicate(FlickrContext context)
        {

            var upload = LookupUpload(context);

            if (upload != null)
                return upload;

            var wrapper = new ApiWrapper(_db);

            var name = Path.GetFileNameWithoutExtension(context.File.Path);

            var photos = wrapper
                        .SearchByName(name)
                        .Where(p => CompareCaptureDate(context.ImageData.GetExifProfile(), p.DateTaken));

            return FindDuplicateImage(context, photos);
        }

        private FlickrUpload FindDuplicateImage(FlickrContext context, IEnumerable<PhotoSearchInfo> photos)
        {
            foreach (var photo in photos)
            {
                //try and avoid loading image data if at all possible
                try
                {
                    var request = WebRequest.CreateHttp(photo.OriginalImageUrl);
                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    using (var image = new MagickImage(stream))
                    {
                        if (!CompareImageData(context.ImageData, image))
                        {
                            //Found duplicate!
                            var upload = new FlickrUpload { FileId = context.File.Id, PhotoId = photo.Id };

                            _db.FlickrUploads.Add(upload);

                            return upload;
                        }
                    }

                }
                catch (WebException)
                {

                }
            }

            return null;
        }


        private bool CompareCaptureDate(ExifProfile exifProfile, DateTime dateTime)
        {
            if (exifProfile == null)//no image data
                return false;

            var createdAtDateEntry = exifProfile.Values.SingleOrDefault(t => t.Tag == ExifTag.DateTimeOriginal);

            if (createdAtDateEntry == null)
                return false;

            DateTime createdAt;
            if (!DateTime.TryParse(createdAtDateEntry.Value.ToString(), out createdAt))
            {
                return false;
            }

            TimeSpan difference = createdAt - dateTime;

            return difference.TotalSeconds < 1.0;
        }

        private bool CompareImageData(MagickImage upload, MagickImage candidate)
        {
            candidate.Resize(upload.Width, upload.Height);
            var comparison = upload.Compare(candidate, ErrorMetric.NormalizedCrossCorrelation);

            return comparison > 0.95;//Pull from config
        }
    }
}
