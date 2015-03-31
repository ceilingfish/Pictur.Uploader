using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ceilingfish.Pictur.Core.Flickr.Api
{
    public class PhotoSearchResult
    {
        [JsonProperty("photos")]
        public PhotoSearchResultPage ResultPage { get; set; }

        [JsonIgnore]
        internal IEnumerable<PhotoSearchInfo> Results
        {
            get { return ResultPage != null ? ResultPage.Results : Enumerable.Empty<PhotoSearchInfo>(); }
        }
    }

    public class PhotoSearchResultPage
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pages")]
        public int PageCount { get; set; }

        [JsonProperty("perpage")]
        public int PageSize { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("photo")]
        public IEnumerable<PhotoSearchInfo> Results { get; set; }
    }

}
