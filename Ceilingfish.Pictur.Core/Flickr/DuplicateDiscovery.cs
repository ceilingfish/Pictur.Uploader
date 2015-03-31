using System.IO;
using System.Net;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using ImageMagick;
using System;
using System.Linq;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class DuplicateDiscovery : IExecutor<FlickrContext>
    {
        private readonly IDatabase _db;

        public DuplicateDiscovery(IDatabase db)
        {
            _db = db;
        }

        public void Execute(FlickrContext context)
        {
            var wrapper = new ApiWrapper(_db);

            var name = Path.GetFileNameWithoutExtension(context.File.Path);

            var photos = wrapper.SearchByName(name);

            foreach (var photo in photos)
            {
                if (!CompareCaptureDate(context.ImageData.GetExifProfile(), photo.DateTaken))
                    continue;

                //try and avoid loading image data if at all possible
                try
                {
                    var request = WebRequest.CreateHttp(photo.OriginalImageUrl);
                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    using (var image = new MagickImage(stream))
                    {
                        if (!CompareImageData(context.ImageData, image))
                            continue;

                        var upload = _db.FlickrUploads.GetPhotoByFlickrIdAndFileId(photo.Id, context.File.Id);
                        if (upload != null)
                        {
                            context.Upload = upload;
                            return;
                        }
                    }

                }
                catch (WebException)
                {

                }
            }

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
