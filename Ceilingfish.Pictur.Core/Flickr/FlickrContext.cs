using System;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Pipeline;
using ImageMagick;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class FlickrContext : ExecutorContext, IDisposable
    {
        internal FlickrUpload Upload { get; set; }

        internal FlickrUploadState UploadState { get; set; }

        private MagickImage _imageData;
        internal MagickImage ImageData
        {
            get
            {
                if (_imageData == null)
                    _imageData = new MagickImage(File.Path);

                return _imageData;
            }
        }

        internal FlickrContext(File file, FileOperationType type)
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
