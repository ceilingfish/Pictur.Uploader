﻿using System.Collections.Generic;
using System.Linq;
using Ceilingfish.Pictur.Core.Persistence;
using Raven.Client;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class FlickrUploadCollection : PluginResourceCollection<FlickrUpload>
    {
        public FlickrUploadCollection(IDocumentStore store)
            : base(store)
        {
        }

        public IEnumerable<FlickrUpload> GetByPhotoId(string flickrId)
        {
            using (var session = Store.OpenSession())
            {
                return session
                    .Query<FlickrUpload>()
                    .Where(p => p.PhotoId.Equals(flickrId));
            }
        }
    }
}
