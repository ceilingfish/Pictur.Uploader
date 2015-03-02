using Ceilingfish.Pictur.Core.Models;

namespace Ceilingfish.Pictur.Core.Events
{
    public class DirectoryRemovedArgs
    {
        public readonly Directory Directory;

        public DirectoryRemovedArgs(Directory d)
        {
            Directory = d;
        }
    }
}
