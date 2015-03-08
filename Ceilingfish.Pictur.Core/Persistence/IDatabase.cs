using Ceilingfish.Pictur.Core.Flickr;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;
using Raven.Database.Config.Settings;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public interface IDatabase
    {
        FileCollection Files { get; }

        RavenPersistenceCollection<Directory> Directories { get; }

        Settings Settings { get; }

        PluginResourceCollection<FlickrUpload> FlickrUploads { get; }
    }
}
