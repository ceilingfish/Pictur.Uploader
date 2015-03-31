using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Flickr.Api
{
    public class BasePhotoInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("owner")]
        public string FlickrUserId { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("farm")]
        public int FarmId { get; set; }

        public Uri ImageUri
        {
            get
            {
                return new Uri(string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}.jpg", FarmId, Server, Id, Secret));
            }
        }

        public Uri GetImageUri(ImageSize size)
        {
            return new Uri(string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_{4}.jpg", FarmId, Server, Id, Secret, (char)size));
        }

    }
}
