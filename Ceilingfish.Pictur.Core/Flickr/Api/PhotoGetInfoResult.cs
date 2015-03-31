using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Flickr.Api
{
    public class PhotoGetInfoResult : BasePhotoInfo
    {
        [JsonProperty("originalsecret")]
        public string OriginalSecret { get; set; }

        [JsonProperty("originalformat")]
        public string OriginalFormat { get; set; }

        public Uri OriginalImageUri
        {
            get
            {
                return new Uri(string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_o.{4}", FarmId, Server, Id, OriginalSecret, OriginalFormat));
            }
        }
    }
}
