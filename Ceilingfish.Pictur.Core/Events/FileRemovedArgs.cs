using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Events
{
    public class FileRemovedArgs : EventArgs
    {
        public readonly ManagedDirectory Directory;
        public readonly string FilePath;

        public FileRemovedArgs(ManagedDirectory dir, string path)
        {
            Directory = dir;
            FilePath = path;
        }
    }
}
