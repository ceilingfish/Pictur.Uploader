using System.IO;
using Passive;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class ManagedDirectory
    {

        private DirectoryInfo _dirInfo;
        internal DirectoryInfo Directory
        {
            get { return _dirInfo ?? (_dirInfo = new DirectoryInfo(Path)); }
        }
    }
}
