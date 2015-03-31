using Newtonsoft.Json;
using System;

namespace Ceilingfish.Pictur.Core.Flickr.Api
{
    public class PhotoSearchInfo : BasePhotoInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ispublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("isfriend")]
        public bool IsFriend { get; set; }

        [JsonProperty("isfamily")]
        public bool IsFamily { get; set; }

        [JsonProperty("url_o")]
        public Uri OriginalImageUrl { get; set; }

        [JsonProperty("datetaken")]
        public DateTime DateTaken { get; set; }
    }
}
