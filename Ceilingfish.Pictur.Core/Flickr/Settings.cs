namespace Ceilingfish.Pictur.Core.Flickr
{
    public class Settings
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string OAuthToken { get; set; }
        public string UserId { get; set; }
        public FlickrStatus Status { get; set; }
        public AlbumStrategy AlbumStrategy { get; set; }
    }
}
