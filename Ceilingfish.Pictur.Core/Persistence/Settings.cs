namespace Ceilingfish.Pictur.Core.Persistence
{
    public class Settings
    {
        private Flickr.Settings _flickr;

        public Flickr.Settings Flickr
        {
            get { return _flickr ?? (_flickr = new Flickr.Settings()); }
            set { _flickr = value; }
        }
    }
}
