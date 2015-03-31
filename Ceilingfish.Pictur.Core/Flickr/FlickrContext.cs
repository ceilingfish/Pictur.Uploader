using System;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Pipeline;
using ImageMagick;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class FlickrContext : ExecutorContext, IDisposable
    {
        public FlickrUpload Upload { get; set; }

        private MagickImage _imageData;
        public MagickImage ImageData
        {
            get
            {
                if (_imageData == null)
                    _imageData = new MagickImage(File.Path);

                return _imageData;
            }
        }

        public FlickrContext(File file, FileOperationType type)
            : base(file, type)
        {
        }

        public void Dispose()
        {
            if (_imageData != null)
                _imageData.Dispose();
        }
    }
}
